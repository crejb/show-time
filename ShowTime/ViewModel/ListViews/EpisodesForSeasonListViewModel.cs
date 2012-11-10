using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Data;
using System.Globalization;
using ShowTime.Services;

namespace ShowTime.ViewModel.ListViews
{
    public class EpisodesForSeasonListViewModel : ViewModelBase
    {
        private IDataStore dataManager;
        private IEpisodeThumbnailFilenameProvider thumbnailProvider;
        private Season season;
        private TVShow TVShow
        {
            get { return dataManager.TVShowRepository.Find(season.TVShowId); }
        }
         
        public string SeasonName
        {
            get { return TVShow.Name + " Season " + season.Number; }
        }

        public IEnumerable<EpisodesForSeasonListViewModel_EpisodeListViewItemViewModel> Episodes
        {
            get
            {
                return dataManager.EpisodeRepository
                    .Query(ep => ep.SeasonId.Equals(season.Id))
                    .Select(e => new EpisodesForSeasonListViewModel_EpisodeListViewItemViewModel(
                        e, 
                        thumbnailProvider, 
                        dataManager.BookmarkRepository.Find(new BookmarkId(e.Id)),
                        dataManager.LastWatchedRepository.Query(entry => entry.EpisodeId.Equals(e.Id)).FirstOrDefault()
                        ));
            }
        }

        private EpisodesForSeasonListViewModel_EpisodeListViewItemViewModel selectedEpisode = null;
        public EpisodesForSeasonListViewModel_EpisodeListViewItemViewModel SelectedEpisode
        {
            get { return selectedEpisode; }
            set
            {
                if (value == selectedEpisode)
                    return;

                selectedEpisode = value;

                base.OnPropertyChanged("SelectedEpisode");
            }
        }

        public EpisodesForSeasonListViewModel(IDataStore dataManager, IEpisodeThumbnailFilenameProvider thumbnailProvider, Season season)
        {
            this.dataManager = dataManager;
            this.thumbnailProvider = thumbnailProvider;
            this.season = season;
            this.DisplayName = Strings.SeasonEpisodesViewModel_DisplayName;
        }

        public override string ToString()
        {
            return SeasonName;
        }
    }

    #region Helper Classes- should be nested classes within TVShowSeasonsListViewModel but that doesn't work with XAML :(
    public class EpisodesForSeasonListViewModel_EpisodeListViewItemViewModel : ViewModelBase
    {
        public readonly Episode Episode;
        public readonly Bookmark Bookmark;
        public readonly LastWatchedEntry LastWatchedEntry;

        public int Number
        {
            get { return Episode.Number; }
        }

        public string Title
        {
            get { return Episode.Title; }
        }

        public string Description
        {
            get { return Episode.Description; }
        }

        public string ShortFilename
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(Episode.Filename); }
        }

        public string ThumbnailFilename
        {
            get; private set;
        }

        public TimeSpan? BookmarkTime
        {
            get { return Bookmark != null ? Bookmark.Time : (TimeSpan?)null; }
        }

        public int WatchCount
        {
            get { return LastWatchedEntry != null ? 1 : 0; }
        }

        public string LastWatchedDescription
        {
            get; private set;
        }

        public EpisodesForSeasonListViewModel_EpisodeListViewItemViewModel(Episode episode, IEpisodeThumbnailFilenameProvider thumbnailProvider, Bookmark bookmark = null, LastWatchedEntry lastWatchedEntry = null)
        {
            this.Episode = episode;
            this.ThumbnailFilename = thumbnailProvider.GetThumbnailFilenameForEpisode(episode).ActualFilename;

            Bookmark = bookmark;
            LastWatchedEntry = lastWatchedEntry;

            if (LastWatchedEntry != null)
            {
                var dateTimeStringConverter = new DateTimeToHumanReadableFormatConverter();
                LastWatchedDescription = dateTimeStringConverter.ConvertDateTimeToHumanReadableFormat(LastWatchedEntry.Time);
            }
        }
    }

    #endregion
}

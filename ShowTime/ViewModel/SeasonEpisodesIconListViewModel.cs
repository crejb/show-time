using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Data;
using System.Globalization;
using ShowTime.Services;

namespace ShowTime.ViewModel
{
    public class SeasonEpisodesIconListViewModel : ViewModelBase
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

        public IEnumerable<SeasonEpisodesIconListViewModel_EpisodeIconViewModel> Episodes
        {
            get
            {
                return dataManager.EpisodeRepository
                    .Query(ep => ep.SeasonId.Equals(season.Id))
                    .Select(e => new SeasonEpisodesIconListViewModel_EpisodeIconViewModel(e, thumbnailProvider));
            }
        }

        private SeasonEpisodesIconListViewModel_EpisodeIconViewModel selectedEpisode = null;
        public SeasonEpisodesIconListViewModel_EpisodeIconViewModel SelectedEpisode
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

        public SeasonEpisodesIconListViewModel(IDataStore dataManager, IEpisodeThumbnailFilenameProvider thumbnailProvider, Season season)
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
    public class SeasonEpisodesIconListViewModel_EpisodeIconViewModel : ViewModelBase
    {
        public readonly Episode Episode;

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

        public SeasonEpisodesIconListViewModel_EpisodeIconViewModel(Episode episode, IEpisodeThumbnailFilenameProvider thumbnailProvider)
        {
            this.Episode = episode;
            this.ThumbnailFilename = thumbnailProvider.GetThumbnailFilenameForEpisode(episode).ActualFilename;
        }
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;
using ShowTime.Model;

namespace ShowTime.ViewModel.IconListViews
{
    public class EpisodeIconListViewModel : ViewModelBase
    {
        public event Action<EpisodeId> EpisodeSelected;

        public ICommand SelectEpisodeCommand { get; private set; }

        private Episode selectedEpisode;
        public Episode SelectedEpisode
        {
            get { return selectedEpisode; }
            set
            {
                if (selectedEpisode == value)
                    return;

                selectedEpisode = value;
                OnPropertyChanged("SelectedEpisode");
                SelectedEpisodePreviewViewModel = new EpisodePreviewViewModel(dataStore, selectedEpisode.Id, thumbnailProvider);
            }
        }

        private EpisodePreviewViewModel selectedEpisodePreviewViewModel;
        public EpisodePreviewViewModel SelectedEpisodePreviewViewModel
        {
            get { return selectedEpisodePreviewViewModel; }
            set
            {
                if (selectedEpisodePreviewViewModel == value)
                    return;

                selectedEpisodePreviewViewModel = value;
                OnPropertyChanged("SelectedEpisodePreviewViewModel");
            }
        }

        private readonly IDataStore dataStore;
        private readonly Services.IEpisodeThumbnailFilenameProvider thumbnailProvider;
        private readonly Season season;
        public IEnumerable<MenuItemCommand> MenuItemCommands { get { return BuildMenuItemCommands(); } }

        public EpisodeIconListViewModel(IDataStore dataStore, SeasonId seasonId, Services.IEpisodeThumbnailFilenameProvider thumbnailProvider)
        {
            this.dataStore = dataStore;
            this.thumbnailProvider = thumbnailProvider;
            this.season = dataStore.SeasonRepository.Find(seasonId);
            this.SelectEpisodeCommand = new RelayCommand(param => OnEpisodeSelected((param as MenuItemCommand).Tag as EpisodeId));
        }

        private IEnumerable<MenuItemCommand> BuildMenuItemCommands()
        {
            return dataStore.EpisodeRepository.Query(episode => episode.SeasonId.Equals(season.Id))
                .OrderBy(episode => episode.Number)
                .Select(
                    episode=>
                    new EpisodeIconMenuItemCommand(
                        episode,
                        thumbnailProvider,
                        null,
                        null,
                        dataStore.BookmarkRepository.Find(new BookmarkId(episode.Id)),
                        dataStore.LastWatchedRepository.Query(entry => entry.EpisodeId.Equals(episode.Id)).FirstOrDefault()
                    ));
        }

        private void OnEpisodeSelected(EpisodeId episodeId)
        {
            SelectedEpisode = dataStore.EpisodeRepository.Find(episodeId);

            var handler = EpisodeSelected;
            if (handler != null)
                handler(episodeId);
        }
    }

    public class EpisodeIconMenuItemCommand : MenuItemCommand
    {
        public bool HasBookmark { get; private set; }
        public bool HasLastWatched { get; private set; }
        public string LastWatchedDescription { get; private set; }

        public EpisodeIconMenuItemCommand(
            Episode episode,
            Services.IEpisodeThumbnailFilenameProvider thumbnailProvider,
            ICommand selectedCommand, 
            ICommand confirmedCommand,
            Bookmark bookmark = null,
            LastWatchedEntry lastWatchedEntry = null
            )
            : base(
                    episode.Number + ". " + episode.Title,
                    MenuItemCommand.BuildImageFromFile(thumbnailProvider.GetThumbnailFilenameForEpisode(episode).ActualFilename),
                    episode.Id, selectedCommand, confirmedCommand)
        {
            HasBookmark = bookmark != null;

            if (lastWatchedEntry != null)
            {
                HasLastWatched = true;
                var dateTimeStringConverter = new Services.DateTimeToHumanReadableFormatConverter();
                LastWatchedDescription = dateTimeStringConverter.ConvertDateTimeToHumanReadableFormat(lastWatchedEntry.Time);
            }
        }
    }
}

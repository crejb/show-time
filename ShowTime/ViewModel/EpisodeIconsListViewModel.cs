using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;
using ShowTime.Model;

namespace ShowTime.ViewModel
{
    public class EpisodeIconsListViewModel : ViewModelBase
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

        public EpisodeIconsListViewModel(IDataStore dataStore, SeasonId seasonId, Services.IEpisodeThumbnailFilenameProvider thumbnailProvider)
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
                    new MenuItemCommand(
                        episode.Number + ". " + episode.Title,
                        "../Resources/tv.jpg",
                        episode.Id,
                        null,
                        null
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
}

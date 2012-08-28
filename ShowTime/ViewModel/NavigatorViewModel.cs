using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;
using System.Collections.ObjectModel;
using ShowTime.Model;

namespace ShowTime.ViewModel
{
    public class NavigatorViewModel : ViewModelBase
    {
        public event Action<ViewModelBase> NavigateToViewRequested;
        public ViewModelBase CurrentViewModel { get; set; }

        private ObservableCollection<CommandViewModel> commandList;
        public ObservableCollection<CommandViewModel> CommandList
        {
            get { return commandList; }
            set
            {
                if (value == commandList)
                    return;

                commandList = value;
                base.OnPropertyChanged("CommandList");
            }
        }

        private readonly IDataStore dataStore;

        private MainMenuViewModel mainMenuViewModel;
        private TvShowIconsListViewModel tvShowListViewModel;
        private BrowseAllShowsViewModel browseAllShowsViewModel;
        private UpdateShowTimeCollectionViewModel updateDataViewModel;

        private CommandViewModel homeCommand;
        private CommandViewModel updateCommand;
        private CommandViewModel showsCommand;
        private Services.IEpisodeThumbnailFilenameProvider episodeThumbnailProvider;

        public NavigatorViewModel(IDataStore dataStore, Services.IEpisodeThumbnailFilenameProvider episodeThumbnailProvider, Services.Providers.ITVShowDiscovererProvider discovererProvider)
        {
            this.dataStore = dataStore;
            this.episodeThumbnailProvider = episodeThumbnailProvider;

            homeCommand = new CommandViewModel("Home", new RelayCommand(param => OnHomeCommandExecuted()));
            updateCommand = new CommandViewModel("Update Shows", new RelayCommand(param => OnUpdateCommandExecuted()));
            showsCommand = new CommandViewModel("View Shows", new RelayCommand(param => OnShowsCommandExecuted()));

            mainMenuViewModel = new MainMenuViewModel();
            mainMenuViewModel.WatchShowsSelected += new Action(mainMenuViewModel_WatchShowsSelected);
            mainMenuViewModel.BrowseShowsSelected += new Action(mainMenuViewModel_BrowseShowsSelected);
            mainMenuViewModel.ManageShowsSelected += new Action(mainMenuViewModel_ManageShowsSelected);

            tvShowListViewModel = new TvShowIconsListViewModel(dataStore);
            tvShowListViewModel.TvShowSelected += tvShowListViewModel_TvShowSelected;


            browseAllShowsViewModel = new BrowseAllShowsViewModel(dataStore, episodeThumbnailProvider);
            updateDataViewModel = new UpdateShowTimeCollectionViewModel(dataStore, discovererProvider);

            commandList = new ObservableCollection<CommandViewModel> { homeCommand, showsCommand, updateCommand };

            OnNavigateToViewRequested(mainMenuViewModel);
        }




        private void mainMenuViewModel_WatchShowsSelected()
        {
            OnNavigateToViewRequested(tvShowListViewModel);
        }

        private void tvShowListViewModel_TvShowSelected(TVShowId showId)
        {
            var seasonsListViewModel = new SeasonIconsListViewModel(dataStore, showId);
            seasonsListViewModel.SeasonSelected += seasonsListViewModel_SeasonSelected;
            OnNavigateToViewRequested(seasonsListViewModel);
        }

        private void seasonsListViewModel_SeasonSelected(SeasonId seasonId)
        {
            var episodesListViewModel = new EpisodeIconsListViewModel(dataStore, seasonId, episodeThumbnailProvider);
            episodesListViewModel.EpisodeSelected += episodesListViewModel_EpisodeSelected;
            OnNavigateToViewRequested(episodesListViewModel);
        }

        private void episodesListViewModel_EpisodeSelected(EpisodeId obj)
        {
            
        }




        private void OnShowsCommandExecuted()
        {
            OnNavigateToViewRequested(browseAllShowsViewModel);
        }

        private void OnUpdateCommandExecuted()
        {
            OnNavigateToViewRequested(updateDataViewModel);
        }

        private void OnHomeCommandExecuted()
        {
            OnNavigateToViewRequested(mainMenuViewModel);
        }

        

        private void mainMenuViewModel_BrowseShowsSelected()
        {
            OnNavigateToViewRequested(browseAllShowsViewModel);
        }

        private void mainMenuViewModel_ManageShowsSelected()
        {
            OnNavigateToViewRequested(updateDataViewModel);
        }

        private void OnNavigateToViewRequested(ViewModelBase viewModel)
        {
            CurrentViewModel = viewModel;

            var handler = NavigateToViewRequested;
            if (handler != null)
                handler(viewModel);
        }
    }
}

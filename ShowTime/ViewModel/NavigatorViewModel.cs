using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;
using System.Collections.ObjectModel;
using ShowTime.Model;
using ShowTime.View.Controls.BreadCrumbControl;

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

        private BreadCrumbModel breadCrumbViewModel;
        public BreadCrumbModel BreadCrumbViewModel
        {
            get { return breadCrumbViewModel; }
            set
            {
                if (breadCrumbViewModel == value)
                    return;

                breadCrumbViewModel = value;
                base.OnPropertyChanged("BreadCrumbViewModel");
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
        private Services.IEpisodeThumbnailGenerator episodeThumbnailGenerator;

        public NavigatorViewModel(IDataStore dataStore, Services.IEpisodeThumbnailGenerator episodeThumbnailGenerator, Services.Providers.ITVShowDiscovererProvider discovererProvider)
        {
            this.dataStore = dataStore;
            this.episodeThumbnailGenerator = episodeThumbnailGenerator;

            homeCommand = new CommandViewModel("Home", new RelayCommand(param => OnHomeCommandExecuted()));
            updateCommand = new CommandViewModel("Update Shows", new RelayCommand(param => OnUpdateCommandExecuted()));
            showsCommand = new CommandViewModel("View Shows", new RelayCommand(param => OnShowsCommandExecuted()));

            mainMenuViewModel = new MainMenuViewModel();
            mainMenuViewModel.WatchShowsSelected += new Action(mainMenuViewModel_WatchShowsSelected);
            mainMenuViewModel.BrowseShowsSelected += new Action(mainMenuViewModel_BrowseShowsSelected);
            mainMenuViewModel.ManageShowsSelected += new Action(mainMenuViewModel_ManageShowsSelected);

            tvShowListViewModel = new TvShowIconsListViewModel(dataStore);
            tvShowListViewModel.TvShowSelected += tvShowListViewModel_TvShowSelected;

            browseAllShowsViewModel = new BrowseAllShowsViewModel(dataStore, episodeThumbnailGenerator.FilenameProvider);
            updateDataViewModel = new UpdateShowTimeCollectionViewModel(dataStore, discovererProvider, episodeThumbnailGenerator);

            commandList = new ObservableCollection<CommandViewModel> { homeCommand, showsCommand, updateCommand };


            breadCrumbViewModel = new BreadCrumbModel();
            breadCrumbViewModel.BreadCrumbItems = new System.Collections.ObjectModel.ObservableCollection<BreadCrumbItem>
            {
                new BreadCrumbHeadItem("Home",new RelayCommand(param => OnHomeCommandExecuted()))
            };
            //{
            //    new BreadCrumbHeadItem("Home", new RelayCommand(param => HomeItemClicked())),
            //    new BreadCrumbItem("Shows", new RelayCommand(param => ShowsItemClicked())),
            //    new BreadCrumbItem("Really really long TV show name", new RelayCommand(param => SeasonsItemClicked())),
            //    new BreadCrumbTailItem("Season 1", new RelayCommand(param => HeadItemClicked())),
            //};

            OnNavigateToViewRequested(mainMenuViewModel);
        }


        private void mainMenuViewModel_WatchShowsSelected()
        {
            breadCrumbViewModel.BreadCrumbItems.Clear();
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbHeadItem("Home", new RelayCommand(param => OnHomeCommandExecuted())));
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbTailItem("TV Shows", null));


            OnNavigateToViewRequested(tvShowListViewModel);
        }

        private void tvShowListViewModel_TvShowSelected(TVShowId showId)
        {
            breadCrumbViewModel.BreadCrumbItems.Clear();
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbHeadItem("Home", new RelayCommand(param => OnHomeCommandExecuted())));
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbItem("TV Shows", new RelayCommand(param => mainMenuViewModel_WatchShowsSelected())));
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbTailItem(showId.Name, null));

            var seasonsListViewModel = new SeasonIconsListViewModel(dataStore, showId);
            seasonsListViewModel.SeasonSelected += seasonsListViewModel_SeasonSelected;
            OnNavigateToViewRequested(seasonsListViewModel);
        }

        private void seasonsListViewModel_SeasonSelected(SeasonId seasonId)
        {
            breadCrumbViewModel.BreadCrumbItems.Clear();
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbHeadItem("Home", new RelayCommand(param => OnHomeCommandExecuted())));
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbItem("TV Shows", new RelayCommand(param => mainMenuViewModel_WatchShowsSelected())));
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbItem(seasonId.ShowId.Name, new RelayCommand(param => tvShowListViewModel_TvShowSelected(seasonId.ShowId))));
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbTailItem("Season " + seasonId.SeasonNumber, null));

            var episodesListViewModel = new EpisodeIconsListViewModel(dataStore, seasonId, episodeThumbnailGenerator.FilenameProvider);
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
            breadCrumbViewModel.BreadCrumbItems.Clear();
            breadCrumbViewModel.BreadCrumbItems.Add(new BreadCrumbHeadItem("Home", new RelayCommand(param => OnHomeCommandExecuted())));
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

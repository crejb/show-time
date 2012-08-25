using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;

namespace ShowTime.ViewModel
{
    public class NavigatorViewModel : ViewModelBase
    {
        public event Action<ViewModelBase> NavigateToViewRequested;
        public ViewModelBase CurrentViewModel { get; set; }

        private MainMenuViewModel mainMenuViewModel;
        private BrowseAllShowsViewModel browseAllShowsViewModel;
        private UpdateShowTimeCollectionViewModel updateDataViewModel;
        //private IDataStore dataStore;
        //private Services.IEpisodeThumbnailFilenameProvider episodeThumbnailProvider;
        //private Services.Providers.ITVShowDiscovererProvider discovererProvider;

        public ICommand HomeCommand { get; private set; }

        public NavigatorViewModel(IDataStore dataStore, Services.IEpisodeThumbnailFilenameProvider episodeThumbnailProvider, Services.Providers.ITVShowDiscovererProvider discovererProvider)
        {
            mainMenuViewModel = new MainMenuViewModel();
            mainMenuViewModel.WatchShowsSelected += new Action(mainMenuViewModel_WatchShowsSelected);
            mainMenuViewModel.ManageShowsSelected += new Action(mainMenuViewModel_ManageShowsSelected);

            browseAllShowsViewModel = new BrowseAllShowsViewModel(dataStore, episodeThumbnailProvider);
            updateDataViewModel = new UpdateShowTimeCollectionViewModel(dataStore, discovererProvider);

            OnNavigateToViewRequested(mainMenuViewModel);

            HomeCommand = new RelayCommand(param => OnNavigateToViewRequested(mainMenuViewModel));
        }

        private void mainMenuViewModel_WatchShowsSelected()
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

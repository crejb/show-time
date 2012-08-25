using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ShowTime.ViewModel;
using ShowTime.Model;
using ShowTime.Repositories;
using ShowTime.Services;
using ShowTime.DataLoading;
using ShowTime.Services.EpisodeDetailsBuilders;
using ShowTime.View;
using ShowTime.Services.Providers;

namespace ShowTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IDataStore dataStore;
        private IEpisodeThumbnailFilenameProvider episodeThumbnailFilenameProvider;
        private IEpisodeThumbnailGenerator episodeThumbnailGenerator;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow window = new MainWindow();

            dataStore = CreateDataStore();
            LoadInitialData(dataStore);

            episodeThumbnailFilenameProvider = new MockEpisodeThumbnailFilenameProvider();
            episodeThumbnailGenerator = new FFMpegEpisodeThumbnailGenerator(episodeThumbnailFilenameProvider);

            //// Create the ViewModel to which the main window binds.
            //BrowseAllShowsViewModel browseAllShowsViewModel = new BrowseAllShowsViewModel(dataStore, episodeThumbnailFilenameProvider);
            //window.BrowseAllView.DataContext = browseAllShowsViewModel;

            var directoryScannerProvider = new DirectoryScannerProvider(new EpisodeDetailsBuilder(
                    new ShowTime.Services.EpisodeDetailsBuilders.ShowAttributeBuilder(),
                    new ShowTime.Services.EpisodeDetailsBuilders.SeasonAttributeBuilder(),
                    new ShowTime.Services.EpisodeDetailsBuilders.EpisodeAttributeBuilder()));
            var discovererProvider = new TVShowDiscovererProvider(dataStore, directoryScannerProvider);

            //window.UpdateView.DataContext = new MainMenuViewModel(); // new UpdateShowTimeCollectionViewModel(dataStore, discovererProvider);
            NavigatorViewModel navigator = new NavigatorViewModel(dataStore, episodeThumbnailFilenameProvider, discovererProvider);
            MainWindowViewModel mainViewModel = new MainWindowViewModel(navigator);
            window.DataContext = mainViewModel;
            window.ctrlNavigator.DataContext = navigator;

            window.Show();
        }

        private static IDataStore CreateDataStore()
        {
            var tvShowRepo = new MockTVShowRepository();
            var seasonRepo = new MockSeasonRepository();
            var episodeRepo = new MockEpisodeRepository();

            return new ShowTimeDataStore(tvShowRepo, seasonRepo, episodeRepo);
        }

        private void LoadInitialData(IDataStore dataManager)
        {
            var dataLoader = CreateDataLoader();
            dataLoader.LoadData(dataManager);
        }

        private IDataLoader CreateDataLoader()
        {
            //return new HardcodedDataLoader();
            return new DirectoryParsingDataLoader(@"C:\Users\Chris\Videos\Empty",
                new EpisodeDetailsBuilder(
                    new ShowTime.Services.EpisodeDetailsBuilders.ShowAttributeBuilder(),
                    new ShowTime.Services.EpisodeDetailsBuilders.SeasonAttributeBuilder(),
                    new ShowTime.Services.EpisodeDetailsBuilders.EpisodeAttributeBuilder()));
        }
    }

}

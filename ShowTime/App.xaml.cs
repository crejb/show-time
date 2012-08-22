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

namespace ShowTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IDataStore dataManager;
        private IEpisodeThumbnailFilenameProvider episodeThumbnailFilenameProvider;
        private IEpisodeThumbnailGenerator episodeThumbnailGenerator;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow window = new MainWindow();

            dataManager = CreateDataStore();
            LoadInitialData(dataManager);

            episodeThumbnailFilenameProvider = new MockEpisodeThumbnailFilenameProvider();
            episodeThumbnailGenerator = new FFMpegEpisodeThumbnailGenerator(episodeThumbnailFilenameProvider);

            // Create the ViewModel to which the main window binds.
            BrowseAllShowsViewModel browseAllShowsViewModel = new BrowseAllShowsViewModel(dataManager, episodeThumbnailFilenameProvider);
            window.BrowseAllView.DataContext = browseAllShowsViewModel;

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
            return new DirectoryParsingDataLoader(@"C:\Users\Chris\Videos",
                new EpisodeDetailsBuilder(
                    new ShowTime.Services.Guessers.ShowGuesser(),
                    new ShowTime.Services.Guessers.SeasonGuesser(),
                    new ShowTime.Services.Guessers.EpisodeGuesser()));
        }
    }

}

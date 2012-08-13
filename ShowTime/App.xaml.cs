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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow window = new MainWindow();

            //var dataManager2 = new DataManager(new MockTVShowRepository(), new MockSeasonRepository(), new MockEpisodeRepository());
            //dataManager2.Load();

            var tvShowRepo = new MockTVShowRepository();
            var seasonRepo = new MockSeasonRepository();
            var episodeRepo = new MockEpisodeRepository();
            DataManager dataManager = new DataManager(tvShowRepo, seasonRepo, episodeRepo);

            IEpisodeThumbnailFilenameProvider episodeThumbnailFilenameProvider= new MockEpisodeThumbnailFilenameProvider();
            IEpisodeThumbnailGenerator episodeThumbnailGenerator = new FFMpegEpisodeThumbnailGenerator(episodeThumbnailFilenameProvider);

            var dataLoader = CreateDataLoader();
            dataLoader.LoadData(dataManager);

            // Create the ViewModel to which the main window binds.
            BrowseAllShowsViewModel browseAllShowsViewModel = new BrowseAllShowsViewModel(dataManager, episodeThumbnailFilenameProvider);
            window.BrowseAllView.DataContext = browseAllShowsViewModel;

            window.Show();
        }

        private IDataLoader CreateDataLoader()
        {
            //return new HardcodedDataLoader();
            return new DirectoryParsingDataLoader(@"C:\Users\Chris\Videos",
                new TvShowDiscoveryService(
                    new ShowTime.Services.Guessers.ShowGuesser(),
                    new ShowTime.Services.Guessers.SeasonGuesser(),
                    new ShowTime.Services.Guessers.EpisodeGuesser()));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ShowTime.ViewModel;
using ShowTime.Model;
using ShowTime.Repositories;

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

            foreach (string showName in new string[] { "The Big Bang Theory", "Masterchef" })
            {
                var show = CreateTvShow(showName);
                var seasons = CreateSeasons(show);
                foreach (var season in seasons)
                {
                    var episodes = CreateEpisodes(show, season);
                    foreach (var episode in episodes)
                    {
                        episodeRepo.Insert(episode);
                    }
                    seasonRepo.Insert(season);
                }
                tvShowRepo.Insert(show);
            }

            DataManager dataManager = new DataManager(tvShowRepo, seasonRepo, episodeRepo);

            // Create the ViewModel to which the main window binds.
            BrowseAllShowsViewModel browseAllShowsViewModel = new BrowseAllShowsViewModel(dataManager, new MockEpisodeThumbnailProvider());
            window.BrowseAllView.DataContext = browseAllShowsViewModel;

            window.Show();
        }

        private TVShow CreateTvShow(string title)
        {
            return new TVShow(
                title,
                "description for " + title);
        }

        private List<Season> CreateSeasons(TVShow show)
        {
            var seasons = new List<Season>();
            for (int i = 1; i < 3; i++)
            {
                seasons.Add(new Season(show.Id, i));
            }
            return seasons;
        }

        private List<Episode> CreateEpisodes(TVShow show, Season season)
        {
            var episodes = new List<Episode>();
            for (int i = 1; i < 6; i++)
            {
                episodes.Add(new Episode(show.Id, season.Id, i, "Episode " + i + " of " + show.Name, "Funny" + i + " of " + show.Name, "C:\\File" + i + ".mp4"));
            }
            return episodes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Services;
using ShowTime.Model;

namespace ShowTime.DataLoading
{
    public class DirectoryParsingDataLoader : IDataLoader
    {
        private readonly string directory;
        private readonly ITVShowDiscoveryService showDiscoveryService;

        public DirectoryParsingDataLoader(string directory, ITVShowDiscoveryService showDiscoveryService)
        {
            this.directory = directory;
            this.showDiscoveryService = showDiscoveryService;
        }

        public void LoadData(DataManager dataManager)
        {
            List<string> recursiveFolders = DirSearch(directory);

            var loadedEpisodes = new List<Episode>();
            foreach (string folder in recursiveFolders)
            {
                loadedEpisodes.AddRange(GetEpisodes(folder));
            }

            loadedEpisodes = loadedEpisodes.Distinct().ToList();

            var tvShows = loadedEpisodes.Select(e => new TVShow(e.TVShowId.Name, "")).Distinct();
            var seasons = loadedEpisodes.Select(e => new Season(e.SeasonId.ShowId, e.SeasonId.SeasonNumber)).Distinct();



            foreach (var show in tvShows)
                dataManager.TVShowRepository.Insert(show);
            foreach (var season in seasons)
                dataManager.SeasonRepository.Insert(season);
            foreach (var episode in loadedEpisodes)
                dataManager.EpisodeRepository.Insert(episode);
        }

        private List<string> DirSearch(string sDir)
        {
            List<string> directories = new List<string>();
            try
            {
                foreach (string d in System.IO.Directory.GetDirectories(sDir))
                {
                    directories.AddRange(DirSearch(d));
                }
                directories.Add(sDir);
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return directories;
        }

        private IEnumerable<Episode> GetEpisodes(string folder)
        {
            var directory = new System.IO.DirectoryInfo(folder);

            List<Episode> episodes = new List<Episode>();

            foreach (var file in directory.GetFiles("*.*")
                                          .Where(file => file.Name.EndsWith("avi") || file.Name.EndsWith("mp4")))
            {
                var episodeFileWrapper = new SystemIOEpisodeFileSystemEntry(file.FullName);
                var episodeResults = showDiscoveryService.BestGuessEpisode(episodeFileWrapper);
                if (episodeResults.AllDetailsFound)
                {
                    episodes.Add(episodeResults.Episode);
                }
            }

            return episodes.OrderBy(e => e.Number);
        }
    }
}

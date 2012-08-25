﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Services;
using ShowTime.Model;
using ShowTime.Services.EpisodeDetailsBuilders;

namespace ShowTime.Services
{
    public interface IDirectoryScanner
    {
        void LoadData(IDataStore dataManager);
        
        IEnumerable<TVShow> AllFoundTVShows { get; }
        IEnumerable<Season> AllFoundSeasons { get; }
        IEnumerable<Episode> AllFoundEpisodes { get; }
    }

    public class DirectoryScanner : IDirectoryScanner
    {
        private readonly string directory;
        private readonly IEpisodeDetailsBuilder episodeBuilder;

        public IEnumerable<TVShow> AllFoundTVShows { get; private set; }
        public IEnumerable<Season> AllFoundSeasons { get; private set; }
        public IEnumerable<Episode> AllFoundEpisodes { get; private set; }

        public DirectoryScanner(string directory, IEpisodeDetailsBuilder episodeBuilder)
        {
            this.directory = directory;
            this.episodeBuilder = episodeBuilder;
        }

        public void LoadData(IDataStore dataManager)
        {
            List<string> recursiveFolders = DirSearch(directory);

            var loadedEpisodes = recursiveFolders.SelectMany(folder => GetEpisodes(folder));
            loadedEpisodes = loadedEpisodes.Distinct();

            AllFoundTVShows = loadedEpisodes.Select(e => new TVShow(e.TVShowId.Name, "")).Distinct();
            AllFoundSeasons = loadedEpisodes.Select(e => new Season(e.SeasonId.ShowId, e.SeasonId.SeasonNumber)).Distinct();
            AllFoundEpisodes = loadedEpisodes;
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
                var episodeResults = episodeBuilder.BestGuessEpisode(episodeFileWrapper);
                if (episodeResults.AllDetailsFound)
                {
                    episodes.Add(episodeResults.Episode);
                }
            }

            return episodes.OrderBy(e => e.Number);
        }
    }
}

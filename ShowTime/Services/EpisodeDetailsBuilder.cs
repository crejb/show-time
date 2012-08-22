using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using ShowTime.Model;
using ShowTime.Services.Guessers;

namespace ShowTime.Services
{
    public interface IEpisodeDetailsBuilder
    {
        EpisodeGuessResults BestGuessEpisode(IEpisodeFileSystemEntry episodeFileSystemEntry);
    }

    public class EpisodeGuessResults
    {
        public bool ShowFound { get; private set; }
        public bool SeasonFound { get; private set; }
        public bool EpisodeFound { get; private set; }
        public bool AllDetailsFound { get { return ShowFound && SeasonFound && EpisodeFound; } }
        public Episode Episode { get; private set; }

        public EpisodeGuessResults(Episode episode, bool showFound, bool seasonFound, bool episodeFound)
        {
            Episode = episode;
            ShowFound = showFound;
            SeasonFound = seasonFound;
            EpisodeFound = episodeFound;
        }
    }

    public class EpisodeDetailsBuilder : IEpisodeDetailsBuilder
    {
        private IShowAttributeGuesser ShowGuesser { get; set; }
        private ISeasonAttributeGuesser SeasonGuesser { get; set; }
        private IEpisodeAttributeGuesser EpisodeGuesser { get; set; }

        public EpisodeDetailsBuilder(IShowAttributeGuesser showGuesser, ISeasonAttributeGuesser seasonGuesser, IEpisodeAttributeGuesser episodeGuesser)
        {
            ShowGuesser = showGuesser;
            SeasonGuesser = seasonGuesser;
            EpisodeGuesser = episodeGuesser;
        }

        public EpisodeGuessResults BestGuessEpisode(IEpisodeFileSystemEntry episodeFileSystemEntry)
        {
            var showNameResults = ShowGuesser.GuessShowName(episodeFileSystemEntry);
            var seasonNumberResults = SeasonGuesser.GuessSeasonNumber(episodeFileSystemEntry);
            var episodeNumberResults = EpisodeGuesser.GuessEpisodeNumber(episodeFileSystemEntry);

            var showId = TVShowId.CreateId(showNameResults.ShowName);
            var seasonId = SeasonId.CreateId(showId, seasonNumberResults.Season);

            Episode episode = new Episode(
                showId,
                seasonId,
                episodeNumberResults.Episode,
                string.Empty,
                string.Empty,
                episodeFileSystemEntry.FullFilename
            );

            return new EpisodeGuessResults(episode, showNameResults.Successful, seasonNumberResults.Successful, episodeNumberResults.Successful);
        }
    }

    public interface IEpisodeFileSystemEntry
    {
        string FullFilename{get;}
        string ShortFilename{get;}
        string NameWithoutExtension { get; }
        string ParentDirectoryName{get;}
        string GrandParentDirectoryName{get;}

    }

    public class SystemIOEpisodeFileSystemEntry : IEpisodeFileSystemEntry
    {
        private readonly System.IO.FileInfo file;

        public string FullFilename
        {
            get { return file.FullName; }
        }

        public string ShortFilename
        {
            get { return file.Name; }
        }

        public string NameWithoutExtension
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(FullFilename); }
        }

        public string ParentDirectoryName
        {
            get { return file.Directory.Name; }
        }

        public string GrandParentDirectoryName
        {
            get { return file.Directory.Parent.Name; }
        }

        public SystemIOEpisodeFileSystemEntry(string fullFilename)
        {
            file = new System.IO.FileInfo(fullFilename);
        }
    }
}

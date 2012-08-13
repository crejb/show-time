using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ShowTime.Services.Guessers
{
    public interface ISeasonAttributeGuesser
    {
        SeasonGuesserResults GuessSeasonNumber(IEpisodeFileSystemEntry episode);
    }

    public class SeasonGuesserResults
    {
        private const int UNKNOWN_SEASON_NUMBER = -1;
        public static SeasonGuesserResults UNKNOWN_SEASON_RESULTS = new SeasonGuesserResults(UNKNOWN_SEASON_NUMBER);

        public readonly int Season;
        public bool Successful
        {
            get { return Season != UNKNOWN_SEASON_NUMBER; }
        }

        public SeasonGuesserResults(int guessedSeason)
        {
            this.Season = guessedSeason;
        }
    }

    public class SeasonGuesser : ISeasonAttributeGuesser
    {
        public SeasonGuesserResults GuessSeasonNumber(IEpisodeFileSystemEntry episode)
        {
            // Check if the name of the parent folder is 'season x'
            var result = SearchForSeasonNumberInDirectoryName(episode.ParentDirectoryName);
            if (result.Success)
                return new SeasonGuesserResults(result.Result);

            // Check if the filename has the season in it
            // Matches *E10*, *e10*
            result = SearchForSeasonNumberInFileName(episode.NameWithoutExtension);
            if (result.Success)
                return new SeasonGuesserResults(result.Result);

            return SeasonGuesserResults.UNKNOWN_SEASON_RESULTS;
        }

        private SearchResult SearchForSeasonNumberInDirectoryName(string directoryName)
        {
            // Will match:
            // s9, S9, s 9, S 9
            // season9, Season9, season 9, Season 9
            Regex episodeRegex = new Regex("[Ss](eason)?\\s?([0-9]+)");
            Match match = episodeRegex.Match(directoryName);

            int guessedSeasonNumber = -1;
            bool success = false;
            if (match.Success)
            {
                Debug.Assert(match.Groups.Count == 2 || match.Groups.Count == 3);
                string guessedSeasonString = match.Groups[match.Groups.Count - 1].Value;
                success = int.TryParse(guessedSeasonString, out guessedSeasonNumber);
            }
            return new SearchResult(success, guessedSeasonNumber);
        }

        private SearchResult SearchForSeasonNumberInFileName(string shortFileName)
        {
            // Check if the filename has the season in it
            // Matches *S10*, *s10*
            Regex episodeRegex = new Regex("[Ss]([0-9]+)");
            Match match = episodeRegex.Match(shortFileName);

            int guessedSeasonNumber = -1;
            bool success = false;
            if (match.Success)
            {
                Debug.Assert(match.Groups.Count == 2);
                string guessedSeasonString = match.Groups[1].Value;
                success = int.TryParse(guessedSeasonString, out guessedSeasonNumber);
            }
            return new SearchResult(success, guessedSeasonNumber);
        }

        private class SearchResult
        {
            public readonly bool Success;
            public readonly int Result;
            public SearchResult(bool success, int result)
            {
                Success = success;
                Result = result;
            }
        }
    }
}

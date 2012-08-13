using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ShowTime.Services.Guessers
{
    public interface IEpisodeAttributeGuesser
    {
        EpisodeGuesserResults GuessEpisodeNumber(IEpisodeFileSystemEntry episode);
    }

    public class EpisodeGuesserResults
    {
        private const int UNKNOWN_EPISODE_NUMBER = -1;
        public static EpisodeGuesserResults UNKNOWN_EPISODE_RESULTS = new EpisodeGuesserResults(UNKNOWN_EPISODE_NUMBER);

        public readonly int Episode;
        public bool Successful
        {
            get { return Episode != UNKNOWN_EPISODE_NUMBER; }
        }

        public EpisodeGuesserResults(int guessedEpisode)
        {
            this.Episode = guessedEpisode;
        }
    }

    public class EpisodeGuesser : IEpisodeAttributeGuesser
    {
        public EpisodeGuesserResults GuessEpisodeNumber(IEpisodeFileSystemEntry episode)
        {
            Regex episodeRegex = new Regex("[Ee]([0-9]+)");
            Match match = episodeRegex.Match(episode.NameWithoutExtension);

            if (match.Success)
            {
                Debug.Assert(match.Groups.Count == 2);
                string guessedEpisodeString = match.Groups[1].Value;
                int guessedEpisodeInt;
                if (int.TryParse(guessedEpisodeString, out guessedEpisodeInt))
                    return new EpisodeGuesserResults(guessedEpisodeInt);
            }

            return EpisodeGuesserResults.UNKNOWN_EPISODE_RESULTS; ;
        }
    }
}

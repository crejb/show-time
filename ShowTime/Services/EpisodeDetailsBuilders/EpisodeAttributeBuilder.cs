using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ShowTime.Services.EpisodeDetailsBuilders
{
    public interface IEpisodeAttributeBuilder
    {
        EpisodeAttributeBuilderResults GuessEpisodeNumber(IEpisodeFileSystemEntry episode);
    }

    public class EpisodeAttributeBuilderResults
    {
        private const int UNKNOWN_EPISODE_NUMBER = -1;
        public static EpisodeAttributeBuilderResults UNKNOWN_EPISODE_RESULTS = new EpisodeAttributeBuilderResults(UNKNOWN_EPISODE_NUMBER);

        public readonly int Episode;
        public bool Successful
        {
            get { return Episode != UNKNOWN_EPISODE_NUMBER; }
        }

        public EpisodeAttributeBuilderResults(int guessedEpisode)
        {
            this.Episode = guessedEpisode;
        }
    }

    public class EpisodeAttributeBuilder : IEpisodeAttributeBuilder
    {
        public EpisodeAttributeBuilderResults GuessEpisodeNumber(IEpisodeFileSystemEntry episode)
        {
            Regex episodeRegex = new Regex("[Ee]([0-9]+)");
            Match match = episodeRegex.Match(episode.NameWithoutExtension);

            if (match.Success)
            {
                Debug.Assert(match.Groups.Count == 2);
                string guessedEpisodeString = match.Groups[1].Value;
                int guessedEpisodeInt;
                if (int.TryParse(guessedEpisodeString, out guessedEpisodeInt))
                    return new EpisodeAttributeBuilderResults(guessedEpisodeInt);
            }

            return EpisodeAttributeBuilderResults.UNKNOWN_EPISODE_RESULTS; ;
        }
    }
}

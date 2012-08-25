using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using ShowTime.Model;

namespace ShowTime.Services.EpisodeDetailsBuilders
{
    public interface IEpisodeDetailsBuilder
    {
        EpisodeBuilderResults BestGuessEpisode(IEpisodeFileSystemEntry episodeFileSystemEntry);
    }

    public class EpisodeDetailsBuilder : IEpisodeDetailsBuilder
    {
        private IShowAttributeBuilder ShowGuesser { get; set; }
        private ISeasonAttributeBuilder SeasonGuesser { get; set; }
        private IEpisodeAttributeBuilder EpisodeGuesser { get; set; }

        public EpisodeDetailsBuilder(IShowAttributeBuilder showGuesser, ISeasonAttributeBuilder seasonGuesser, IEpisodeAttributeBuilder episodeGuesser)
        {
            ShowGuesser = showGuesser;
            SeasonGuesser = seasonGuesser;
            EpisodeGuesser = episodeGuesser;
        }

        public EpisodeBuilderResults BestGuessEpisode(IEpisodeFileSystemEntry episodeFileSystemEntry)
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
                System.IO.Path.GetFileNameWithoutExtension(episodeFileSystemEntry.FullFilename),
                string.Empty,
                episodeFileSystemEntry.FullFilename
            );

            return new EpisodeBuilderResults(episode, showNameResults.Successful, seasonNumberResults.Successful, episodeNumberResults.Successful);
        }
    }
}

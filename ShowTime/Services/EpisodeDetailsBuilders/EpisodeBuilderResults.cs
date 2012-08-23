using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Services.EpisodeDetailsBuilders
{
    public class EpisodeBuilderResults
    {
        public bool ShowFound { get; private set; }
        public bool SeasonFound { get; private set; }
        public bool EpisodeFound { get; private set; }
        public bool AllDetailsFound { get { return ShowFound && SeasonFound && EpisodeFound; } }
        public Episode Episode { get; private set; }

        public EpisodeBuilderResults(Episode episode, bool showFound, bool seasonFound, bool episodeFound)
        {
            Episode = episode;
            ShowFound = showFound;
            SeasonFound = seasonFound;
            EpisodeFound = episodeFound;
        }
    }
}

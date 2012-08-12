using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Model
{
    public class LastWatched
    {
        public EpisodeId EpisodeId { get; private set; }
        public DateTime Time { get; private set; }

        public LastWatched(EpisodeId episodeId, DateTime time)
        {
            this.EpisodeId = episodeId;
            this.Time = time;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Model
{
    public class LastWatchedEntry  : Entity<LastWatchedEntryId>
    {
        public LastWatchedEntryId Id{get; private set;}

        public EpisodeId EpisodeId { get{return Id.EpisodeId; }}
        public DateTime Time { get{return Id.Time;}}

        public LastWatchedEntry(EpisodeId episodeId, DateTime time)
        {
            Id = new LastWatchedEntryId(episodeId, time);
        }
    }

    public class LastWatchedEntryId
    {
        public EpisodeId EpisodeId { get; private set; }
        public DateTime Time { get; private set; }

        public LastWatchedEntryId(EpisodeId episodeId, DateTime time)
        {
            EpisodeId = episodeId;
            Time = time;
        }

        public override bool Equals(object obj)
        {
            LastWatchedEntryId other = (LastWatchedEntryId)obj;
            if (other == null)
            {
                return false;
            }

            return this.EpisodeId.Equals(other.EpisodeId)
                && this.Time.Equals(other.Time);
        }

        public override int GetHashCode()
        {
            return EpisodeId.GetHashCode() ^ Time.GetHashCode();
        }
    }
}

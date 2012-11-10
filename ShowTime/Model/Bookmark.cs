using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Model
{
    public class Bookmark : Entity<BookmarkId>
    {
        public BookmarkId Id{ get; private set; }

        public EpisodeId EpisodeId { get { return Id.EpisodeId; } }
        public TimeSpan Time { get; private set; }

        public Bookmark(EpisodeId episodeId, TimeSpan time)
        {
            this.Id = new BookmarkId(episodeId);
            this.Time = time;
        }
    }

    public class BookmarkId
    {
        public EpisodeId EpisodeId { get; private set; }

        public BookmarkId(EpisodeId episodeId)
        {
            EpisodeId = episodeId;
        }

        public override bool Equals(object obj)
        {
            BookmarkId other = (BookmarkId)obj;
            if (other == null)
            {
                return false;
            }

            return this.EpisodeId.Equals(other.EpisodeId);
        }

        public override int GetHashCode()
        {
            return EpisodeId.GetHashCode();
        }
    }
}

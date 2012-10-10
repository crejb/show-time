using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Model
{
    public class Bookmark : Entity<BookmarkId>
    {
        public BookmarkId Id{ get; private set; }

        public EpisodeId EpisodeId { get; private set; }
        public TimeSpan Time { get; private set; }

        public Bookmark(EpisodeId episodeId, TimeSpan time)
        {
            this.Id = new BookmarkId();
            this.EpisodeId = episodeId;
            this.Time = time;
        }
    }

    public class BookmarkId
    {

    }
}

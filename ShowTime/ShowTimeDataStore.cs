using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using ShowTime.Repositories;
using System.IO;
using System.Xml;

namespace ShowTime
{
    public class ShowTimeDataStore : ShowTime.IDataStore
    {
        public IRepository<TVShowId, TVShow> TVShowRepository { get; private set; }
        public IRepository<SeasonId, Season> SeasonRepository { get; private set; }
        public IRepository<EpisodeId, Episode> EpisodeRepository { get; private set; }
        public IRepository<BookmarkId, Bookmark> BookmarkRepository { get; private set; }
        public IRepository<LastWatchedEntryId, LastWatchedEntry> LastWatchedRepository { get; private set; }

        public ShowTimeDataStore(IRepository<TVShowId, TVShow> tvShowRepository,
                           IRepository<SeasonId, Season> seasonRepository,
                           IRepository<EpisodeId, Episode> episodeRepository,
                           IRepository<BookmarkId, Bookmark> bookmarkRepository,
                           IRepository<LastWatchedEntryId, LastWatchedEntry> lastWatchedRepository)
        {
            this.TVShowRepository = tvShowRepository;
            this.SeasonRepository = seasonRepository;
            this.EpisodeRepository = episodeRepository;
            this.BookmarkRepository = bookmarkRepository;
            this.LastWatchedRepository = lastWatchedRepository;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Xml;

namespace ShowTime.Repositories
{
    public class BookmarkRepository : BaseDictionaryRepository<BookmarkId, Bookmark>
    {
        public BookmarkRepository(IRepositoryPersister<BookmarkId, Bookmark> persister)
            : base(persister)
        {
        }
    }

    public class BookmarkRepositoryPersister : XMLRepositoryPersister<BookmarkId, Bookmark>
    {
        public BookmarkRepositoryPersister(string filename)
        {
            this.filename = filename;
        }

        protected override string EntityName
        {
            get { return "Bookmark"; }
        }

        protected override void SaveEntity(Bookmark entity, XmlWriter writer)
        {
            XmlPersistenceUtilities.SaveEpisodeId(entity.Id.EpisodeId, writer);
            writer.WriteElementString("Time", entity.Time.ToString());
        }

        protected override Bookmark LoadEntity(XmlElement element)
        {
            var episodeId = XmlPersistenceUtilities.LoadEpisodeId(element);
            var time = TimeSpan.Parse(element.SelectSingleNode("Time").InnerText);
            return new Bookmark(episodeId, time);
        }
    }
}

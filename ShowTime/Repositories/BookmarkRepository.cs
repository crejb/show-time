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
            throw new Exception();
            //writer.WriteElementString("Name", entity.Name);
            //writer.WriteElementString("Description", entity.Description);
        }

        protected override Bookmark LoadEntity(XmlElement element)
        {
            throw new Exception();
            //var name = element.SelectSingleNode("Name").InnerText;
            //var description = element.SelectSingleNode("Description").InnerText;
            //return new TVShow(name, description);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Xml;

namespace ShowTime.Repositories
{
    public class LastWatchedRepository : BaseDictionaryRepository<LastWatchedEntryId, LastWatchedEntry>
    {
        public LastWatchedRepository(IRepositoryPersister<LastWatchedEntryId, LastWatchedEntry> persister)
            : base(persister)
        {
        }
    }

    public class LastWatchedRepositoryPersister : XMLRepositoryPersister<LastWatchedEntryId, LastWatchedEntry>
    {
        public LastWatchedRepositoryPersister(string filename)
        {
            this.filename = filename;
        }

        protected override string EntityName
        {
            get { return "LastWatchedEntry"; }
        }

        protected override void SaveEntity(LastWatchedEntry entity, XmlWriter writer)
        {
            throw new Exception();
            //writer.WriteElementString("Name", entity.Name);
            //writer.WriteElementString("Description", entity.Description);
        }

        protected override LastWatchedEntry LoadEntity(XmlElement element)
        {
            throw new Exception();
            //var name = element.SelectSingleNode("Name").InnerText;
            //var description = element.SelectSingleNode("Description").InnerText;
            //return new TVShow(name, description);
        }
    }
}

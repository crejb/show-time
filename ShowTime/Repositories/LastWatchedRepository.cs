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
            XmlPersistenceUtilities.SaveEpisodeId(entity.Id.EpisodeId, writer);
            writer.WriteElementString("Time", entity.Time.ToString());
        }

        protected override LastWatchedEntry LoadEntity(XmlElement element)
        {
            var episodeId = XmlPersistenceUtilities.LoadEpisodeId(element);
            var time = DateTime.Parse(element.SelectSingleNode("Time").InnerText);
            return new LastWatchedEntry(episodeId, time);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Xml;

namespace ShowTime.Repositories
{
    public class EpisodeRepository : BaseDictionaryRepository<EpisodeId, Episode>
    {
        public EpisodeRepository(IRepositoryPersister<EpisodeId, Episode> persister)
            : base(persister)
        {
        }
    }

    public class EpisodeRepositoryPersister : XMLRepositoryPersister<EpisodeId, Episode>
    {
        public EpisodeRepositoryPersister(string filename)
        {
            this.filename = filename;
        }

        protected override string EntityName
        {
            get { return "Episode"; }
        }

        protected override void SaveEntity(Episode entity, XmlWriter writer)
        {
            XmlPersistenceUtilities.SaveSeasonId(entity.SeasonId, writer);
            writer.WriteElementString("Number", entity.Number.ToString());
            writer.WriteElementString("Title", entity.Title);
            writer.WriteElementString("Description", entity.Description);
            writer.WriteElementString("Filename", entity.Filename);
        }

        protected override Episode LoadEntity(XmlElement element)
        {
            var seasonId = XmlPersistenceUtilities.LoadSeasonId(element);
            var tvShowId = seasonId.ShowId;
            var number = int.Parse(element.SelectSingleNode("Number").InnerText);
            var title = element.SelectSingleNode("Title").InnerText;
            var description = element.SelectSingleNode("Description").InnerText;
            var episodeFilename = element.SelectSingleNode("Filename").InnerText;

            return new Episode(tvShowId, seasonId, number, title, description, episodeFilename);
        }
    }
}

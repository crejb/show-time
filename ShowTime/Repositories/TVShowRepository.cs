using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Xml;

namespace ShowTime.Repositories
{
    public class TVShowRepository : BaseDictionaryRepository<TVShowId, TVShow>
    {
        public TVShowRepository(IRepositoryPersister<TVShowId, TVShow> persister)
            : base(persister)
        {
        }
    }

    public class TVShowRepositoryPersister : XMLRepositoryPersister<TVShowId, TVShow>
    {
        public TVShowRepositoryPersister(string filename)
        {
            this.filename = filename;
        }

        protected override string EntityName
        {
            get { return "TVShow"; }
        }

        protected override void SaveEntity(TVShow entity, XmlWriter writer)
        {
            writer.WriteElementString("Name", entity.Name);
            writer.WriteElementString("Description", entity.Description);
        }

        protected override TVShow LoadEntity(XmlElement element)
        {
            var name = element.SelectSingleNode("Name").InnerText;
            var description = element.SelectSingleNode("Description").InnerText;
            return new TVShow(name, description);
        }
    }
}

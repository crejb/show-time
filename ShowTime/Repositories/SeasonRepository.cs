using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Xml;

namespace ShowTime.Repositories
{
    public class SeasonRepository : BaseDictionaryRepository<SeasonId, Season>
    {
        public SeasonRepository(IRepositoryPersister<SeasonId, Season> persister)
            : base(persister)
        {
        }
    }

    public class SeasonRepositoryPersister : XMLRepositoryPersister<SeasonId, Season>
    {
        public SeasonRepositoryPersister(string filename)
        {
            this.filename = filename;
        }

        protected override string EntityName
        {
            get { return "Season"; }
        }

        protected override void SaveEntity(Season entity, XmlWriter writer)
        {
            XmlPersistenceUtilities.SaveTVShowId(entity.TVShowId, writer);
            writer.WriteElementString("Number", entity.Number.ToString());
        }

        protected override Season LoadEntity(XmlElement element)
        {
            var tvShowId = XmlPersistenceUtilities.LoadTVShowId(element);
            var number = int.Parse(element.SelectSingleNode("Number").InnerText);
            return new Season(tvShowId, number);
        }
    }
}

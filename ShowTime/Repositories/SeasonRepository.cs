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

    public static class XmlPersistenceUtilities
    {
        public const string ELEMENT_STRING_TV_SHOW_ID = "TVShowId";
        public const string ELEMENT_STRING_TV_SHOW_ID_NAME = "Name";
        public static void SaveTVShowId(TVShowId id, XmlWriter writer)
        {
            writer.WriteStartElement(ELEMENT_STRING_TV_SHOW_ID);
            writer.WriteElementString(ELEMENT_STRING_TV_SHOW_ID_NAME, id.Name);
            writer.WriteEndElement();
        }

        public static TVShowId LoadTVShowId(XmlNode element)
        {
            var node = element.SelectSingleNode(ELEMENT_STRING_TV_SHOW_ID);
            var name = node.SelectSingleNode(ELEMENT_STRING_TV_SHOW_ID_NAME).InnerText;
            return TVShowId.CreateId(name);
        }

        public const string ELEMENT_STRING_SEASON_ID = "SeasonId";
        public const string ELEMENT_STRING_SEASON_ID_NUMBER = "Number";
        public static void SaveSeasonId(SeasonId id, XmlWriter writer)
        {
            writer.WriteStartElement(ELEMENT_STRING_SEASON_ID);
            SaveTVShowId(id.ShowId, writer);
            writer.WriteElementString(ELEMENT_STRING_SEASON_ID_NUMBER, id.SeasonNumber.ToString());
            writer.WriteEndElement();
        }

        public static SeasonId LoadSeasonId(XmlNode element)
        {
            var node = element.SelectSingleNode(ELEMENT_STRING_SEASON_ID);
            var tvShowId = LoadTVShowId(node);
            var number = int.Parse(node.SelectSingleNode(ELEMENT_STRING_SEASON_ID_NUMBER).InnerText);
            return SeasonId.CreateId(tvShowId, number);
        }
    }
}

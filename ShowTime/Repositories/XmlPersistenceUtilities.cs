using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Xml;

namespace ShowTime.Repositories
{
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

        public const string ELEMENT_STRING_EPISODE_ID = "EpisodeId";
        public const string ELEMENT_STRING_EPISODE_ID_NUMBER = "Number";
        public static void SaveEpisodeId(EpisodeId id, XmlWriter writer)
        {
            writer.WriteStartElement(ELEMENT_STRING_EPISODE_ID);
            SaveTVShowId(id.ShowId, writer);
            SaveSeasonId(id.SeasonId, writer);
            writer.WriteElementString(ELEMENT_STRING_EPISODE_ID_NUMBER, id.EpisodeNumber.ToString());
            writer.WriteEndElement();
        }

        public static EpisodeId LoadEpisodeId(XmlNode element)
        {
            var node = element.SelectSingleNode(ELEMENT_STRING_EPISODE_ID);
            var tvShowId = LoadTVShowId(node);
            var seasonId = LoadSeasonId(node);
            var number = int.Parse(node.SelectSingleNode(ELEMENT_STRING_EPISODE_ID_NUMBER).InnerText);
            return EpisodeId.CreateId(tvShowId, seasonId, number);
        }
    }
}

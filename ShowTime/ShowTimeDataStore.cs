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
        public IRepository<TVShow, TVShowId> TVShowRepository { get; private set; }
        public IRepository<Season, SeasonId> SeasonRepository { get; private set; }
        public IRepository<Episode, EpisodeId> EpisodeRepository { get; private set; }

        public ShowTimeDataStore(IRepository<TVShow, TVShowId> tvShowRepository,
                           IRepository<Season, SeasonId> seasonRepository,
                           IRepository<Episode, EpisodeId> episodeRepository)
        {
            this.TVShowRepository = tvShowRepository;
            this.SeasonRepository = seasonRepository;
            this.EpisodeRepository = episodeRepository;
        }

        public void Save()
        {
            Action<TVShow, XmlWriter> saveItemCallback = (show, writer) =>
            {
                writer.WriteElementString("Name", show.Name);
                writer.WriteElementString("Description", show.Description);
            };

            SaveRepo<TVShow, TVShowId>(TVShowRepository, saveItemCallback, "TVShow", "C:\\ShowTimeData\\ShowData.xml");
        }

        public void Load()
        {
            Func<XmlElement, TVShow> loadItemCallback = (element) =>
            {
                var id = long.Parse(element.SelectSingleNode("ID").InnerText);
                var name = element.SelectSingleNode("Name").InnerText;
                var description = element.SelectSingleNode("Description").InnerText;
                return new TVShow(name, description);
            };

            LoadRepo<TVShow, TVShowId>(TVShowRepository, loadItemCallback, "TVShow", "C:\\ShowTimeData\\ShowData.xml");
        }

        private void SaveRepo<T, TKey>(IRepository<T, TKey> repo, Action<T, XmlWriter> saveItemCallback, string itemType, string filename) where T : Entity<TKey>
        {
            var allItems = repo.GetAll();

            string repoTitle = string.Format("{0}Repository", itemType);

            XmlWriter xmlWriter = new System.Xml.XmlTextWriter(new StreamWriter(filename));
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement(repoTitle);

            foreach (var item in allItems)
            {
                xmlWriter.WriteStartElement(itemType);
                saveItemCallback(item, xmlWriter);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();
        }

        private void LoadRepo<T, TKey>(IRepository<T, TKey> repo, Func<XmlElement, T> loadItemCallback, string itemType, string filename) where T : Entity<TKey>
        {
            var document = new XmlDocument();
            document.Load(filename);

            var itemElements = document.GetElementsByTagName(itemType);
            foreach (var itemElement in itemElements)
            {
                var item = loadItemCallback(itemElement as XmlElement);
                repo.Insert(item);
            }
        }
    }
}


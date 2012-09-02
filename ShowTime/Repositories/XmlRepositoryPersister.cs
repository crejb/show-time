using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Xml;
using System.IO;

namespace ShowTime.Repositories
{
    public abstract class XMLRepositoryPersister<TKey, TValue> : IRepositoryPersister<TKey, TValue> where TValue : Entity<TKey>
    {
        protected string filename;

        protected abstract string EntityName { get; }

        protected abstract void SaveEntity(TValue entity, XmlWriter writer);
        protected abstract TValue LoadEntity(XmlElement element);

        public void Load(IRepository<TKey, TValue> repository)
        {
            if (!File.Exists(filename))
            {
                return;
            }

            var document = new XmlDocument();
            document.Load(filename);

            var itemElements = document.GetElementsByTagName(EntityName);
            foreach (var itemElement in itemElements)
            {
                var item = LoadEntity(itemElement as XmlElement);
                repository.Insert(item);
            }
        }

        public void Save(IRepository<TKey, TValue> repository)
        {
            (new FileInfo(filename)).Directory.Create();

            var allItems = repository.GetAll();

            string repoTitle = string.Format("{0}Repository", EntityName);
            
            XmlWriter xmlWriter = new System.Xml.XmlTextWriter(new StreamWriter(filename));
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement(repoTitle);

            foreach (var item in allItems)
            {
                xmlWriter.WriteStartElement(EntityName);
                SaveEntity(item, xmlWriter);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.ViewModel
{
    public class EpisodeViewModel : ViewModelBase
    {
        private DataManager dataManager;
        private Episode episode;

        public string ShowName
        {
            get { return dataManager.TVShowRepository.Find(episode.TVShowId).Name; }
        }

        public int SeasonNumber
        {
            get { return dataManager.SeasonRepository.Find(episode.SeasonId).Number; }
        }

        public int EpisodeNumber
        {
            get { return episode.Number; }
        }

        public string Title
        {
            get { return episode.Title; }
        }

        public string Description
        {
            get { return episode.Description; }
        }

        public EpisodeViewModel(DataManager dataManager, Episode episode)
        {
            this.dataManager = dataManager;
            this.episode = episode;
        }

        private void GenerateVideoThumbnailImage()
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.DataLoading
{
    public class HardcodedDataLoader : IDataLoader
    {
        public void LoadData(DataManager dataManager)
        {
            foreach (string showName in new string[] { "The Big Bang Theory", "Masterchef" })
            {
                var show = CreateTvShow(showName);
                var seasons = CreateSeasons(show);
                foreach (var season in seasons)
                {
                    var episodes = CreateEpisodes(show, season);
                    foreach (var episode in episodes)
                    {
                        dataManager.EpisodeRepository.Insert(episode);
                    }
                    dataManager.SeasonRepository.Insert(season);
                }
                dataManager.TVShowRepository.Insert(show);
            }
        }

        private TVShow CreateTvShow(string title)
        {
            return new TVShow(
                title,
                "description for " + title);
        }

        private List<Season> CreateSeasons(TVShow show)
        {
            var seasons = new List<Season>();
            for (int i = 1; i < 3; i++)
            {
                seasons.Add(new Season(show.Id, i));
            }
            return seasons;
        }

        private List<Episode> CreateEpisodes(TVShow show, Season season)
        {
            var episodes = new List<Episode>();
            for (int i = 1; i < 6; i++)
            {
                episodes.Add(new Episode(show.Id, season.Id, i, "Episode " + i + " of " + show.Name, "Funny" + i + " of " + show.Name, "C:\\File" + i + ".mp4"));
            }
            return episodes;
        }
    }
}

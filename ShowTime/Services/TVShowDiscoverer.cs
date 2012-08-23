using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Services
{
    public interface ITVShowDiscoverer
    {
        IEnumerable<TVShow> AllTVShows { get; }
        IEnumerable<Season> AllSeasons { get; }
        IEnumerable<Episode> AllEpisodes { get; }

        IEnumerable<TVShow> NewTVShows { get; }
        IEnumerable<Season> NewSeasons { get; }
        IEnumerable<Episode> NewEpisodes { get; }
    }

    public class TVShowDiscoverer : ITVShowDiscoverer
    {
        private readonly IDirectoryScanner scanner;

        public IEnumerable<TVShow> AllTVShows { get { return scanner.AllFoundTVShows; } }
        public IEnumerable<Season> AllSeasons { get { return scanner.AllFoundSeasons; } }
        public IEnumerable<Episode> AllEpisodes { get { return scanner.AllFoundEpisodes; } }

        public IEnumerable<TVShow> NewTVShows { get; private set; }
        public IEnumerable<Season> NewSeasons { get; private set; }
        public IEnumerable<Episode> NewEpisodes { get; private set; }

        public TVShowDiscoverer(IDataStore dataStore, IDirectoryScanner scanner)
        {
            this.scanner = scanner;

            NewTVShows = scanner.AllFoundTVShows.Where(
                tvShow => dataStore.TVShowRepository.Find(tvShow.Id) == null);

            NewSeasons = scanner.AllFoundSeasons.Where(
                season => dataStore.SeasonRepository.Find(season.Id) == null);

            NewEpisodes = scanner.AllFoundEpisodes.Where(
                episode => dataStore.EpisodeRepository.Find(episode.Id) == null);
        }
    }
}

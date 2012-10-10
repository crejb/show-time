using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using ShowTime.Services.Providers;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;
using System.Collections.ObjectModel;
using ShowTime.Services;

namespace ShowTime.ViewModel
{
    public class UpdateShowTimeCollectionViewModel : ViewModelBase
    {
        private IDataStore dataStore;
        private readonly ITVShowDiscovererProvider discovererProvider;
        private IEpisodeThumbnailGenerator thumbnailGenerator;

        private ObservableCollection<DiscoveredEpisodeViewModel> discoveredEpisodes;
        public ObservableCollection<DiscoveredEpisodeViewModel> DiscoveredEpisodes
        {
            get { return discoveredEpisodes; }
        }

        private string searchDirectory;
        public string SearchDirectory
        {
            get { return searchDirectory; }
            set
            {
                if (value == searchDirectory)
                    return;

                searchDirectory = value;

                base.OnPropertyChanged("SearchDirectory");
            }
        }

        RelayCommand searchDirectoryCommand;
        public ICommand SearchDirectoryCommand
        {
            get { return searchDirectoryCommand; }
        }

        RelayCommand applyCommand;
        public ICommand ApplyCommand
        {
            get { return applyCommand; }
        }

        public UpdateShowTimeCollectionViewModel(IDataStore dataStore, ITVShowDiscovererProvider discovererProvider, IEpisodeThumbnailGenerator thumbnailGenerator)
        {
            this.dataStore = dataStore;
            this.discovererProvider = discovererProvider;
            this.thumbnailGenerator = thumbnailGenerator;

            searchDirectoryCommand = new RelayCommand(
                        param => this.SearchDirectoryForNewEpisodes(),
                        param => this.CanSearchDirectory
                        );

            applyCommand = new RelayCommand(
                        param => this.InsertNewEpisodesIntoRepository(),
                        param => this.CanApply
                        );
        }

        public bool CanSearchDirectory
        {
            get
            {
                return !String.IsNullOrEmpty(searchDirectory) && System.IO.Directory.Exists(searchDirectory);
            }
        }

        public void SearchDirectoryForNewEpisodes()
        {
            if (!CanSearchDirectory)
                throw new InvalidOperationException();

            var discoverer = discovererProvider.GetTVShowDiscoverer(searchDirectory);
            discoveredEpisodes = new ObservableCollection<DiscoveredEpisodeViewModel>(
                discoverer.NewEpisodes.Select(episode => new DiscoveredEpisodeViewModel(dataStore, episode))
            );

            base.OnPropertyChanged("DiscoveredEpisodes");
        }

        public bool CanApply
        {
            get
            {
                return discoveredEpisodes != null && discoveredEpisodes.Any(e => e.IsSelected);
            }
        }

        private void InsertNewEpisodesIntoRepository()
        {
            if (!CanApply)
                throw new InvalidOperationException();

            var selectedNewEpisodes = discoveredEpisodes.Where(e => e.IsSelected);

            var newShows = GetNewTVShows(selectedNewEpisodes);
            SaveNewTVShowsInRepository(newShows);

            var newSeasons = GetNewSeasons(selectedNewEpisodes);
            SaveNewSeasonsInRepository(newSeasons);

            SaveNewEpisodesInRepository(selectedNewEpisodes);
            GenerateThumbnailsForNewEpisodes(selectedNewEpisodes);
        }

        private IEnumerable<TVShow> GetNewTVShows(IEnumerable<DiscoveredEpisodeViewModel> selectedNewEpisodes)
        {
            var newShows = selectedNewEpisodes.Where(
                episode => episode.IsShowNew).Select(episode => new TVShow(episode.ShowName, episode.Description)).Distinct();
            return newShows;
        }

        private void SaveNewTVShowsInRepository(IEnumerable<TVShow> newShows)
        {
            foreach (var show in newShows)
                dataStore.TVShowRepository.Insert(show);
            dataStore.TVShowRepository.Save();
        }

        private IEnumerable<Season> GetNewSeasons(IEnumerable<DiscoveredEpisodeViewModel> selectedNewEpisodes)
        {
            var newSeasons = selectedNewEpisodes.Where(
                episode => episode.IsSeasonNew).Select(episode => new Season(episode.TVShowId, episode.SeasonNumber)).Distinct();
            return newSeasons;
        }

        private void SaveNewSeasonsInRepository(IEnumerable<Season> newSeasons)
        {
            foreach (var season in newSeasons)
                dataStore.SeasonRepository.Insert(season);
            dataStore.SeasonRepository.Save();
        }

        private void SaveNewEpisodesInRepository(IEnumerable<DiscoveredEpisodeViewModel> selectedNewEpisodes)
        {
            foreach (var episodeVM in selectedNewEpisodes)
            {
                var episode = episodeVM.BuildEpisode();
                dataStore.EpisodeRepository.Insert(episode);
            }
            dataStore.EpisodeRepository.Save();
        }

        private void GenerateThumbnailsForNewEpisodes(IEnumerable<DiscoveredEpisodeViewModel> selectedNewEpisodes)
        {
            foreach (var episodeVM in selectedNewEpisodes)
            {
                var episode = episodeVM.BuildEpisode();
                thumbnailGenerator.GenerateThumbnailForEpisode(episode);
            }
        }
    }

    public class DiscoveredEpisodeViewModel : ViewModelBase
    {
        private readonly IDataStore dataStore;

        private string showName;
        public string ShowName { 
            get { return showName; }
            set
            {
                if (value == showName)
                    return;
                showName = value;
                base.OnPropertyChanged("ShowName");
            }
        }

        private int seasonNumber;
        public int SeasonNumber
        {
            get { return seasonNumber; }
            set
            {
                if (value == seasonNumber)
                    return;
                seasonNumber = value;
                base.OnPropertyChanged("SeasonNumber");
            }
        }

        private int episodeNumber;
        public int EpisodeNumber
        {
            get { return episodeNumber; }
            set
            {
                if (value == episodeNumber)
                    return;
                episodeNumber = value;
                base.OnPropertyChanged("EpisodeNumber");
            }
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Filename { get; private set; }

        public TVShowId TVShowId { get { return TVShowId.CreateId(ShowName); } }
        public SeasonId SeasonId { get { return SeasonId.CreateId(TVShowId, SeasonNumber); } }
        public EpisodeId EpisodeId { get { return EpisodeId.CreateId(TVShowId, SeasonId, EpisodeNumber); } }

        public bool IsShowNew
        {
            get { return dataStore.TVShowRepository.Find(TVShowId) == null; }
        }

        public bool IsSeasonNew
        {
            get { return IsShowNew || dataStore.SeasonRepository.Find(SeasonId) == null; }
        }

        private bool isSelected = true;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value == isSelected)
                    return;

                isSelected = value;

                base.OnPropertyChanged("IsSelected");
            }
        }

        public DiscoveredEpisodeViewModel(IDataStore dataStore, Episode episode)
        {
            this.dataStore = dataStore;
            this.ShowName = episode.TVShowId.Name;
            this.SeasonNumber = episode.SeasonId.SeasonNumber;
            this.EpisodeNumber = episode.Number;
            this.Title = episode.Title;
            this.Description = episode.Description;
            this.Filename = episode.Filename;
        }

        public Episode BuildEpisode()
        {
            return new Episode(TVShowId, SeasonId, EpisodeNumber, Title, Description, Filename);
        }
    }
}

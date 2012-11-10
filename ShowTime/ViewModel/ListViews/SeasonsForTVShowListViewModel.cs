using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Data;
using System.Globalization;

namespace ShowTime.ViewModel.ListViews
{
    public class SeasonsForTVShowListViewModel : ViewModelBase
    {
        private IDataStore dataManager;
        public readonly TVShow TVShow;

        public string TVShowName
        {
            get { return TVShow.Name; }
        }

        public IEnumerable<SeasonsForTVShowListViewModel_SeasonListViewItemViewModel> Seasons
        {
            get { return dataManager.SeasonRepository
                .Query(s => s.TVShowId.Equals(TVShow.Id))
                .Select(s => new SeasonsForTVShowListViewModel_SeasonListViewItemViewModel(dataManager, s));
            }
        }

        private SeasonsForTVShowListViewModel_SeasonListViewItemViewModel selectedSeason = null;
        public SeasonsForTVShowListViewModel_SeasonListViewItemViewModel SelectedSeason
        {
            get { return selectedSeason; }
            set
            {
                if (value == selectedSeason)
                    return;

                selectedSeason = value;

                base.OnPropertyChanged("SelectedSeason");
            }
        }

        public SeasonsForTVShowListViewModel(IDataStore dataManager, TVShow tvShow)
        {
            this.dataManager = dataManager;
            this.TVShow = tvShow;
            this.DisplayName = TVShowName;
        }
    }

    #region Helper Classes- should be nested classes within TVShowSeasonsListViewModel but that doesn't work with XAML :(
    public class SeasonsForTVShowListViewModel_SeasonListViewItemViewModel : ViewModelBase
    {
        private IDataStore dataManager;

        public readonly Season Season;

        public int SeasonNumber
        {
            get { return Season.Number; }
        }

        public int EpisodeCount
        {
            get { return dataManager.EpisodeRepository.Query(e => e.SeasonId.Equals(Season.Id)).Count(); }
        }

        public SeasonsForTVShowListViewModel_SeasonListViewItemViewModel(IDataStore dataManager, Season season)
        {
            this.dataManager = dataManager;
            this.Season = season;
        }
    }

    #endregion
}

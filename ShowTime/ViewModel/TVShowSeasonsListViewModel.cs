using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Data;
using System.Globalization;

namespace ShowTime.ViewModel
{
    public class TVShowSeasonsListViewModel : ViewModelBase
    {
        private DataManager dataManager;
        public readonly TVShow TVShow;

        public string TVShowName
        {
            get { return TVShow.Name; }
        }

        public IEnumerable<TVShowSeasonsListViewModel_SeasonListViewItemViewModel> Seasons
        {
            get { return dataManager.SeasonRepository
                .Query(s => s.TVShowId.Equals(TVShow.Id))
                .Select(s => new TVShowSeasonsListViewModel_SeasonListViewItemViewModel(dataManager, s)); }
        }

        private TVShowSeasonsListViewModel_SeasonListViewItemViewModel selectedSeason = null;
        public TVShowSeasonsListViewModel_SeasonListViewItemViewModel SelectedSeason
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

        public TVShowSeasonsListViewModel(DataManager dataManager, TVShow tvShow)
        {
            this.dataManager = dataManager;
            this.TVShow = tvShow;
            this.DisplayName = TVShowName;
        }
    }

    #region Helper Classes- should be nested classes within TVShowSeasonsListViewModel but that doesn't work with XAML :(
    public class TVShowSeasonsListViewModel_SeasonListViewItemViewModel : ViewModelBase
    {
        private DataManager dataManager;

        public readonly Season Season;

        public int SeasonNumber
        {
            get { return Season.Number; }
        }

        public int EpisodeCount
        {
            get { return dataManager.EpisodeRepository.Query(e => e.SeasonId.Equals(Season.Id)).Count(); }
        }

        public TVShowSeasonsListViewModel_SeasonListViewItemViewModel(DataManager dataManager, Season season)
        {
            this.dataManager = dataManager;
            this.Season = season;
        }
    }

    //[ValueConversion(typeof(Season), typeof(TVShowSeasonsListViewModel_SeasonListViewItemViewModel))]
    //public class TVShowSeasonsListViewModel_SeasonListViewItemConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType,
    //        object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //            return null;

    //        Season season = (Season)value;
    //        return new TVShowSeasonsListViewModel_SeasonListViewItemViewModel(season);
    //    }

    //    public object ConvertBack(object value, Type targetType,
    //        object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //            return null;

    //        TVShowSeasonsListViewModel_SeasonListViewItemViewModel seasonViewModel = (TVShowSeasonsListViewModel_SeasonListViewItemViewModel)value;
    //        return seasonViewModel.Season;
    //    }
    //}
    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Data;
using System.Globalization;

namespace ShowTime.ViewModel
{
    public class TVShowListViewModel : ViewModelBase
    {
        private DataManager dataManager;

        public IEnumerable<TVShowListViewModel_TVShowListViewItemViewModel> TVShows
        {
            get;
            private set;
        }

        private TVShowListViewModel_TVShowListViewItemViewModel selectedShow = null;
        public TVShowListViewModel_TVShowListViewItemViewModel SelectedShow
        {
            get { return selectedShow; }
            set
            {
                if (value == selectedShow)
                    return;

                selectedShow = value;

                base.OnPropertyChanged("SelectedShow");
            }
        }

        public TVShowListViewModel(DataManager dataManager, IEnumerable<TVShow> tvShows)
        {
            this.dataManager = dataManager;
            this.TVShows = tvShows.Select(show => new TVShowListViewModel_TVShowListViewItemViewModel(dataManager, show));
        }
    }

    #region Helper Classes- should be nested classes within TVShowSeasonsListViewModel but that doesn't work with XAML :(
    public class TVShowListViewModel_TVShowListViewItemViewModel : ViewModelBase
    {
        private DataManager dataManager;
        public readonly TVShow TVShow;

        public string Name{
            get{return TVShow.Name;}
        }
        
        public string Description{
            get{return TVShow.Description;}
        }

        public int SeasonCount
        {
            get { return dataManager.SeasonRepository.Query(s=>s.TVShowId == TVShow.Id).Count(); }
        }

        public int EpisodeCount
        {
            get { return dataManager.EpisodeRepository.Query(e => e.TVShowId == TVShow.Id).Count(); }
        }

        public TVShowListViewModel_TVShowListViewItemViewModel(DataManager dataManager, TVShow show)
        {
            this.dataManager = dataManager;
            this.TVShow = show;
        }
    }

    //[ValueConversion(typeof(TVShow), typeof(TVShowListViewModel_TVShowListViewItemViewModel))]
    //public class TVShowListViewModel_TVShowListViewItemConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType,
    //        object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //            return null;

    //        TVShow show = (TVShow)value;
    //        return new TVShowListViewModel_TVShowListViewItemViewModel(dataManager, show);
    //    }

    //    public object ConvertBack(object value, Type targetType,
    //        object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //            return null;

    //        TVShowListViewModel_TVShowListViewItemViewModel showViewModel = (TVShowListViewModel_TVShowListViewItemViewModel)value;
    //        return showViewModel.TVShow;
    //    }
    //}
    #endregion
}

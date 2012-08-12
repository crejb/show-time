using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Data;
using System.Globalization;

namespace ShowTime.ViewModel
{
    public class SeasonEpisodesListViewModel : ViewModelBase
    {
        private DataManager dataManager;

        private Season season;

        private TVShow TVShow
        {
            get { return dataManager.TVShowRepository.Find(season.TVShowId); }
        }
         

        public string SeasonName
        {
            get { return TVShow.Name + " Season " + season.Number; }
        }

        public IEnumerable<SeasonEpisodesListViewModel_EpisodeListViewItemViewModel> Episodes
        {
            get
            {
                return dataManager.EpisodeRepository
                    .Query(ep => ep.SeasonId == season.Id)
                    .Select(e => new SeasonEpisodesListViewModel_EpisodeListViewItemViewModel(e));
            }
        }

        private Episode selectedEpisode = null;
        public Episode SelectedEpisode
        {
            get { return selectedEpisode; }
            set
            {
                if (value == selectedEpisode)
                    return;

                selectedEpisode = value;

                base.OnPropertyChanged("SelectedEpisode");
            }
        }

        public SeasonEpisodesListViewModel(DataManager dataManager, Season season)
        {
            this.dataManager = dataManager;
            this.season = season;
            this.DisplayName = Strings.SeasonEpisodesViewModel_DisplayName;
        }

        public override string ToString()
        {
            return SeasonName;
        }
    }

    #region Helper Classes- should be nested classes within TVShowSeasonsListViewModel but that doesn't work with XAML :(
    public class SeasonEpisodesListViewModel_EpisodeListViewItemViewModel : ViewModelBase
    {
        public readonly Episode Episode;

        public int Number
        {
            get { return Episode.Number; }
        }

        public string Title
        {
            get { return Episode.Title; }
        }

        public string Description
        {
            get { return Episode.Description; }
        }

        public SeasonEpisodesListViewModel_EpisodeListViewItemViewModel(Episode episode)
        {
            this.Episode = episode;
        }
    }

    [ValueConversion(typeof(Episode), typeof(SeasonEpisodesListViewModel_EpisodeListViewItemViewModel))]
    public class SeasonEpisodesListViewModel_EpisodeListViewItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            Episode episode = (Episode)value;
            return new SeasonEpisodesListViewModel_EpisodeListViewItemViewModel(episode);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            SeasonEpisodesListViewModel_EpisodeListViewItemViewModel episodeViewModel = (SeasonEpisodesListViewModel_EpisodeListViewItemViewModel)value;
            return episodeViewModel.Episode;
        }
    }
    #endregion
}

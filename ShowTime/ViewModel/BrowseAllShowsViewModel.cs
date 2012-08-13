using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Data;
using System.Globalization;
using ShowTime.Services;

namespace ShowTime.ViewModel
{
    public class BrowseAllShowsViewModel : ViewModelBase
    {
        private DataManager dataManager;
        private IEpisodeThumbnailFilenameProvider episodeThumbnailProvider;

        #region Properties
        private TVShowListViewModel showsViewModel;
        public TVShowListViewModel ShowsViewModel
        {
            get { return showsViewModel; }
            private set
            {
                if (value == showsViewModel)
                    return;

                if (showsViewModel != null)
                    showsViewModel.PropertyChanged -= showsViewModel_PropertyChanged;

                showsViewModel = value;

                if (showsViewModel != null)
                    showsViewModel.PropertyChanged += showsViewModel_PropertyChanged;

                OnPropertyChanged("ShowsViewModel");

                SelectedShowViewModel = (showsViewModel == null || showsViewModel.SelectedShow == null ? null : new TVShowSeasonsListViewModel(dataManager, showsViewModel.SelectedShow.TVShow));
            }
        }

        //public TVShow SelectedShow { get { return SelectedShowViewModel == null ? null : SelectedShowViewModel.TVShow; } }
        private TVShowSeasonsListViewModel selectedShowViewModel;
        public TVShowSeasonsListViewModel SelectedShowViewModel
        {
            get{ return selectedShowViewModel; }
            set
            {
                if (value == selectedShowViewModel)
                    return;

                if(selectedShowViewModel != null)
                    selectedShowViewModel.PropertyChanged -= selectedShowViewModel_PropertyChanged;

                selectedShowViewModel = value;

                if (selectedShowViewModel != null)
                    selectedShowViewModel.PropertyChanged += selectedShowViewModel_PropertyChanged;

                OnPropertyChanged("SelectedShowViewModel");

                SelectedSeasonViewModel = (selectedShowViewModel == null || selectedShowViewModel.SelectedSeason == null ? null : new SeasonEpisodesIconListViewModel(dataManager, episodeThumbnailProvider, selectedShowViewModel.SelectedSeason.Season));
            }
        }

        //public Season SelectedSeason { get { return SelectedSeasonViewModel == null ? null : SelectedSeasonViewModel.Season; } }
        private SeasonEpisodesIconListViewModel selectedSeasonViewModel;
        public SeasonEpisodesIconListViewModel SelectedSeasonViewModel
        {
            get { return selectedSeasonViewModel; }
            set
            {
                if (value == selectedSeasonViewModel)
                    return;

                if (selectedSeasonViewModel != null)
                    selectedSeasonViewModel.PropertyChanged -= selectedSeasonViewModel_PropertyChanged;

                selectedSeasonViewModel = value;

                if(selectedSeasonViewModel != null)
                    selectedSeasonViewModel.PropertyChanged += selectedSeasonViewModel_PropertyChanged;

                OnPropertyChanged("SelectedSeasonViewModel");

                SelectedEpisodeViewModel = (selectedSeasonViewModel == null || selectedSeasonViewModel.SelectedEpisode == null ? null : new EpisodeViewModel(dataManager, selectedSeasonViewModel.SelectedEpisode.Episode));
            }
        }

        //public Episode SelectedEpisode { get { return SelectedEpisodeViewModel.Episode; } }
        private EpisodeViewModel selectedEpisodeViewModel;
        public EpisodeViewModel SelectedEpisodeViewModel
        {
            get { return selectedEpisodeViewModel; }
            set
            {
                if (value == selectedEpisodeViewModel)
                    return;

                selectedEpisodeViewModel = value;
                OnPropertyChanged("SelectedEpisodeViewModel");
            }
        }
        #endregion

        public BrowseAllShowsViewModel(DataManager dataManager, IEpisodeThumbnailFilenameProvider episodeThumbnailProvider)
        {
            this.dataManager = dataManager;
            this.episodeThumbnailProvider = episodeThumbnailProvider;
            this.ShowsViewModel = new TVShowListViewModel(dataManager, dataManager.TVShowRepository.GetAll());
        }

        private void showsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedShow")
            {
                if (showsViewModel == null)
                {
                    SelectedShowViewModel = null;
                }
                else
                {
                    SelectedShowViewModel = new TVShowSeasonsListViewModel(dataManager, showsViewModel.SelectedShow.TVShow);
                }
            }
        }

        private void selectedShowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedSeason")
            {
                if (selectedShowViewModel == null)
                {
                    SelectedSeasonViewModel = null;
                }
                else
                {
                    SelectedSeasonViewModel = new SeasonEpisodesIconListViewModel(dataManager, episodeThumbnailProvider, selectedShowViewModel.SelectedSeason.Season);
                }
            }
        }

        private void selectedSeasonViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedEpisode")
            {
                if (selectedSeasonViewModel == null)
                {
                    SelectedEpisodeViewModel = null;
                }
                else
                {
                    SelectedEpisodeViewModel = new EpisodeViewModel(dataManager, selectedSeasonViewModel.SelectedEpisode.Episode);
                }
            }
        }
    }
}

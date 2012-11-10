using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Data;
using System.Globalization;
using ShowTime.Services;
using ShowTime.ViewModel.ListViews;

namespace ShowTime.ViewModel
{
    public class BrowseAllShowsViewModel : ViewModelBase
    {
        private IDataStore dataManager;
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

                SelectedShowViewModel = (showsViewModel == null || showsViewModel.SelectedShow == null ? null : new SeasonsForTVShowListViewModel(dataManager, showsViewModel.SelectedShow.TVShow));
            }
        }

        //public TVShow SelectedShow { get { return SelectedShowViewModel == null ? null : SelectedShowViewModel.TVShow; } }
        private SeasonsForTVShowListViewModel selectedShowViewModel;
        public SeasonsForTVShowListViewModel SelectedShowViewModel
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

                SelectedSeasonViewModel = (selectedShowViewModel == null || selectedShowViewModel.SelectedSeason == null ? null : new EpisodesForSeasonListViewModel(dataManager, episodeThumbnailProvider, selectedShowViewModel.SelectedSeason.Season));
            }
        }

        private EpisodesForSeasonListViewModel selectedSeasonViewModel;
        public EpisodesForSeasonListViewModel SelectedSeasonViewModel
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

                SelectedEpisodeViewModel = (selectedSeasonViewModel == null || selectedSeasonViewModel.SelectedEpisode == null ? null : new EpisodePreviewViewModel(dataManager, selectedSeasonViewModel.SelectedEpisode.Episode.Id, episodeThumbnailProvider));
            }
        }

        private EpisodePreviewViewModel selectedEpisodeViewModel;
        public EpisodePreviewViewModel SelectedEpisodeViewModel
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

        public BrowseAllShowsViewModel(IDataStore dataManager, IEpisodeThumbnailFilenameProvider episodeThumbnailProvider)
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
                    SelectedShowViewModel = new SeasonsForTVShowListViewModel(dataManager, showsViewModel.SelectedShow.TVShow);
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
                    SelectedSeasonViewModel = new EpisodesForSeasonListViewModel(dataManager, episodeThumbnailProvider, selectedShowViewModel.SelectedSeason.Season);
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
                    SelectedEpisodeViewModel = new EpisodePreviewViewModel(dataManager, selectedSeasonViewModel.SelectedEpisode.Episode.Id, episodeThumbnailProvider);
                }
            }
        }
    }
}

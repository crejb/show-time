using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Input;
using ShowTime.Services.CloseActions;

namespace ShowTime.ViewModel
{
    public class EpisodePreviewViewModel
    {
        private IDataStore dataStore;
        private Episode episode;

        public string ShowName { get { return episode.TVShowId.Name; } }
        public int SeasonNumber { get { return episode.SeasonId.SeasonNumber; } }
        public int EpisodeNumber { get { return episode.Number; } }
        public string Filename { get { return episode.Title; } }
        public int ViewCount { get; set; }
        public DateTime LastWatched { get; set; }
        public string EpisodeThumbnail { get; set; }

        public ICommand NavigateToFileCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }

        public EpisodePreviewViewModel(IDataStore dataStore, EpisodeId episodeId, Services.IEpisodeThumbnailFilenameProvider thumbnailProvider)
        {
            this.dataStore = dataStore;
            this.episode = dataStore.EpisodeRepository.Find(episodeId);

            EpisodeThumbnail = thumbnailProvider.GetThumbnailFilenameForEpisode(episode).ActualFilename;
            //TODO: LastWatchedRepo
            //TODO: BookmarkRepo
            ViewCount = 2;
            LastWatched = DateTime.Now;

            NavigateToFileCommand = new ViewModel.Commands.RelayCommand(param =>
            {
                System.Diagnostics.Process.Start("explorer.exe", @"/select, " + episode.Filename);
            });

            PlayCommand = new ViewModel.Commands.RelayCommand(param =>
            {
                PlayEpisode();
            });
        }

        private VideoPlayRequestHandler playHandler;

        private void PlayEpisode()
        {
            if (playHandler == null)
            {
                playHandler = new VideoPlayRequestHandler(
                    new VideoPlayerController(),
                    dataStore,
                    new EpisodeCloseActionsCalculatorProvider(),
                    new EpisodeCloseActionsExecutor(dataStore)
                    );
            }

            playHandler.PlayVideo(
                new VideoPlayRequest(episode.Id)
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Windows.Input;

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
        }

    }
}

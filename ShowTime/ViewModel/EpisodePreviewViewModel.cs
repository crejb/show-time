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
        public DateTime? LastWatchedTime { get; set; }
        public string EpisodeThumbnail { get; set; }

        public ICommand NavigateToFileCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand ClearBookmarkCommand { get; private set; }
        public ICommand ResetLastWatchedCommand { get; private set; }
        public ICommand SetLastWatchedCommand { get; private set; }

        private readonly Bookmark bookmark;
        public bool HasBookmark { get { return bookmark != null; } }
        public TimeSpan? BookmarkTime { get { return HasBookmark ? bookmark.Time : (TimeSpan?)null; } }

        public EpisodePreviewViewModel(IDataStore dataStore, EpisodeId episodeId, Services.IEpisodeThumbnailFilenameProvider thumbnailProvider)
        {
            this.dataStore = dataStore;
            this.episode = dataStore.EpisodeRepository.Find(episodeId);

            EpisodeThumbnail = thumbnailProvider.GetThumbnailFilenameForEpisode(episode).ActualFilename;

            // Should be either 1 or 0 bookmarks for the episode
            this.bookmark = dataStore.BookmarkRepository.Query(bk => bk.EpisodeId.Equals(episodeId)).
                FirstOrDefault();

            var lastWatchedEntries = dataStore.LastWatchedRepository.Query(lwe => lwe.EpisodeId.Equals(episodeId));


            if (lastWatchedEntries.Any())
            {
                ViewCount = lastWatchedEntries.Count();
                LastWatchedTime = lastWatchedEntries.Last().Time;
            }
            else
            {
                ViewCount = 0;
                LastWatchedTime = null;
            }

            NavigateToFileCommand = new ViewModel.Commands.RelayCommand(param =>
            {
                System.Diagnostics.Process.Start("explorer.exe", @"/select, " + episode.Filename);
            });

            PlayCommand = new ViewModel.Commands.RelayCommand(param =>
            {
                PlayEpisode();
            });

            ClearBookmarkCommand = new ViewModel.Commands.RelayCommand(param =>
            {
                ClearBookmark();
            });

            ResetLastWatchedCommand = new ViewModel.Commands.RelayCommand(param =>
            {
                ResetLastWatched();
            });

            SetLastWatchedCommand = new ViewModel.Commands.RelayCommand(param =>
            {
                SetLastWatched();
            });
        }

        private VideoPlayRequestHandler playHandler;

        private void PlayEpisode()
        {
            var playHandler = new VideoPlayRequestHandler(
                new VideoPlayerController(),
                dataStore,
                new EpisodeCloseActionsCalculatorProvider(),
                new EpisodeCloseActionsExecutor(dataStore)
                );

            playHandler.PlayVideo(
                new VideoPlayRequest(episode.Id, bookmark)
            );
        }

        private void ClearBookmark()
        {
            var bookmarksForEpisode = dataStore.BookmarkRepository.Query(bk => bk.EpisodeId.Equals(episode.Id));
            foreach (var bookmark in bookmarksForEpisode.ToList())
            {
                dataStore.BookmarkRepository.Delete(bookmark);
            }
            dataStore.BookmarkRepository.Save();
        }

        private void ResetLastWatched()
        {
            var lastWatchedEntriesForEpisode = dataStore.LastWatchedRepository.Query(lwe => lwe.EpisodeId.Equals(episode.Id));
            foreach (var lwe in lastWatchedEntriesForEpisode.ToList())
            {
                dataStore.LastWatchedRepository.Delete(lwe);
            }
            dataStore.LastWatchedRepository.Save();
        }

        private void SetLastWatched()
        {
            var newLastWatchedEntry = new LastWatchedEntry(episode.Id, DateTime.Now);
            dataStore.LastWatchedRepository.Insert(newLastWatchedEntry);
            dataStore.LastWatchedRepository.Save();
        }
    }
}

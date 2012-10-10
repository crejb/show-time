using System;
using ShowTime.Model;

namespace ShowTime.Services.CloseActions
{
    public interface IEpisodeCloseActionsExecutor
    {
        void ExecuteActions(ShowTime.Model.Episode episode, EpisodePlayerClosedEventArgs e, IEpisodeCloseActionsCalculator actionCalculator);
    }

    public class EpisodeCloseActionsExecutor : IEpisodeCloseActionsExecutor
    {
        private IDataStore dataStore;

        public EpisodeCloseActionsExecutor(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public void ExecuteActions(Episode episode, EpisodePlayerClosedEventArgs e, IEpisodeCloseActionsCalculator actionCalculator)
        {
            if (actionCalculator.ShouldClearBookmark)
                ClearEpisodeBookmark(episode);

            if (actionCalculator.ShouldSetBookmark)
                SetEpisodeBookmark(episode, e.SeekPositionAtClose);

            if (actionCalculator.ShouldMarkAsWatched)
                MarkEpisodeAsWatched(episode);
        }

        private void ClearEpisodeBookmark(Episode episode)
        {
            var bookmarksForEpisode = dataStore.BookmarkRepository.Query(bk => bk.EpisodeId.Equals(episode.Id));

            foreach (var bookmark in bookmarksForEpisode)
            {
                dataStore.BookmarkRepository.Delete(bookmark);
            }
        }

        private void SetEpisodeBookmark(Episode episode, TimeSpan bookmarkTime)
        {
            ClearEpisodeBookmark(episode);
            var bookmark = new Bookmark(episode.Id, bookmarkTime);
            dataStore.BookmarkRepository.Insert(bookmark);
        }

        private void MarkEpisodeAsWatched(Episode episode)
        {
            //TODO: Add Watched repository
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Services.CloseActions
{
    public interface IEpisodeCloseActionsCalculatorProvider
    {
        IEpisodeCloseActionsCalculator GetActionsCalculator(Episode episode, EpisodePlayerClosedEventArgs e);
    }

    public class EpisodeCloseActionsCalculatorProvider : IEpisodeCloseActionsCalculatorProvider
    {
        public IEpisodeCloseActionsCalculator GetActionsCalculator(Episode episode, EpisodePlayerClosedEventArgs e)
        {
            return new EpisodeCloseActionsCalculator(episode, e);
        }
    }

    public interface IEpisodeCloseActionsCalculator
    {
        bool ShouldClearBookmark { get; }
        bool ShouldMarkAsWatched { get; }
        bool ShouldSetBookmark { get; }
    }

    public class EpisodeCloseActionsCalculator : IEpisodeCloseActionsCalculator
    {
        public bool ShouldClearBookmark { get; private set; }
        public bool ShouldSetBookmark { get; private set; }
        public bool ShouldMarkAsWatched { get; private set; }

        public EpisodeCloseActionsCalculator(Episode episode, EpisodePlayerClosedEventArgs e)
        {
            CalculateShouldClearBookmark(episode, e);
            CalculateShouldSetBookmark(episode, e);
            CalculateShouldMarkAsWatched(episode, e);
        }

        private void CalculateShouldClearBookmark(Episode episode, EpisodePlayerClosedEventArgs e)
        {
            ShouldClearBookmark = false;

            if (IsExitImmediatelyAfterOpen(e))
            {
                ShouldClearBookmark = false;
                return;
            }

            if (IsExitTimeNearEnd(e))
            {
                ShouldClearBookmark = true;
            }
        }

        private void CalculateShouldSetBookmark(Episode episode, EpisodePlayerClosedEventArgs e)
        {
            if (IsExitImmediatelyAfterOpen(e)
                || IsExitTimeNearStart(e)
                || IsExitTimeNearEnd(e))
            {
                ShouldSetBookmark = false;
            }
            else
            {
                ShouldSetBookmark = true;
            }
        }

        private void CalculateShouldMarkAsWatched(Episode episode, EpisodePlayerClosedEventArgs e)
        {
            ShouldMarkAsWatched = false;

            if (IsExitImmediatelyAfterOpen(e))
            {
                ShouldMarkAsWatched = false;
                return;
            }

            if (IsExitTimeNearEnd(e))
            {
                ShouldMarkAsWatched = true;
            }
        }

        TimeSpan DISTANCE_FROM_END_THRESHOLD = TimeSpan.FromSeconds(10);
        TimeSpan DISTANCE_FROM_START_THRESHOLD = TimeSpan.FromSeconds(10);
        TimeSpan IMMEDIATE_TIME_THRESHOLD = TimeSpan.FromSeconds(10);

        private bool IsExitTimeNearEnd(EpisodePlayerClosedEventArgs e)
        {
            var timeUntilEnd = e.LastVideoLength.Subtract(e.SeekPositionAtClose);
            return timeUntilEnd <= DISTANCE_FROM_END_THRESHOLD;
        }

        private bool IsExitImmediatelyAfterOpen(EpisodePlayerClosedEventArgs e)
        {
            return e.ElapsedTimeSinceStarted <= IMMEDIATE_TIME_THRESHOLD;
        }

        private bool IsExitTimeNearStart(EpisodePlayerClosedEventArgs e)
        {
            return e.SeekPositionAtClose <= DISTANCE_FROM_START_THRESHOLD;
        }
    }
}

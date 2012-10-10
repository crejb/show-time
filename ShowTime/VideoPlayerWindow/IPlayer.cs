using System;
using ShowTime.Model;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;
using System.Diagnostics;
using ShowTime.VideoPlayerWindow;

namespace ShowTime
{
    public interface IPlayer
    {
        Episode CurrentEpisode { get; }

        void Play(Episode episode, Bookmark bookmrk);

        event EventHandler<EpisodePlayerClosedEventArgs> Closed;
        event EventHandler<BookmarkRequestedEventArgs> BookmarkRequested;
    }

    public class VideoPlayerController : IPlayer
    {
        public event EventHandler<EpisodePlayerClosedEventArgs> Closed;
        public event EventHandler<BookmarkRequestedEventArgs> BookmarkRequested;

        private Stopwatch elapsedTimer;

        public Episode CurrentEpisode
        {
            get; private set;
        }

        private readonly PlayerWindowView playerView;
        private readonly ShowTime.VideoPlayerWindow.PlayerWindowViewModel playerModel;

        public VideoPlayerController()
        {
            playerView = new PlayerWindowView();
            playerModel = new ShowTime.VideoPlayerWindow.PlayerWindowViewModel();
            playerModel.Closed += new EventHandler<VideoClosedEventArgs>(playerModel_Closed);
            playerView.DataContext = playerModel;

            playerModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(playerModel_PropertyChanged);
            
            elapsedTimer = new Stopwatch();
        }

        void playerModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SeekPosition")
            {
                Console.WriteLine("{0} of {1} for {2}", playerModel.SeekPosition, playerModel.VideoLength, playerModel.VideoFilename);
            }
        }

        public void Play(Episode episode, Bookmark bookmark)
        {
            CurrentEpisode = episode;

            playerModel.VideoFilename = episode.Filename;
            if (bookmark != null)
            {
                playerModel.SeekPosition = bookmark.Time;
            }

            elapsedTimer.Reset();
            elapsedTimer.Start();

            playerView.Show();
        }

        void playerModel_Closed(object sender, VideoClosedEventArgs e)
        {
            elapsedTimer.Stop();

            var handler = Closed;
            if (handler != null)
            {
                handler(this, BuildClosedEventArgs(e));
            }
        }

        private EpisodePlayerClosedEventArgs BuildClosedEventArgs(VideoClosedEventArgs e)
        {
            var timeFromStartToStop = elapsedTimer.Elapsed;
            return new EpisodePlayerClosedEventArgs(CurrentEpisode, e.Reason, e.LastVideoLength, e.SeekPositionAtClose, timeFromStartToStop);
        }
    }

    public class EpisodePlayerClosedEventArgs : VideoClosedEventArgs
    {
        public readonly Episode Episode;
        public readonly TimeSpan ElapsedTimeSinceStarted;

        public EpisodePlayerClosedEventArgs(Episode episode,
            CloseReason reason,
            TimeSpan lastEpisodeLength,
            TimeSpan seekPositionAtClose,
            TimeSpan elapsedTimeSinceStarted)
            : base(reason, lastEpisodeLength, seekPositionAtClose)
        {
            this.Episode = episode;
            this.ElapsedTimeSinceStarted = elapsedTimeSinceStarted;
        }
    }

    public class BookmarkRequestedEventArgs : EventArgs
    {
        public Episode Episode { get; private set; }
        public readonly TimeSpan SeekPosition;
    }
}

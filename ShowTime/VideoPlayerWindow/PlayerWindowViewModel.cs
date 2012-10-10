using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.ViewModel;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;

namespace ShowTime.VideoPlayerWindow
{
    public class PlayerWindowViewModel : ViewModelBase
    {
        public event EventHandler<VideoClosedEventArgs> Closed;

        private string videoFilename;
        public string VideoFilename
        {
            get { return videoFilename; }
            set
            {
                if (value == videoFilename)
                    return;

                videoFilename = value;
                base.OnPropertyChanged("VideoFilename");
            }
        }

        private TimeSpan seekPosition;
        public TimeSpan SeekPosition
        {
            get { return seekPosition; }
            set
            {
                if (value == seekPosition)
                    return;

                seekPosition = value;
                base.OnPropertyChanged("SeekPosition");
            }
        }

        private TimeSpan videoLength;
        public TimeSpan VideoLength
        {
            get { return videoLength; }
            set
            {
                if (value == videoLength)
                    return;

                videoLength = value;
                base.OnPropertyChanged("VideoLength");
            }
        }

        public ICommand CreateBookmarkCommand { get; private set; }
        public ICommand VideoCompleteCommand { get; private set; }
        public ICommand CloseVideoCommand { get; private set; }

        public PlayerWindowViewModel()
        {
            this.CreateBookmarkCommand = new RelayCommand(param => CreateBookmarkHandler());
            this.VideoCompleteCommand = new RelayCommand(param => VideoCompletedHandler());
            this.CloseVideoCommand = new RelayCommand(param => VideoClosedHandler());
        }

        private void VideoClosedHandler()
        {
            OnVideoClose(VideoClosedEventArgs.CloseReason.UserClosed);
        }

        private void VideoCompletedHandler()
        {
            OnVideoClose(VideoClosedEventArgs.CloseReason.VideoCompleted);
        }

        private void CreateBookmarkHandler()
        {
            throw new NotImplementedException();
        }

        private void OnVideoClose(VideoClosedEventArgs.CloseReason closeReason)
        {
            var args = BuildPlayerClosedEventArgs(closeReason);
            var handler = Closed;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private VideoClosedEventArgs BuildPlayerClosedEventArgs(VideoClosedEventArgs.CloseReason closeReason)
        {
            return new VideoClosedEventArgs(closeReason, VideoLength, SeekPosition);
        }
    }

    public class VideoClosedEventArgs : EventArgs
    {
        public enum CloseReason
        {
            VideoCompleted,
            UserClosed,
            UserClosedWithoutBookmark
        }

        public readonly CloseReason Reason;
        public readonly TimeSpan LastVideoLength;
        public readonly TimeSpan SeekPositionAtClose;

        public VideoClosedEventArgs(
            CloseReason reason,
            TimeSpan lastVideoLength,
            TimeSpan seekPositionAtClose)
        {
            this.Reason = reason;
            this.LastVideoLength = lastVideoLength;
            this.SeekPositionAtClose = seekPositionAtClose;
        }
    }
}
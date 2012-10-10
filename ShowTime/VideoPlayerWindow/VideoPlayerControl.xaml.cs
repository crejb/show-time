using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ShowTime.View.Controls
{
    /// <summary>
    /// Interaction logic for VideoPlayerControl.xaml
    /// </summary>
    public partial class VideoPlayerControl : UserControl
    {
        public string Filename
        {
            get { return (string)GetValue(FilenameProperty); }
            set { SetValue(FilenameProperty, value); }
        }

        public TimeSpan VideoLength
        {
            get { return (TimeSpan)GetValue(VideoLengthProperty); }
            set { throw new Exception("An attempt ot modify Read-Only property"); }
        }

        public TimeSpan SeekPosition
        {
            get { return (TimeSpan)GetValue(SeekPositionProperty); }
            set { SetValue(SeekPositionProperty, value); }
        }

        public ICommand VideoCompleteCommand
        {
            get { return (ICommand)GetValue(VideoCompleteCommandProperty); }
            set { SetValue(VideoCompleteCommandProperty, value); }
        }

        #region Dependency Properties
        public static readonly DependencyProperty FilenameProperty =
            DependencyProperty.Register("Filename", typeof(string), typeof(VideoPlayerControl), new UIPropertyMetadata("", FilenamePropertyChangedCallback));
        public static readonly DependencyProperty VideoLengthProperty =
            DependencyProperty.Register("VideoLength", typeof(TimeSpan), typeof(VideoPlayerControl), new UIPropertyMetadata(TimeSpan.Zero));
        public static readonly DependencyProperty SeekPositionProperty =
            DependencyProperty.Register("SeekPosition", typeof(TimeSpan), typeof(VideoPlayerControl), new UIPropertyMetadata(TimeSpan.Zero, SeekPositionPropertyChangedCallback));
        public static readonly DependencyProperty VideoCompleteCommandProperty =
            DependencyProperty.Register("VideoCompleteCommand", typeof(ICommand), typeof(VideoPlayerControl), new UIPropertyMetadata(null));
        #endregion

        #region Dependency Property Callbacks
        /// <summary>
        /// When the Seek Position is set, update the seek position of the media control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void SeekPositionPropertyChangedCallback
                        (DependencyObject d,
                         DependencyPropertyChangedEventArgs e)
        {
            var thisObject = d as VideoPlayerControl;

            // Prevent recursive setting of this property.
            if (thisObject.settingSlider)
                return;

            thisObject.ctrlMedia.Position = thisObject.SeekPosition;
        }

        /// <summary>
        /// When Filename is changed, load the video file into the media control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void FilenamePropertyChangedCallback
                        (DependencyObject d,
                         DependencyPropertyChangedEventArgs e)
        {
            var thisObject = d as VideoPlayerControl;
            thisObject.LoadVideo();
        }
        #endregion

        private DispatcherTimer seekPositionPollTimer;
        private DispatcherTimer dragTimer;
        private bool settingSlider = false;
        private bool isDragging = false;
        private bool isFullScreen = false;

        public VideoPlayerControl()
        {
            InitializeComponent();

            seekPositionPollTimer = new DispatcherTimer();
            seekPositionPollTimer.Interval = TimeSpan.FromSeconds(1);
            seekPositionPollTimer.Tick += new EventHandler(seekPositionPollTimer_Tick);

            dragTimer = new DispatcherTimer();
            dragTimer.Interval = TimeSpan.FromSeconds(0.3);
            dragTimer.Tick += new EventHandler(dragTimer_Tick);

            ctrlMedia.MediaOpened += new RoutedEventHandler(ctrlMedia_MediaOpened);
            ctrlMedia.MediaEnded += new RoutedEventHandler(ctrlMedia_MediaEnded);
        }

        private void ctrlMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            var handler = VideoCompleteCommand;
            if(handler != null && handler.CanExecute(null)){
                handler.Execute(null);
            }
        }

        /// <summary>
        /// Set the video source to the current filename, and start playing the video
        /// </summary>
        private void LoadVideo()
        {
            ctrlMedia.Source = new Uri(this.Filename);
            ctrlMedia.Play();
        }

        private void ctrlPlayButton_Click(object sender, RoutedEventArgs e)
        {
            ctrlMedia.Play();
        }

        private void ctrlPauseButton_Click(object sender, RoutedEventArgs e)
        {
            ctrlMedia.Pause();
        }

        private void ctrlMedia_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan videoLength = ctrlMedia.NaturalDuration.TimeSpan;
            SetValue(VideoLengthProperty, videoLength);

            SetTimeSliderProperties(videoLength);

            seekPositionPollTimer.Start();
        }

        private void SetTimeSliderProperties(TimeSpan videoLength)
        {
            ctrlTimeSlider.Maximum = videoLength.TotalSeconds;
            ctrlTimeSlider.SmallChange = 1;
            ctrlTimeSlider.LargeChange = Math.Min(10, videoLength.Seconds / 10);
        }

        /// <summary>
        /// Can't bind directly to the media control seek position, so poll
        /// the value once per second, updating the slider timer slider position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seekPositionPollTimer_Tick(object sender, EventArgs e)
        {
            if (isDragging)
                return;

            settingSlider = true;
            ctrlTimeSlider.Value = ctrlMedia.Position.TotalSeconds;
            SetValue(SeekPositionProperty, ctrlMedia.Position);
            settingSlider = false;
        }

        /// <summary>
        /// When the user clicks on the time slider to jump to a position, update the
        /// seek position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (settingSlider || isDragging)
                return;

            ctrlMedia.Position = TimeSpan.FromSeconds(ctrlTimeSlider.Value);
        }

        /// <summary>
        /// When the user starts to drag the slider:
        /// -Pause the video
        /// While the user is dragging the slider:
        /// -Every 300ms, sync the video seek position with the current position that the user has dragged to (dragTimer_Tick)
        /// When the user finishes dragging the slider:
        /// -Set the video seek position to the current position of the slider
        /// -Unpause the video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlTimeSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            ctrlMedia.Pause();
            dragTimer.Start();
            this.isDragging = true;
        }

        private void dragTimer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
                return;

            ctrlMedia.Position = TimeSpan.FromSeconds(ctrlTimeSlider.Value);
        }

        private void ctrlTimeSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            dragTimer.Stop();
            ctrlMedia.Position = TimeSpan.FromSeconds(ctrlTimeSlider.Value);
            ctrlMedia.Play();
            this.isDragging = false;
        }

        private void ctrlMedia_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (isFullScreen)
            {
                isFullScreen = false;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowStyle = WindowStyle.None;
                window.WindowState = WindowState.Maximized;
                isFullScreen = true;
            }
        }
    }
}

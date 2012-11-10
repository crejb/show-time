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
using System.Windows.Shapes;
using ShowTime.ViewModel.Commands;

namespace ShowTime
{
    /// <summary>
    /// Interaction logic for PlayerWindowView.xaml
    /// </summary>
    public partial class PlayerWindowView : Window
    {
        private bool isClosingDueToVideoComplete = false;

        public PlayerWindowView()
        {
            InitializeComponent();

            // Bind the Video Completed command of the VIdeo Player Control
            // Manually close the window automatically without firing the CloseVideoCommand,
            // and fire the VideoCompleteCommand
            ctlVideoPlayer.VideoCompleteCommand = new RelayCommand(param => VideoCompletedHandler());
            ctlVideoPlayer.VideoStoppedCommand = new RelayCommand(param => VideoStoppedHandler());
        }

        private void VideoCompletedHandler()
        {
            isClosingDueToVideoComplete = true;
            this.Close();
            isClosingDueToVideoComplete = false;

            ICommand completedCommand = ViewUtilities.GetValueFromDataContext("VideoCompleteCommand", DataContext) as ICommand;
            if (completedCommand.CanExecute(null))
            {
                completedCommand.Execute(null);
            }
        }

        private void VideoStoppedHandler()
        {
            isClosingDueToVideoComplete = false;
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (isClosingDueToVideoComplete)
            {
                return;
            }

            ICommand closeCommand = ViewUtilities.GetValueFromDataContext("CloseVideoCommand", DataContext) as ICommand;
            if (closeCommand.CanExecute(null))
            {
                closeCommand.Execute(null);
            }
        }
    }

    public static class ViewUtilities
    {
        public static object GetValueFromDataContext(string dpName, object dataContext)
        {
            Binding b = new Binding(dpName)
            {
                Source = dataContext
            };

            return EvalBinding(b);
        }
        public static object EvalBinding(Binding b)
        {
            DummyDO d = new DummyDO();
            BindingOperations.SetBinding(d, DummyDO.ValueProperty, b);
            return d.Value;
        }

        private class DummyDO : DependencyObject
        {
            public object Value
            {
                get { return (object)GetValue(ValueProperty); }
                set { SetValue(ValueProperty, value); }
            }

            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(object), typeof(DummyDO), new UIPropertyMetadata(null));

        }
    }
}

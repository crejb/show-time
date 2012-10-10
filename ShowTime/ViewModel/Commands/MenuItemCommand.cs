using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;

namespace ShowTime.ViewModel.Commands
{
    public class MenuItemCommand
    {
        public static ImageSource BuildImageFromResource(string imageUri)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(imageUri, UriKind.Relative);
            img.EndInit();
            return img;
        }

        public static ImageSource BuildImageFromFile(string imageUri)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(imageUri);
            img.EndInit();
            return img;
        }

        public ImageSource MenuItemImage { get; private set; }
        public string MenuItemText { get; private set; }
        public object Tag { get; private set; }

        public ICommand SelectedCommand { get; private set; }
        public ICommand ConfirmedCommand { get; private set; }

        public MenuItemCommand(string menuItemText, ImageSource image, object tag, ICommand selectedCommand, ICommand confirmedCommand)
        {
            MenuItemText = menuItemText;
            Tag = tag;
            MenuItemImage = image;

            SelectedCommand = selectedCommand;
            ConfirmedCommand = confirmedCommand;
        }

        public void InvokeSelectedCommand()
        {
            InvokeCommand(SelectedCommand);
        }

        public void InvokeConfirmedCommand()
        {
            InvokeCommand(ConfirmedCommand);
        }

        private void InvokeCommand(ICommand command)
        {
            if (command != null && command.CanExecute(Tag))
            {
                command.Execute(Tag);
            }
        }
    }
}

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
        public ImageSource MenuItemImage { get; private set; }
        public string MenuItemText { get; private set; }
        public object Tag { get; private set; }

        public ICommand SelectedCommand { get; private set; }
        public ICommand ConfirmedCommand { get; private set; }

        public MenuItemCommand(string menuItemText, string imageUri, object tag, ICommand selectedCommand, ICommand confirmedCommand)
        {
            MenuItemText = menuItemText;
            Tag = tag;
            BuildImage(imageUri);

            SelectedCommand = selectedCommand;
            ConfirmedCommand = confirmedCommand;
        }

        private void BuildImage(string imageUri)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(imageUri, UriKind.Relative);
            img.EndInit();
            MenuItemImage = img;
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

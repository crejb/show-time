using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.ViewModel.Commands;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShowTime.ViewModel
{
    public class MainMenuViewModel : ViewModelBase
    {
        public event Action WatchShowsSelected;
        public event Action ManageShowsSelected;

        public IEnumerable<MenuItemCommand> MenuItemCommands { get; private set; }

        public MainMenuViewModel()
        {
            MenuItemCommands = new List<MenuItemCommand>
            {
                new MenuItemCommand(
                    "Watch TV Shows",
                    "../Resources/tv.jpg",
                    null,
                    new RelayCommand(
                            param => OnWatchShowsSelected()
                    )
                ),
                new MenuItemCommand(
                    "Manage TV Shows",
                    "../Resources/folder.jpg",
                    null,
                    new RelayCommand(
                            param => OnManageShowsSelected()
                    )
                )
            };
        }

        private void OnManageShowsSelected()
        {
            var handler = ManageShowsSelected;
            if (handler != null)
                handler();
        }

        private void OnWatchShowsSelected()
        {
            var handler = WatchShowsSelected;
            if (handler != null)
                handler();
        }
    }
}

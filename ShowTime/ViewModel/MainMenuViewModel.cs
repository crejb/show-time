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
        public event Action BrowseShowsSelected;

        public IEnumerable<MenuItemCommand> MenuItemCommands { get; private set; }

        public MainMenuViewModel()
        {
            MenuItemCommands = new List<MenuItemCommand>
            {
                new MenuItemCommand(
                    "Watch TV Shows",
                    MenuItemCommand.BuildImageFromResource("../Resources/tv.jpg"),
                    null,
                    null,
                    new RelayCommand(
                            param => OnWatchShowsSelected()
                    )
                ),
                new MenuItemCommand(
                    "Browse Database",
                    MenuItemCommand.BuildImageFromResource("../Resources/tv.jpg"),
                    null,
                    null,
                    new RelayCommand(
                            param => OnBrowseShowsSelected()
                    )
                ),
                new MenuItemCommand(
                    "Manage TV Shows",
                    MenuItemCommand.BuildImageFromResource("../Resources/folder.jpg"),
                    null,
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

        private void OnBrowseShowsSelected()
        {
            var handler = BrowseShowsSelected;
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

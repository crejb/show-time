using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.ViewModel.Commands;
using System.Windows.Input;
using ShowTime.Model;

namespace ShowTime.ViewModel
{
    public class TvShowIconsListViewModel : ViewModelBase
    {
        public event Action<TVShowId> TvShowSelected;

        public ICommand SelectShowCommand { get; private set; }

        private readonly IDataStore dataStore;
        public IEnumerable<MenuItemCommand> MenuItemCommands { get { return BuildMenuItemCommands(); } }

        public TvShowIconsListViewModel(IDataStore dataStore)
        {
            this.dataStore = dataStore;
            this.SelectShowCommand = new RelayCommand(param => OnShowSelected((param as MenuItemCommand).Tag as TVShowId));
        }

        private IEnumerable<MenuItemCommand> BuildMenuItemCommands()
        {
            return dataStore.TVShowRepository.GetAll().Select(
                show=>
                new MenuItemCommand(
                    show.Name,
                    MenuItemCommand.BuildImageFromResource("../Resources/tv.jpg"),
                    show.Id,
                    null, null
                ));
        }

        private void OnShowSelected(TVShowId id)
        {
            var handler = TvShowSelected;
            if (handler != null)
                handler(id);
        }
    }
}

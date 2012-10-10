using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ShowTime.ViewModel.Commands;
using ShowTime.Model;

namespace ShowTime.ViewModel
{
    public class SeasonIconsListViewModel : ViewModelBase
    {
        public event Action<SeasonId> SeasonSelected;

        public ICommand SelectSeasonCommand { get; private set; }

        private readonly IDataStore dataStore;
        private readonly TVShow tvShow;
        public IEnumerable<MenuItemCommand> MenuItemCommands { get { return BuildMenuItemCommands(); } }

        public SeasonIconsListViewModel(IDataStore dataStore, TVShowId tvShowId)
        {
            this.dataStore = dataStore;
            this.tvShow = dataStore.TVShowRepository.Find(tvShowId);
            this.SelectSeasonCommand = new RelayCommand(param => OnSeasonSelected((param as MenuItemCommand).Tag as SeasonId));
        }

        private IEnumerable<MenuItemCommand> BuildMenuItemCommands()
        {
            return dataStore.SeasonRepository.Query(season => season.TVShowId.Equals(tvShow.Id))
                .Select(
                    season=>
                    new MenuItemCommand(
                        "Season " + season.Number,
                        MenuItemCommand.BuildImageFromResource("../Resources/tv.jpg"),
                        season.Id,
                        null,
                        null
                    ));
        }

        private void OnSeasonSelected(SeasonId seasonId)
        {
            var handler = SeasonSelected;
            if (handler != null)
                handler(seasonId);
        }
    }
}

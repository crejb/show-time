using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase currentViewModel = null;
        public ViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                if (value == currentViewModel)
                    return;

                currentViewModel = value;
                base.OnPropertyChanged("CurrentViewModel");
            }
        }

        public MainWindowViewModel(NavigatorViewModel navigator)
        {
            navigator.NavigateToViewRequested += new Action<ViewModelBase>(navigator_NavigateToViewRequested);
            CurrentViewModel = navigator.CurrentViewModel;
        }

        private void navigator_NavigateToViewRequested(ViewModelBase viewModel)
        {
            CurrentViewModel = viewModel;
        }
    }
}

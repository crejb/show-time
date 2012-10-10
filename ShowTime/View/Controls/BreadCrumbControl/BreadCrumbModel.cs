using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShowTime.View.Controls.BreadCrumbControl
{
    public class BreadCrumbModel : ViewModelBase
    {
        private ObservableCollection<BreadCrumbItem> breadCrumbItems;
        public ObservableCollection<BreadCrumbItem> BreadCrumbItems
        {
            get { return breadCrumbItems; }
            set
            {
                if (value == breadCrumbItems)
                    return;

                breadCrumbItems = value;
                base.OnPropertyChanged("BreadCrumbItems");
            }
        }
    }

    public class BreadCrumbItem
    {
        public ICommand MenuAction { get; set; }
        public string Text { get; set; }
        public BreadCrumbItem(string text, ICommand action)
        {
            MenuAction = action;
            Text = text;
        }
    }

    public class BreadCrumbHeadItem : BreadCrumbItem
    {
        public BreadCrumbHeadItem(string text, ICommand action)
            : base(text, action) { }
    }

    public class BreadCrumbTailItem : BreadCrumbItem
    {
        public BreadCrumbTailItem(string text, ICommand action)
            : base(text, action) { }
    }
}

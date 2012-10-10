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
using System.Globalization;

namespace ShowTime.View.Controls.BreadCrumbControl
{
    /// <summary>
    /// Interaction logic for BreadCrumbItemControl.xaml
    /// </summary>
    public partial class BreadCrumbItemControl : UserControl
    {
        public BreadCrumbItemControl()
        {
            InitializeComponent();
        }

        public bool HeadItem
        {
            get { return (bool)GetValue(HeadItemProperty); }
            set { SetValue(HeadItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeadItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadItemProperty =
            DependencyProperty.Register("HeadItem", typeof(bool), typeof(BreadCrumbItemControl), new UIPropertyMetadata(false, new PropertyChangedCallback(ValueChanged)));


        public bool TailItem
        {
            get { return (bool)GetValue(TailItemProperty); }
            set { SetValue(TailItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeadItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TailItemProperty =
            DependencyProperty.Register("TailItem", typeof(bool), typeof(BreadCrumbItemControl), new UIPropertyMetadata(false, new PropertyChangedCallback(ValueChanged)));

        private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

        }
    }

    public class HeadItemBoolToPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            bool isHeadItem = (bool)value;
            if (isHeadItem)
            {
                return new Point(0, 0.5);
            }
            else
            {
                return new Point(0.9, 0.5);
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

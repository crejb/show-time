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
using ShowTime.ViewModel.Commands;

namespace ShowTime.View
{
    /// <summary>
    /// Interaction logic for IconListPanel.xaml
    /// </summary>
    public partial class IconListPanel : UserControl
    {
        public IconListPanel()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconContentProperty =
             DependencyProperty.Register("IconContent", typeof(DataTemplate),
             typeof(IconListPanel), new FrameworkPropertyMetadata(null));

        public DataTemplate IconContent
        {
            get { return (DataTemplate)GetValue(IconContentProperty); }
            set { SetValue(IconContentProperty, value); }
        }

        public IEnumerable<MenuItemCommand> MenuItemCommands
        {
            get { return (IEnumerable<MenuItemCommand>)GetValue(MenuItemCommandsProperty); }
            set { SetValue(MenuItemCommandsProperty, value); }
        }

        public static readonly DependencyProperty MenuItemCommandsProperty =
            DependencyProperty.Register("MenuItemCommands", typeof(IEnumerable<MenuItemCommand>), typeof(IconListPanel), null);



        public ICommand ItemSelected
        {
            get { return (ICommand)GetValue(ItemSelectedProperty); }
            set { SetValue(ItemSelectedProperty, value); }
        }

        public static readonly DependencyProperty ItemSelectedProperty =
            DependencyProperty.Register("ItemSelected", typeof(ICommand), typeof(IconListPanel), null);



        public ICommand ItemConfirmed
        {
            get { return (ICommand)GetValue(ItemConfirmedProperty); }
            set { SetValue(ItemConfirmedProperty, value); }
        }

        public static readonly DependencyProperty ItemConfirmedProperty =
            DependencyProperty.Register("ItemConfirmed", typeof(ICommand), typeof(IconListPanel), null);



        public MenuItemCommand SelectedItem
        {
            get { return (MenuItemCommand)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(MenuItemCommand), typeof(IconListPanel), null);



        public MenuItemCommand ConfirmedItem
        {
            get { return (MenuItemCommand)GetValue(ConfirmedItemProperty); }
            set { SetValue(ConfirmedItemProperty, value); }
        }

        public static readonly DependencyProperty ConfirmedItemProperty =
            DependencyProperty.Register("ConfirmedItem", typeof(MenuItemCommand), typeof(IconListPanel), null);



        private void bdrItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var menuItem = ((sender as Button).Content as MenuItemCommand);
            ConfirmedItem = menuItem;
            InvokeCommand(ItemConfirmed, menuItem);
            InvokeCommand(menuItem.ConfirmedCommand, menuItem);
            e.Handled = true;
        }

        private void bdrItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = ((sender as Button).Content as MenuItemCommand);
            SelectedItem = menuItem;
            InvokeCommand(ItemSelected, menuItem);
            InvokeCommand(menuItem.SelectedCommand, menuItem);
        }

        private void InvokeCommand(ICommand command, object data)
        {
            if (command != null && command.CanExecute(data))
            {
                command.Execute(data);
            }
        }
    }
}

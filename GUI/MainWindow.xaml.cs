using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Backend;


namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EnterPoEID_GotFocus(object sender, RoutedEventArgs e)
        {
            EnterPoEID.Text = "";
        }

        private void EnterPoEID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EnterPoEID.Text.Length == 0)
            {
                EnterPoEID.Text = "Enter Poe ID";
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            TabItem item = new TabItem();
            item.Header = "new Header";
            item.Style = this.FindResource("TabsStyle") as Style;
            item.Content = "aldskfja;lskdfjl;askdjfl;asdjfl;kkasjdflkasjdf;lasdf";
            TabControlKey.Items.Add(item);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TableDataTestClass test1 = new TableDataTestClass() { Name = "Name1", Price = 1 };
            TableDataTestClass test2 = new TableDataTestClass() { Name = "Name2", Price = 2 };
            TableDataTestClass test3 = new TableDataTestClass() { Name = "Name3", Price = 3 };
            BindingList<TableDataTestClass> myData = new BindingList<TableDataTestClass>();
            myData.Add(test1);
            myData.Add(test2);
            myData.Add(test3);

            PathOfExileApiFunctions.GetItemsInAStashTabAsync("79b7ba7eb9c78313c61b64726904084b", "Hardcore", "GoStormUp", 0, ItemVariant.itemsWithNameAndPrice);
            dgDataTable.ItemsSource = myData;
        }
    }
}
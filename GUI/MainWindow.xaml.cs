using System;
using System.Collections.Generic;
using System.Linq;
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
            item.Style = this.FindResource("TabsStyle") as Style;
            item.Header = "new Header";
            item.Content = "aldskfja;lskdfjl;askdjfl;asdjfl;kkasjdflkasjdf;lasdf";
            TabControlKey.Items.Add(item);
        }

        
    }
}

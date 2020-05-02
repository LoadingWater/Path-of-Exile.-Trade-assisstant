using Backend.GUIFunctions;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GUIFunctions guiFunctions = new GUIFunctions();
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

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            guiFunctions.GoButtonFunction("79b7ba7eb9c78313c61b64726904084b", "Hardcore", "GoStormUp", MainTabControl, (Style)TryFindResource("DataGridStyle"));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
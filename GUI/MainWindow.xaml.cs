using Backend.GUIFunctions;
using System.Windows;
using Backend.Classes;
using System;



namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GUIFunctions guiFunctions = new GUIFunctions();
        CustomClient client;
        
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
            //97dfc9145fbfc40c9e19031c4e1b08ba
            if (EnterPoEID.Text.Length > 0 && EnterPoEID.Text.ToString() != "Enter PoE ID")
            {
                client = CustomClient.GetClient("POESESSID", EnterPoEID.Text.ToString(), new Uri("https://www.pathofexile.com/"));
                guiFunctions.GoButtonFunction(client, "Hardcore", "GoStormUp", MainTabControl, (Style)TryFindResource("DataGridStyle"));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
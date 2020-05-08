using Backend.GUIFunctions;
using System.Windows;
using Backend.Classes;
using System;
using System.Net;
using Backend.Models;



namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GUIFunctions guiFunctions = new GUIFunctions();
        CustomClient client = new CustomClient(new Cookie("POESESSID", "default"), new Uri("https://pathofexile.com/"));
        
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
                client.CookieContainer.Add(new Uri("https://pathofexile.com/"), new Cookie("POESESSID", EnterPoEID.Text.ToString()));
                guiFunctions.GoButtonFunction(client, new PlayerInfo("Hardcore", "GoStormUp"), MainTabControl, (Style)TryFindResource("DataGridStyle"));
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
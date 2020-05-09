using Backend.GUIFunctions;
using System.Windows;
using Backend.Classes;
using System;
using System.Net;
using Backend.Models;
using Backend.APIFunctions;
using Backend.ApplicationViewModel;
using Backend.Database;
using System.Diagnostics;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //GUIFunctions guiFunctions = new GUIFunctions();
        //CustomClient client = new CustomClient(new Cookie("POESESSID", "default"), new Uri("https://pathofexile.com/"));
        ApplicationViewModel applicationViewModel = new ApplicationViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = applicationViewModel;
        }

        #region EnterPoEID
        private void EnterPoEID_GotFocus(object sender, RoutedEventArgs e)
        {
            applicationViewModel.GuiData.Poesessid = "";
        }

        private void EnterPoEID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (applicationViewModel.GuiData.Poesessid.Length == 0)
            {
                applicationViewModel.GuiData.Poesessid = "Enter sessid";
            }
        }
        #endregion

        #region GoButton
        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //97dfc9145fbfc40c9e19031c4e1b08ba
                applicationViewModel.CustomClient.CookieContainer.Add(new Uri("https://pathofexile.com/"), new Cookie("POESESSID", applicationViewModel.GuiData.Poesessid));
                var responses = await PathOfExileApiFunctions.GetItemsInAllStashTabsAsStringAsync(applicationViewModel.CustomClient, new PlayerInfo("Hardcore", "GoStormUp"));
                var models = ResponseToModelConverter.ConvertAllResponses(responses);
                applicationViewModel.DatabaseFunctions.UpdateDatabase(models, applicationViewModel.DatabaseContext);
                //Create gui
            }
            catch (Exception x)
            {
                switch (x.HResult)
                {
                    case -2146233088: MessageBox.Show("Wrong Poesessid"); break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
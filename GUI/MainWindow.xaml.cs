using System.Windows;
using Backend.Classes;
using System;
using System.Net;
using Backend.Models;
using Backend.APIFunctions;
using Backend.ApplicationViewModel;
using Backend.Database;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading.Tasks;
using System.IO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationViewModel applicationViewModel = new ApplicationViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = applicationViewModel;
        }

        #region EnterPoEID textbox
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

        #region GetItems button
        private async void Get_items_Click(object sender, RoutedEventArgs e)
        {

            if (applicationViewModel.GuiData.Cooldown.ElapsedMilliseconds == 0 || applicationViewModel.GuiData.Cooldown.ElapsedMilliseconds > 60_000)
            {
                try
                {
                    //Start timer. 40 requests per minute. 60 sec cooldown
                    applicationViewModel.GuiData.Cooldown.Restart();
                    //Init viewmodel
                    applicationViewModel.CustomClient.CookieContainer.Add(new Uri("https://pathofexile.com/"), new Cookie("POESESSID", applicationViewModel.GuiData.Poesessid));
                    //Get responses and convert them
                    var responses = await PathOfExileApiFunctions.GetItemsInAllStashTabsAsStringAsync(applicationViewModel.CustomClient, new PlayerInfo(((LeagueModel)Leagues.SelectedItem).Id, "GoStormUp"));
                    var models = ResponseToModelConverter.ConvertAllResponses(responses);
                    //Update database
                    applicationViewModel.DatabaseFunctions.UpdateDatabase(models, applicationViewModel.DatabaseContext);
                    //create datagrid
                    applicationViewModel.GuiFunctions.CreateDataGrid(applicationViewModel.DatabaseContext, MainTabControl, applicationViewModel.GuiData.Leagues[Leagues.SelectedIndex].Id);
                    //97dfc9145fbfc40c9e19031c4e1b08ba
                }
                catch (Exception x)
                {
                    switch (x.HResult)
                    {
                        case -2146233088: MessageBox.Show("Wrong Poesessid"); applicationViewModel.GuiData.Cooldown.Reset(); break;
                        default: MessageBox.Show(x.Message); break;
                    }
                }
            }
            else
            {
                MessageBox.Show($"Api cooldown: {60 - applicationViewModel.GuiData.Cooldown.ElapsedMilliseconds * 0.001}"); 
            }
        }
        #endregion

        #region UpdateUiDatagrid
        private void Update_UI_Click(object sender, RoutedEventArgs e)
        {
            applicationViewModel.GuiFunctions.CreateDataGrid(applicationViewModel.DatabaseContext, MainTabControl, applicationViewModel.GuiData.Leagues[Leagues.SelectedIndex].Id);
        }
        #endregion

        //#region Move settings popup when window location changed
        //private void Window_LocationChanged(object sender, EventArgs e)
        //{
        //    //NOTE: Why does it work with any offset change?
        //    var originalOffset = Pop.HorizontalOffset;
        //    Pop.HorizontalOffset += Pop.HorizontalOffset + 1;
        //    Pop.HorizontalOffset = originalOffset;
        //}
        //#endregion

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string path = "userData.txt";
            if (File.Exists(path))
            {
                var fileData = File.ReadAllLines(path);
                if (fileData.Length != 0)
                {
                    //NOTE: both in viewmodel or both in controls?
                    applicationViewModel.GuiData.Poesessid = fileData[0];
                    Leagues.SelectedIndex =  int.Parse(fileData[1]);
                }
            }
            //Get list of leagues in leagues combobox
            applicationViewModel.GuiData.Leagues = await PathOfExileApiFunctions.GetLeaguesListAsync(applicationViewModel.CustomClient);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string path = "userData.txt";
            string[] data = { applicationViewModel.GuiData.Poesessid, Leagues.SelectedIndex.ToString() };
            File.WriteAllLines(path, data);
        }

        private void SearchItems_GotFocus(object sender, RoutedEventArgs e)
        {
            applicationViewModel.GuiData.ItemsToSearch = "";
        }

        private void SearchItems_LostFocus(object sender, RoutedEventArgs e)
        {
            if (applicationViewModel.GuiData.ItemsToSearch.Length == 0)
            {
                applicationViewModel.GuiData.ItemsToSearch = "Search items";
            }
        }

        private void SearchItems_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MainTabControl.Items.Count != 0 && applicationViewModel.GuiData.ItemsToSearch.Length != 0)
            {
                int selectedTab = MainTabControl.SelectedIndex;
                TabItem backupTab = (TabItem)MainTabControl.Items.GetItemAt(selectedTab);
                DataGrid tabItems = (DataGrid)backupTab.Content;
                foreach (var item in tabItems.Items)
                {
                    string itemName = ((DataGridItemModel)item).ItemName;

                    
                    if (itemName.Contains(applicationViewModel.GuiData.ItemsToSearch) || itemName.ToLower().Contains(applicationViewModel.GuiData.ItemsToSearch))
                    {
                        Debug.WriteLine("----------------------");
                        Debug.WriteLine(itemName);
                    }
                }
            }
        }
    }
}
﻿using System.Windows;
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

        #region GetItems
        private async void Get_items_Click(object sender, RoutedEventArgs e)
        {

            if (applicationViewModel.GuiData.Cooldown.ElapsedMilliseconds == 0 || applicationViewModel.GuiData.Cooldown.ElapsedMilliseconds > 60_000)
            {
                try
                {
                    applicationViewModel.GuiData.Cooldown.Restart();

                    applicationViewModel.CustomClient.CookieContainer.Add(new Uri("https://pathofexile.com/"), new Cookie("POESESSID", applicationViewModel.GuiData.Poesessid));
                    var responses = await PathOfExileApiFunctions.GetItemsInAllStashTabsAsStringAsync(applicationViewModel.CustomClient, new PlayerInfo("Hardcore", "GoStormUp"));
                    var models = ResponseToModelConverter.ConvertAllResponses(responses);
                    applicationViewModel.DatabaseFunctions.UpdateDatabase(models, applicationViewModel.DatabaseContext);
                    MainTabControl.Items.Clear();
                    applicationViewModel.GuiFunctions.CreateDataGrid(applicationViewModel.DatabaseContext, MainTabControl);
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

        private void Update_UI_Click(object sender, RoutedEventArgs e)
        {
            applicationViewModel.GuiFunctions.CreateDataGrid(applicationViewModel.DatabaseContext, MainTabControl);
        }
    }
}
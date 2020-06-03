using Backend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Backend.Classes;



namespace Backend.APIFunctions
{
    public static class PathOfExileApiFunctions
    {
        public static async Task<string> GetItemsInOneTabAsStringAsync(CustomClient client, PlayerInfo playerInfo, int tabNumber)
        {
            try
            {
                //NOTE:Can we reuse Uri and not create a new one every time?
                Uri address = new Uri(client.HttpClient.BaseAddress, $"character-window/get-stash-items?league={playerInfo.League}&tabs=1&tabIndex={tabNumber}&accountName={playerInfo.AccountName}");
                string response = await client.HttpClient.GetStringAsync(address);
                return response;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        public static async Task<List<Task<string>>> GetItemsInAllStashTabsAsStringAsync(CustomClient client, PlayerInfo playerInfo)
        {
            //TODO: restric request rate here? or in gui creation/ mb here
            int numberOfStashTabs = await GetNumberOfStashTabsAsync(client, playerInfo);
            int maxRequests = numberOfStashTabs <= 40 ? numberOfStashTabs : 40;
            List<Task<string>> tabs = new List<Task<string>>();

            for (int i = 0; i < maxRequests; i++)
            {
                int tmp = i;
                tabs.Add(Task.Run(() => GetItemsInOneTabAsStringAsync(client, playerInfo, tmp)));
            }
            await Task.WhenAll(tabs);
            return tabs;
        }
        public static async Task<int> GetNumberOfStashTabsAsync(CustomClient client, PlayerInfo playerInfo)
        {
            Uri address = new Uri(client.HttpClient.BaseAddress, $"character-window/get-stash-items?league={playerInfo.League}&tabs=1&tabIndex=0&accountName={playerInfo.AccountName}");
            string response = await client.HttpClient.GetStringAsync(address);

            ItemModel.RootObject stashModel = new ItemModel.RootObject();
            stashModel = JsonConvert.DeserializeObject<ItemModel.RootObject>(response);

            return stashModel.numTabs;
        }
        public static async Task<List<LeagueModel>> GetLeaguesListAsync(CustomClient client)
        {
            try
            {
                Uri address = new Uri(client.HttpClient.BaseAddress, $"api/leagues");
                string response = await client.HttpClient.GetStringAsync(address);
                //LATER: create converter?
                var result = JsonConvert.DeserializeObject<List<LeagueModel>>(response);
                return result;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}

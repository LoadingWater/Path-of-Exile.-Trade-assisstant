using Backend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Backend.Classes;


namespace Backend.APIFunctions
{
    using ItemsInOneTab = Task<List<TrimmedItemModel>>;
    using EveryTabWithItems = List<Task<List<TrimmedItemModel>>>;
    public static class PathOfExileApiFunctions
    {
        public static async Task<List<TrimmedItemModel>> GetItemsInAStashTabAsync(CustomClient client, PlayerInfo playerInfo, int tabNumber)
        {
            try
            {
                Uri address = new Uri(client.HttpClient.BaseAddress, $"character-window/get-stash-items?league={playerInfo.League}&tabs=1&tabIndex={tabNumber}&accountName={playerInfo.AccountName}");
                string response = await client.HttpClient.GetStringAsync(address);
                //convert response to object
                ItemModel.RootObject stashModel = new ItemModel.RootObject();
                stashModel = JsonConvert.DeserializeObject<ItemModel.RootObject>(response);
                //list to return
                List<TrimmedItemModel> listOfItems = new List<TrimmedItemModel>();

                for (int i = 0; i < stashModel.items.Count; i++)
                {
                    //local variables
                    string itemName     = stashModel.items[i].name;
                    string itemPrice    = stashModel.items[i].note;
                    string tabName      = stashModel.tabs[tabNumber].n;
                    string itemId       = stashModel.items[i].id;
                    string itemDescText = stashModel.items[i].descrText;

                    if (itemName.Length != 0)
                    {
                        if (itemPrice != null)
                        {
                            listOfItems.Add(new TrimmedItemModel() { Id = itemId, Price = itemPrice, TabName = tabName, Name = itemName, DescrText = itemDescText });
                        }
                        else if (tabName.Contains("~b/o") || tabName.Contains("~price"))
                        {
                            listOfItems.Add(new TrimmedItemModel() { Id = itemId, Price = tabName, TabName = tabName, Name = itemName, DescrText = itemDescText });
                        }
                    }
                }
                return listOfItems;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        //Task<List<TrimmedModel>> one tab
        //List<Task<List<TrimmedModel>>> all tabs
        //Task<List<Task<List<TrimmedModel>>>> all tabs with return type Task
        public static async Task<List<ItemsInOneTab>> GetItemsInAllStashTabsAsync(CustomClient client, PlayerInfo playerInfo)
        {
            //TODO: restric request rate here? or in gui creation/ mb here
            int numberOfStashTabs = await GetNumberOfStashTabsAsync(client, playerInfo);
            EveryTabWithItems tabs = new EveryTabWithItems();
            int maxRequests = numberOfStashTabs <= 40 ? numberOfStashTabs : 40;

            for (int i = 0; i < maxRequests; i++)
            {
                int tmp = i;
                tabs.Add(Task.Run(() => GetItemsInAStashTabAsync(client, playerInfo, tmp)));
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

        #region Private functions
        #endregion
    }
}

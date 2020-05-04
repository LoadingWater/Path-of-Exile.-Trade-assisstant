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
        public static async Task<List<TrimmedItemModel>> GetItemsInAStashTabAsync(CustomClient client, string league, string accountName, int tabNumber, ItemVariant itemVariant)
        {
            try
            {
                if (client.HttpClient.BaseAddress == null)
                {
                    client.HttpClient.BaseAddress = new Uri("https://www.pathofexile.com/");
                }

                Uri address = new Uri(client.HttpClient.BaseAddress, $"character-window/get-stash-items?league={league}&tabs=1&tabIndex={tabNumber}&accountName={accountName}");
                string response = await client.HttpClient.GetStringAsync(address);

                //convert response to object
                ItemModel.RootObject stashModel = new ItemModel.RootObject();
                stashModel = JsonConvert.DeserializeObject<ItemModel.RootObject>(response);
                //list to return
                List<TrimmedItemModel> listOfItems = new List<TrimmedItemModel>();

                for (int i = 0; i < stashModel.items.Count; i++)
                {
                    //local variables
                    string itemName = stashModel.items[i].name;
                    string itemPrice = stashModel.items[i].note;
                    string tabName = stashModel.tabs[tabNumber].n;

                    if (itemName.Length != 0)
                    {
                        if (itemPrice != null)
                        {
                            listOfItems.Add(new TrimmedItemModel() { Price = itemPrice, TabName = tabName, Name = itemName });
                        }
                        else if (tabName.Contains("~b/o") || tabName.Contains("~price"))
                        {
                            listOfItems.Add(new TrimmedItemModel() { Price = tabName, TabName = tabName, Name = itemName });
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
        public static async Task<int> GetNumberOfStashTabs(CustomClient client, string league, string accountName)
        {
            if (client.HttpClient.BaseAddress == null)
            {
                client.HttpClient.BaseAddress = new Uri("https://www.pathofexile.com/");
            }
            Uri address = new Uri(client.HttpClient.BaseAddress, $"character-window/get-stash-items?league={league}&tabs=1&tabIndex=0&accountName={accountName}");
            string response = await client.HttpClient.GetStringAsync(address);
            ItemModel.RootObject stashModel = new ItemModel.RootObject();
            stashModel = JsonConvert.DeserializeObject<ItemModel.RootObject>(response);
            return stashModel.numTabs;
        }

        #region Private functions

        #endregion
    }
}

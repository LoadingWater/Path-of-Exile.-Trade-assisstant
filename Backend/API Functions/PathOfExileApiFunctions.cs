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
        public static async Task<List<TrimmedItemModel>> GetItemsInAStashTabAsync(string poesessid, string league, string accountName, int tabNumber, ItemVariant itemVariant)
        {
            try
            {
                //setup httpclient and make request
                if (CustomClient.HttpClient == null)
                {
                    CustomClient.CreateClient(new Uri("https://www.pathofexile.com/"), poesessid);
                }
                Uri address = new Uri(CustomClient.HttpClient.BaseAddress, $"character-window/get-stash-items?league={league}&tabs=1&tabIndex={tabNumber}&accountName={accountName}");
                string response = await CustomClient.HttpClient.GetStringAsync(address);

                //convert response to object
                ItemModel.RootObject stashModel = new ItemModel.RootObject();
                stashModel = JsonConvert.DeserializeObject<ItemModel.RootObject>(response);

                //list to return
                List<TrimmedItemModel> l = new List<TrimmedItemModel>();
                for (int i = 0; i < stashModel.items.Count; i++)
                {
                    //local variables
                    string itemName = stashModel.items[i].name;
                    string itemPrice = stashModel.items[i].note;
                    string tabName = stashModel.tabs[tabNumber].n;

                    //select variant
                    if (itemVariant == ItemVariant.allItems)
                    {
                        l.Add(new TrimmedItemModel() { Price = itemPrice, TabName = tabName, Name = itemName });
                    }
                    else if (itemVariant == ItemVariant.itemsWithName)
                    {
                        if (itemName.Length != 0)
                        {
                            l.Add(new TrimmedItemModel() { Price = itemPrice, TabName = tabName, Name = itemName });
                        }
                    }
                    else if (itemVariant == ItemVariant.itemsWithNameAndPrice)
                    {
                        if (itemName.Length != 0)
                        {
                            if (itemPrice != null)
                            {
                                l.Add(new TrimmedItemModel() { Price = itemPrice, TabName = tabName, Name = itemName });
                            }
                            else if (tabName.Contains("~b/o") || tabName.Contains("~price"))
                            {
                                l.Add(new TrimmedItemModel() { Price = tabName, TabName = tabName, Name = itemName });
                            }
                        }
                    }
                }
                return l;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        public static async Task<int> GetStashTabNumbersAsync(string poesessid, string league, string accountName)
        {
            //setup httpclient and make request
            if (CustomClient.HttpClient == null)
            {
                CustomClient.CreateClient(new Uri("https://www.pathofexile.com/"), poesessid);
            }
            Uri address = new Uri(CustomClient.HttpClient.BaseAddress, $"character-window/get-stash-items?league={league}&tabs=1&tabIndex=0&accountName={accountName}");
            string response = await CustomClient.HttpClient.GetStringAsync(address);
            ItemModel.RootObject stashModel = new ItemModel.RootObject();
            stashModel = JsonConvert.DeserializeObject<ItemModel.RootObject>(response);
            return stashModel.numTabs;
        }
    }
}

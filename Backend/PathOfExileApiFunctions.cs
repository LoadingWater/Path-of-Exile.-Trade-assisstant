using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Backend
{
    public static class PathOfExileApiFunctions
    {
        private static void MainInitialization(Uri address, string poessid)
        {
            if (CustomClient.HttpClient == null)
            {
                CustomClient.InitClient();
                CustomClient.CookieContainer.Add(address, new Cookie("POESESSID", poessid));
                CustomClient.HttpClient.BaseAddress = address;
            }
        }
        public static async Task<List<ItemModel.RootObject>> GetItemsInAStashTabAsync(string poessid, string league, string accountName, int stashNumber, ItemVariant itemVariant)
        {
            ItemModel.RootObject stashModel = new ItemModel.RootObject();
            Uri address = new Uri($"https://www.pathofexile.com/character-window/get-stash-items?league={league}&tabs=1&tabIndex={stashNumber}&accountName={accountName}");
            try
            {
                MainInitialization(address, poessid);
                string response = await CustomClient.HttpClient.GetStringAsync(address);
                stashModel = JsonConvert.DeserializeObject<ItemModel.RootObject>(response);

                List<ItemModel.RootObject> l = new List<ItemModel.RootObject>();
                for (int i = 0; i < stashModel.items.Count; i++)
                {
                    if (itemVariant == ItemVariant.allItems)
                    {
                        l.Add(stashModel);
                    }
                    else if (itemVariant == ItemVariant.itemsWithName)
                    {
                        if (stashModel.items[i].name.Length != 0)
                        {
                            l.Add(stashModel);
                        }
                    }
                    else if (itemVariant == ItemVariant.itemsWithNameAndPrice)
                    {
                        if (stashModel.items[i].name.Length != 0)
                        {
                            if (stashModel.items[i].note != null || stashModel.tabs[stashNumber].n  != null)
                            {
                                l.Add(stashModel);
                                Console.WriteLine($"{stashModel.items[i].name} Price: {stashModel.items[i].note} Note: {stashModel.tabs[stashNumber].n}");
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
    }
}

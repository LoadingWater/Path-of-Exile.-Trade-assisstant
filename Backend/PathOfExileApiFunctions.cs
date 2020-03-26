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
    static public class PathOfExileApiFunctions
    {
        static private HttpClient httpClient;
        static private HttpClientHandler httpClientHandler;
        static private CookieContainer cookieContainer;
        public static void InitializeClientAndHandler()
        {
            try
            {
                cookieContainer = new CookieContainer();
                httpClientHandler = new HttpClientHandler();
                httpClientHandler.CookieContainer = cookieContainer;
                httpClient = new HttpClient(httpClientHandler);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Returns all item in a given tab
        /// </summary>
        /// <param name="poessid"></param>
        /// <param name="league"></param>
        /// <param name="accountName"></param>
        /// <param name="stashNumber"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static async void GetItemsInAStashTabAsync(string poessid, string league, string accountName, int stashNumber)
        {
            try
            {
                StashModel.RootObject stashModel = new StashModel.RootObject();

                Uri address = new Uri($"https://www.pathofexile.com/character-window/get-stash-items?league={league}&tabs=1&tabIndex={stashNumber}&accountName={accountName}");
                cookieContainer.Add(address, new Cookie("POESESSID", poessid));
                httpClient.BaseAddress = address;
                string response = await httpClient.GetStringAsync(address);
                stashModel = JsonConvert.DeserializeObject<StashModel.RootObject>(response);
                List<string> l = new List<string>();
                foreach (var item in stashModel.items)
                {
                    if (item.name != "")
                    {
                        Console.WriteLine(item.name);
                        l.Add(item.name);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}

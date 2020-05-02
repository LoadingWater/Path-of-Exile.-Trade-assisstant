using System;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace Backend.Classes
{
    public static class CustomClient
    {
        public static HttpClient HttpClient { get; set; }
        public static HttpClientHandler HttpClientHandler { get; set; }
        public static CookieContainer CookieContainer { get; set; }
        public static void CreateClient(Uri baseAddress, string poesessid)
        {
            try
            {
                CookieContainer = new CookieContainer();
                CookieContainer.Add(baseAddress, new Cookie("POESESSID", poesessid));
                HttpClientHandler = new HttpClientHandler();
                HttpClientHandler.CookieContainer = CookieContainer;
                HttpClient = new HttpClient(HttpClientHandler);
                HttpClient.BaseAddress = baseAddress;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}

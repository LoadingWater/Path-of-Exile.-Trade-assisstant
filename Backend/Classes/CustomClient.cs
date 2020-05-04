using System;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace Backend.Classes
{
    public class CustomClient
    {
        private static CustomClient _customClient;
        private CustomClient()
        {
            CookieContainer = new CookieContainer();
            HttpClientHandler = new HttpClientHandler() { CookieContainer = CookieContainer };
            HttpClient = new HttpClient(HttpClientHandler);
        }
        public HttpClient HttpClient { get; private set; }
        private HttpClientHandler HttpClientHandler { get; set; }
        private CookieContainer CookieContainer { get; set; }

        public static CustomClient GetClient(string cookieName = null, string cookieValue = null, Uri addressForCookies = null)//TODO: mb check na обязательную поставка аргументов?
        {
            if (_customClient == null)
            {
                _customClient = new CustomClient();
            }
            if (cookieName != null && cookieValue != null && addressForCookies != null)
            {
                _customClient.CookieContainer.Add(addressForCookies, new Cookie(cookieName, cookieValue));                
            }
            return _customClient;
        }
        public void SetCookies(Uri address, string cookieName, string value)
        {
            CookieContainer.Add(address, new Cookie(cookieName, value));
        }
    }
}

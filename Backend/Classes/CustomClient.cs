using System;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace Backend.Classes
{
    public class CustomClient
    {
        public CustomClient(Cookie cookie, Uri clientBaseAddress)
        {
            CookieContainer = new CookieContainer();
            CookieContainer.Add(clientBaseAddress, cookie);
            HttpClientHandler = new HttpClientHandler() { CookieContainer = CookieContainer };
            HttpClient = new HttpClient(HttpClientHandler);
            HttpClient.BaseAddress = clientBaseAddress;
        }
        public HttpClient HttpClient { get; private set; }
        public HttpClientHandler HttpClientHandler { get; private set; }
        public CookieContainer CookieContainer { get; private set; }
    }
}

﻿using System;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace Backend
{
    public static class CustomClient
    { 
        public static HttpClient HttpClient{ get; set; }
        public static HttpClientHandler HttpClientHandler { get; set; }
        public static CookieContainer CookieContainer { get; set; }
        public static void InitClient()
        {
            try
            {
                CookieContainer = new CookieContainer();
                HttpClientHandler = new HttpClientHandler();
                HttpClientHandler.CookieContainer = CookieContainer;
                HttpClient = new HttpClient(HttpClientHandler);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}

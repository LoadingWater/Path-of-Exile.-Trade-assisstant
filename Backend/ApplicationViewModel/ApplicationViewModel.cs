﻿using System;
using Backend.Classes;
using System.Net;
using Backend.Database;
using System.Data.Entity;
using Backend.APIFunctions;


namespace Backend.ApplicationViewModel
{
    public class ApplicationViewModel
    {
        private GuiData           _guiData;
        private CustomClient      _customClient;
        private DatabaseContext   _databaseContext;
        private DatabaseFunctions _databaseFunctions;
        private GuiFunctions      _guIFunctions;
            
        #region Commands declaration
        #endregion

        public ApplicationViewModel()
        {
            _guiData = new GuiData();
            //TODO: parameterless init and init when button GO clicked?
            _customClient = new CustomClient(new Cookie("POESESSID", "default"), new Uri("https://pathofexile.com/"));
            _databaseContext = new DatabaseContext();
            //TODO: async load
            _databaseContext.Tabs.Load();
            _databaseContext.Items.Load();
            _databaseFunctions = new DatabaseFunctions();
            _guIFunctions = new GuiFunctions();
        }
        public GuiData           GuiData           { get { return _guiData; } }
        public CustomClient      CustomClient      { get { return _customClient; } }
        public DatabaseContext   DatabaseContext   { get { return _databaseContext; } set { _databaseContext = value; } }
        public DatabaseFunctions DatabaseFunctions { get { return _databaseFunctions; } }
        public GuiFunctions      GuiFunctions      { get { return _guIFunctions; } }
       
        #region Commands implementation
        #endregion

    }
}
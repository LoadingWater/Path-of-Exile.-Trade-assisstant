using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Classes;
using Backend.APIFunctions;
using Backend.Models;
using System.Net;
using System.Windows;
using Backend.Models;
using Backend.Database;
using System.Data.Entity;

namespace Backend.ApplicationViewModel
{
    public class ApplicationViewModel
    {
        private GuiData _guiData;
        private CustomClient _customClient;
        private DatabaseContext _databaseContext;
        private DatabaseFunctions _databaseFunctions;

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
        }
        public GuiData           GuiData           { get { return _guiData; } }
        public CustomClient      CustomClient      { get { return _customClient; } }
        public DatabaseContext   DatabaseContext   { get { return _databaseContext; } set { _databaseContext = value; } }
        public DatabaseFunctions DatabaseFunctions { get { return _databaseFunctions; } }
        #region Commands implementation
        #endregion

    }
}
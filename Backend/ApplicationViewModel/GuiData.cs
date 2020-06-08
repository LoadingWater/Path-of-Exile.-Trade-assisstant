using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Backend.Models;



namespace Backend.ApplicationViewModel
{
    public class GuiData : INotifyPropertyChanged
    {
        private string _poesessid = "Enter sessid";
        private Stopwatch _cooldown;
        private bool _isPopOpen = false;
        private List<LeagueModel> _leagues;
        private bool _isGetItemsAvailable = false;

        public string Poesessid
        {
            get { return _poesessid; }
            set
            {
                _poesessid = value;
                OnPropertyChanged();
            }
        }
        public Stopwatch Cooldown
        {
            get
            {
                if (_cooldown == null)
                {
                    _cooldown = new Stopwatch();
                    OnPropertyChanged();
                }
                return _cooldown;
            }
            set { _cooldown = value; }
        }
        public bool IsPopOpen
        { 
            get 
            { 
                return _isPopOpen; 
            } 
            set 
            { 
                _isPopOpen = value; 
                OnPropertyChanged(); 
            } 
        }
        public List<LeagueModel> Leagues
        {
            get { return _leagues; }
            set 
            { 
                _leagues = value;
                IsGetItemsAvailable = true;
                OnPropertyChanged();
            }
        }
        public bool IsGetItemsAvailable
        {
            get
            {
                return _isGetItemsAvailable;
            }
            set
            {
                _isGetItemsAvailable = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

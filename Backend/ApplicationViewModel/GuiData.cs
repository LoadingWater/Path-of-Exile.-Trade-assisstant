using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace Backend.ApplicationViewModel
{
    public class GuiData : INotifyPropertyChanged
    {
        private string _poesessid;
        public string Poesessid
        {
            get { return _poesessid; }
            set
            {
                _poesessid = value;
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

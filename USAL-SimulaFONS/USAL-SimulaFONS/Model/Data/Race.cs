using System.Collections.ObjectModel;
using System.ComponentModel;

namespace USAL_SimulaFONS { 
    class Race : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int lapsValue;
        private ObservableCollection<RaceState> lapStatesList;

        public Race()
        {
            lapStatesList = new ObservableCollection<RaceState>();
        }
        public int Laps
        {
            get { return lapsValue; }
            set { lapsValue = value; OnPropertyChanged("Laps"); }
        }

        public ObservableCollection<RaceState> LapStates
        {
            get { return lapStatesList; }
            set { lapStatesList = value; OnPropertyChanged("LapStates"); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

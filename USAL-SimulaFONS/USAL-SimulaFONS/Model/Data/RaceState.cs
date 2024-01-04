using System.ComponentModel;

namespace USAL_SimulaFONS
{
    class RaceState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    
        private int lapsStartValue;
        private int lapsEndValue;
        private int incidentTypeValue;

        public int LapStart
        {
            get { return lapsStartValue; }
            set { lapsStartValue = value; OnPropertyChanged("LapStart"); }
        }
        public int LapEnd
        {
            get { return lapsEndValue; }
            set { lapsEndValue = value; OnPropertyChanged("LapEnd"); }
        }

        public int IncidentType
        {
            get { return incidentTypeValue; }
            set { incidentTypeValue = value; OnPropertyChanged("IncidentType"); }
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

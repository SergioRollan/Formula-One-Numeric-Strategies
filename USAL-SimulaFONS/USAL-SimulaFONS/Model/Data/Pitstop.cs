using System.ComponentModel;

namespace USAL_SimulaFONS
{
    class Pitstop : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int stopLapValue = -1;
        private Tyre newTyreValue;

        public Pitstop()
        {
            newTyreValue = new Tyre();
        }

        public int StopLap
        {
            get { return stopLapValue; }
            set { stopLapValue = value; OnPropertyChanged("StopLap"); }
        }

        public Tyre NewTyre
        {
            get { return newTyreValue; }
            set { newTyreValue = value; OnPropertyChanged("NewTyre"); }
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;


namespace USAL_SimulaFONS
{
    class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Driver> driversValue;
        private Track trackDataValue;
        private Race raceDataValue;



        public ObservableCollection<Driver> Drivers
        {
            get { return driversValue; }
            set { driversValue = value; OnPropertyChanged("Drivers"); }
        }

        public Track TrackData
        {
            get { return trackDataValue; }
            set { trackDataValue = value; OnPropertyChanged("TrackData"); }
        }

        public Race RaceData
        {
            get { return raceDataValue; }
            set { raceDataValue = value; OnPropertyChanged("RaceData"); }
        }

        public string DriversString()
        {
            string retur = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("-1\t");
                int a;
                if (driversValue.Count == 0) a = retur.Length;
                foreach (Driver driverTemp in driversValue) sb.Append(driverTemp.ToStringCustom());
                retur = sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
            return retur;
        }

        public Model()
        {
            Drivers = new ObservableCollection<Driver> ();
            TrackData = new Track();
            RaceData = new Race();
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

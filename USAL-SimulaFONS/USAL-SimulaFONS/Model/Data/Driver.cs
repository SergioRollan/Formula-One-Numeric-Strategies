using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace USAL_SimulaFONS
{
    class Driver : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /* Variables */
        private int dorsalValue;
        private int startPosValue;
        private string nameValue;
        private string surnameValue;
        private double qTimeValue;
        private double abilityDiffValue;
        private ObservableCollection<Tyre> tyresList;
        private ObservableCollection<Pitstop> strategyList;

        public Driver(int dorsal, string name, string surname, double qTime, int startPos, double abilityDiff)
        {
            Dorsal = dorsal;
            Name = name;
            Surname = surname;
            QTime = qTime;
            StartPos = startPos;
            AbilityDiff = abilityDiff;
            Tyres = new ObservableCollection<Tyre>();
            Strategy = new ObservableCollection<Pitstop>();

            for(int i = 0; i < 10; i++)
            {
                Strategy.Add(new Pitstop());
            }
        }


        public string ToStringCustom()
        {
            string retur = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                int a;
                if (tyresList.Count == 0) a = retur.Length;
                sb.Append(string.Format("{0}\t{1}", tyresList[0].LapsNum, tyresList[0].TyreType));
                foreach (Tyre ty in tyresList) if (tyresList.IndexOf(ty) != 0) sb.Append(ty.ToStringCustom());
                retur = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", dorsalValue, startPosValue, nameValue, surnameValue, qTimeValue, abilityDiffValue, sb);
            }
            catch (Exception)
            {
                MessageBox.Show("Error parseando drivers", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception();
            }
            return retur;
        }

        /// <summary>
        /// Dorsal of the driver. Serves as an unique ID
        /// </summary>
        public int Dorsal
        {
            get { return dorsalValue; }
            set { dorsalValue = value; OnPropertyChanged("Dorsal"); }
        }

        /// <summary>
        /// Starting position of the driver
        /// </summary>
        public int StartPos
        {
            get { return startPosValue; }
            set { startPosValue = value; OnPropertyChanged("StartPos"); }
        }

        /// <summary>
        /// Driver's name
        /// </summary>
        public string Name
        {
            get { return nameValue; }
            set { nameValue = value; OnPropertyChanged("Name"); }
        }

        /// <summary>
        /// Driver's surname
        /// </summary>
        public string Surname
        {
            get { return surnameValue; }
            set { surnameValue = value; OnPropertyChanged("Surname"); }
        }

        /// <summary>
        /// Fastest lap set by the driver in qualifying
        /// </summary>
        public double QTime
        {
            get { return qTimeValue; }
            set { qTimeValue = value; OnPropertyChanged("QTime"); }
        }

        /// <summary>
        /// Time diff between fastest qualifying lap and driver's fastest lap
        /// </summary>
        public double AbilityDiff
        {
            get { return abilityDiffValue; }
            set { abilityDiffValue = value; OnPropertyChanged("AbilityDiff"); }
        }

        /// <summary>
        /// Tyre list
        /// </summary>
        public ObservableCollection<Tyre> Tyres
        {
            get { return tyresList; }
            set { tyresList = value; OnPropertyChanged("Tyres"); }
        }

        /// <summary>
        /// Strategy List
        /// </summary>
        public ObservableCollection<Pitstop> Strategy
        {
            get { return strategyList; }
            set { strategyList = value; OnPropertyChanged("Strategy"); }
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

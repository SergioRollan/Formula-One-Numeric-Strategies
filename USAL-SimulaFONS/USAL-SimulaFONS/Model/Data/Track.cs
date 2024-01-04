using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;


namespace USAL_SimulaFONS
{
    class Track : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int lapNumValue;

        private double racePaceDiffValue;
        private double gridPosTimeLossValue;
        private double standingStartTimeLossValue;

        private int startingFuelValue;
        private double fuelTimeLossValue;
        private double fuelMassLossValue;

        private double DRSTimeGainValue;
        private double overtakingDeltaDiffValue;
        private double overtakingTimeLossValue;
        private double overtakenTimeLossValue;

        private double pitstopLossGreenValue;
        private double pitstopLossSafetyCarValue;
        private double deltaVSCTimeValue;

        private double lapDegHardValue;
        private double lapDegMedValue;
        private double lapDegSoftValue;

        private double lapDiffSoftMedValue;
        private double lapDiffSoftHardValue;

        public string ToStringCustom()
        {
            string retur;
            try
            {
                retur = string.Format("-2\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\n", lapNumValue, startingFuelValue, racePaceDiffValue, gridPosTimeLossValue,
                    standingStartTimeLossValue, fuelTimeLossValue, fuelMassLossValue, DRSTimeGainValue, overtakingDeltaDiffValue, overtakingTimeLossValue, overtakenTimeLossValue,
                    pitstopLossGreenValue, pitstopLossSafetyCarValue, deltaVSCTimeValue, lapDegHardValue, lapDegMedValue, lapDegSoftValue, lapDiffSoftHardValue, lapDiffSoftMedValue);
            }
            catch (Exception)
            {
                MessageBox.Show("Error parseando track", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            return retur;
        }

        public int LapNum
        {
            get { return lapNumValue; }
            set { lapNumValue = value; OnPropertyChanged("LapNum"); }
        }


        public double RacePaceDiff
        {
            get { return racePaceDiffValue; }
            set { racePaceDiffValue = value; OnPropertyChanged("RacePaceDiff"); }
        }

        public double GridPosTimeLoss
        {
            get { return gridPosTimeLossValue; }
            set { gridPosTimeLossValue = value; OnPropertyChanged("GridPosTimeLoss"); }
        }

        public double StandingStartTimeLoss
        {
            get { return standingStartTimeLossValue; }
            set { standingStartTimeLossValue = value; OnPropertyChanged("StandingStartTimeLoss"); }
        }


        public int StartingFuel
        {
            get { return startingFuelValue; }
            set { startingFuelValue = value; OnPropertyChanged("StartingFuel"); }
        }

        public double FuelTimeLoss
        {
            get { return fuelTimeLossValue; }
            set { fuelTimeLossValue = value; OnPropertyChanged("FuelTimeLoss"); }
        }

        public double FuelMassLoss
        {
            get { return fuelMassLossValue; }
            set { fuelMassLossValue = value; OnPropertyChanged("FuelMassLoss"); }
        }


        public double DRSTimeGain
        {
            get { return DRSTimeGainValue; }
            set { DRSTimeGainValue = value; OnPropertyChanged("DRSTimeGain"); }
        }

        public double OvertakingDeltaDiff
        {
            get { return overtakingDeltaDiffValue; }
            set { overtakingDeltaDiffValue = value; OnPropertyChanged("OvertakingDeltaDiff"); }
        }

        public double OvertakingTimeLoss
        {
            get { return overtakingTimeLossValue; }
            set { overtakingTimeLossValue = value; OnPropertyChanged("OvertakingTimeLoss"); }
        }

        public double OvertakenTimeLoss
        {
            get { return overtakenTimeLossValue; }
            set { overtakenTimeLossValue = value; OnPropertyChanged("OvertakenTimeLoss"); }
        }

        public double PitstopLossGreen
        {
            get { return pitstopLossGreenValue; }
            set { pitstopLossGreenValue = value; OnPropertyChanged("PitstopLossGreen"); }
        }

        public double PitstopLossSafetyCar
        {
            get { return pitstopLossSafetyCarValue; }
            set { pitstopLossSafetyCarValue = value; OnPropertyChanged("PitstopLossSafetyCar"); }
        }

        public double DeltaVSCTime
        {
            get { return deltaVSCTimeValue; }
            set { deltaVSCTimeValue = value; OnPropertyChanged("DeltaVSCTime"); }
        }


        public double LapDegHard
        {
            get { return lapDegHardValue; }
            set { lapDegHardValue = value; OnPropertyChanged("LapDegHard"); }
        }

        public double LapDegMed
        {
            get { return lapDegMedValue; }
            set { lapDegMedValue = value; OnPropertyChanged("LapDegMed"); }
        }

        public double LapDegSoft
        {
            get { return lapDegSoftValue; }
            set { lapDegSoftValue = value; OnPropertyChanged("LapDegSoft"); }
        }


        public double LapDiffSoftMed
        {
            get { return lapDiffSoftMedValue; }
            set { lapDiffSoftMedValue = value; OnPropertyChanged("LapDiffSoftMed"); }
        }

        public double LapDiffSoftHard
        {
            get { return lapDiffSoftHardValue; }
            set { lapDiffSoftHardValue = value; OnPropertyChanged("LapDiffSoftHard"); }
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

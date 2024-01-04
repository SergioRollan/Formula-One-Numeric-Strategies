using System;
using System.ComponentModel;

namespace USAL_SimulaFONS
{
    class Tyre : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int lapsNumValue;
        private int tyreTypeValue;

        public string ToStringCustom()
        {
            try
            {

                return string.Format("\t{0}\t{1}", lapsNumValue, tyreTypeValue);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        public Tyre()
        {

        }

        public Tyre(int tyreType, int lapsNum)
        {
            TyreType = tyreType;
            LapsNum = lapsNum;
        }

        public int TyreType
        {
            get { return tyreTypeValue; }
            set { tyreTypeValue = value; OnPropertyChanged("TyreType"); }
        }

        public int LapsNum
        {
            get { return lapsNumValue; }
            set { lapsNumValue = value; OnPropertyChanged("LapsNum"); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            switch(TyreType)
            {
                case TyreCompound.HARD:
                    return "Hard (" + this.LapsNum + ")";

                case TyreCompound.MEDIUM:
                    return "Medium (" + this.LapsNum + ")";

                case TyreCompound.SOFT:
                    return "Soft (" + this.LapsNum + ")";

                default:
                    return "Unknown";
            }
        }

    }
}

using System.Text.RegularExpressions;
using System.Windows;

namespace USAL_SimulaFONS
{
    /// <summary>
    /// Lógica de interacción para AddTeamWindow.xaml
    /// </summary>
    public partial class AddTyreWindow : Window
    {
        public int TyreLaps { get { return int.Parse(tyreLapsTxt.Text); }}

        public int TyreType { get; set; }

        public AddTyreWindow()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            Regex regexInt = new Regex("^[0-9]+$");

            if (!regexInt.IsMatch(tyreLapsTxt.Text))
            {
                MessageBox.Show("Los campos numericos contienen texto", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            switch (tyreSelector.Text)
            {
                case "Soft":
                    TyreType = TyreCompound.SOFT;
                    break;

                case "Medium":
                    TyreType = TyreCompound.MEDIUM;
                    break;

                case "Hard":
                    TyreType = TyreCompound.HARD;
                    break;

                default:
                    return;
            }

            DialogResult = true;
        }
    }
}

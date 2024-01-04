using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace USAL_SimulaFONS
{
    /// <summary>
    /// Lógica de interacción para AskForFileNameWindow.xaml
    /// </summary>
    public partial class AskForFileNameWindow : Window
    {
        public string fileName { get; set; }
        public AskForFileNameWindow()
        {
            InitializeComponent();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtbx.Text.Equals(string.Empty)) { MessageBox.Show("Introduzca en nombre del archivo p.f", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            fileName = txtbx.Text;
            DialogResult = true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
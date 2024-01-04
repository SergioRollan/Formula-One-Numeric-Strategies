using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace USAL_SimulaFONS
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model appModel;
        MLApp.MLApp matlab;

        public MainWindow()
        {
            InitializeComponent();

            appModel = new Model();
            matlab = new MLApp.MLApp();

            this.DataContext = appModel;
            this.trackMenu.DataContext = appModel.TrackData;

            driverList.ItemsSource = appModel.Drivers;
            raceList.ItemsSource = appModel.RaceData.LapStates;

        }

        private void driverList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Driver tmpDriver = (Driver)driverList.SelectedItem;

            if (tmpDriver == null) return;

            dorsalTxt.Text = tmpDriver.Dorsal.ToString();
            nameTxt.Text = tmpDriver.Name;
            surnameTxt.Text = tmpDriver.Surname;
            startingPosTxt.Text = tmpDriver.StartPos.ToString();
            qTimeTxt.Text = tmpDriver.QTime.ToString();
            tyreList.ItemsSource = appModel.Drivers[appModel.Drivers.IndexOf(tmpDriver)].Tyres;

        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            Regex regexInt = new Regex("^[0-9]+$");
            Regex regexDouble = new Regex("[-+]?[0-9]*,?[0-9]+$");

            Driver tmpDriver = null;

            if (dorsalTxt.Text.Length == 0 || nameTxt.Text.Length == 0 || surnameTxt.Text.Length == 0 || qTimeTxt.Text.Length == 0 || startingPosTxt.Text.Length == 0)
            {
                MessageBox.Show("Deben de ser introducidos todos los campos para añadir a un piloto", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!regexInt.IsMatch(dorsalTxt.Text) || !regexInt.IsMatch(startingPosTxt.Text) || !regexDouble.IsMatch(qTimeTxt.Text))
            {
                MessageBox.Show("Los campos numericos contienen texto", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Driver existingDriver = null;
            foreach (Driver driverIterator in appModel.Drivers)
                if (driverIterator.Dorsal == int.Parse(dorsalTxt.Text))
                {
                    existingDriver = driverIterator;
                    break;
                }

            try
            {
                tmpDriver = new Driver(int.Parse(dorsalTxt.Text), nameTxt.Text, surnameTxt.Text, double.Parse(qTimeTxt.Text), int.Parse(startingPosTxt.Text), 0);
            }
            catch (Exception)
            {
                MessageBox.Show("Ha habido algún error en los campos", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (existingDriver != null)
            {
                tmpDriver.Tyres = existingDriver.Tyres;
                appModel.Drivers.Remove(existingDriver);
            }

            appModel.Drivers.Add(tmpDriver);

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Driver tmpDriver = (Driver)driverList.SelectedItem;

            if (tmpDriver == null) return;

            appModel.Drivers.Remove(tmpDriver);
        }



        private void addTyreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (appModel.Drivers.IndexOf((Driver)driverList.SelectedItem) == -1)
            {
                MessageBox.Show("Añada primero al piloto antes de añadir sus neumáticos", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AddTyreWindow tyreWindow = new AddTyreWindow();

            tyreWindow.ShowDialog();

            if (tyreWindow.DialogResult == true)
            {
                Tyre tempTyre = new Tyre(tyreWindow.TyreType, tyreWindow.TyreLaps);

                appModel.Drivers[appModel.Drivers.IndexOf((Driver)driverList.SelectedItem)].Tyres.Add(tempTyre);
            }

        }

        private void deleteTyreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (appModel.Drivers.IndexOf((Driver)driverList.SelectedItem) != -1)
                appModel.Drivers[appModel.Drivers.IndexOf((Driver)driverList.SelectedItem)].Tyres.Remove((Tyre)this.tyreList.SelectedItem);
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (appTabControl.SelectedIndex == 3)
            {
                trackMenu.DataContext = appModel.TrackData;
            }

        }

        private static double doubleParse(object value)
        {
            double result;

            string doubleAsString = value.ToString();
            IEnumerable<char> doubleAsCharList = doubleAsString.ToList();

            if (doubleAsCharList.Where(ch => ch == '.' || ch == ',').Count() <= 1)
            {
                double.TryParse(doubleAsString.Replace(',', '.'),
                    System.Globalization.NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out result);
            }
            else
            {
                if (doubleAsCharList.Where(ch => ch == '.').Count() <= 1
                    && doubleAsCharList.Where(ch => ch == ',').Count() > 1)
                {
                    double.TryParse(doubleAsString.Replace(",", string.Empty),
                        System.Globalization.NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out result);
                }
                else if (doubleAsCharList.Where(ch => ch == ',').Count() <= 1
                    && doubleAsCharList.Where(ch => ch == '.').Count() > 1)
                {
                    double.TryParse(doubleAsString.Replace(".", string.Empty).Replace(',', '.'),
                        System.Globalization.NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out result);
                }
                else
                {
                    throw new Exception($"Error parsing {doubleAsString} as double, try removing thousand separators (if any)");
                }
            }

            return result;
        }
        private void trackSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                appModel.TrackData.RacePaceDiff = doubleParse(racePaceDiffTxt.Text);
                appModel.TrackData.GridPosTimeLoss = doubleParse(gridPositionTimeLossTxt.Text);
                appModel.TrackData.StandingStartTimeLoss = doubleParse(standingStartTxt.Text);
                appModel.TrackData.StartingFuel = int.Parse(startingFuelTxt.Text);
                appModel.TrackData.FuelTimeLoss = doubleParse(fuelTimeLossTxt.Text);
                appModel.TrackData.FuelMassLoss = doubleParse(fuelMassLossTxt.Text);
                appModel.TrackData.DRSTimeGain = doubleParse(drsTimeGainTxt.Text);
                appModel.TrackData.OvertakingDeltaDiff = doubleParse(overtakingDeltaTxt.Text);
                appModel.TrackData.OvertakingTimeLoss = doubleParse(overtakingTimeTxt.Text);
                appModel.TrackData.OvertakenTimeLoss = doubleParse(overtakenTimeTxt.Text);
                appModel.TrackData.PitstopLossGreen = doubleParse(pitStopGreenTxt.Text);
                appModel.TrackData.PitstopLossSafetyCar = doubleParse(pitStopYellowTxt.Text);
                appModel.TrackData.DeltaVSCTime = doubleParse(deltaVSCTimeTxt.Text);
                appModel.TrackData.LapDegHard = doubleParse(hardDegradationTxt.Text);
                appModel.TrackData.LapDegMed = doubleParse(mediumDegradationTxt.Text);
                appModel.TrackData.LapDegSoft = doubleParse(softDegradationTxt.Text);
                appModel.TrackData.LapDiffSoftMed = doubleParse(mediumDiffTxt.Text);
                appModel.TrackData.LapDiffSoftHard = doubleParse(hardDiffTxt.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Algún valor numérico contiene texto.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            return;
        }

        private void driverExportBtn_Click(object sender, RoutedEventArgs e)
        {
            AskForFileNameWindow filenamewin = new AskForFileNameWindow();
            filenamewin.ShowDialog();
            if (filenamewin.DialogResult == false) return;
            string fileName = filenamewin.fileName;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string carpeta = "/FilesFONS";
            StringBuilder sb = new StringBuilder();
            sb.Append(desktopPath).Append(carpeta);
            string ruta = sb.ToString();
            sb.Append("/").Append(fileName).Append(".fons");
            string archivo = sb.ToString();
            if (!File.Exists(archivo))
            {
                Directory.CreateDirectory(ruta);
                File.CreateText(archivo).Dispose();
                StreamWriter sw = new StreamWriter(archivo);
                string strWr = appModel.DriversString();
                if (strWr == null) { MessageBox.Show("No hay suficientes datos de pilotos para exportar", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                sw.Write(strWr);
                sw.Close();
            }
            else MessageBox.Show("Ya existe un archivo .fons con ese nombre", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show("Archivo creado con éxito.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void trackExportBtn_Click(object sender, RoutedEventArgs e)
        {
            AskForFileNameWindow filenamewin = new AskForFileNameWindow();
            filenamewin.ShowDialog();
            if (filenamewin.DialogResult == false) return;
            string fileName = filenamewin.fileName;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string carpeta = "/FilesFONS";
            StringBuilder sb = new StringBuilder();
            sb.Append(desktopPath).Append(carpeta);
            string ruta = sb.ToString();
            sb.Append("/").Append(fileName).Append(".fons");
            string archivo = sb.ToString();
            if (!File.Exists(archivo))
            {
                Directory.CreateDirectory(ruta);
                File.CreateText(archivo).Dispose();
                StreamWriter sw = new StreamWriter(archivo);
                string strWr = appModel.TrackData.ToStringCustom();
                if (strWr == null) { MessageBox.Show("No hay suficientes datos de circuito para exportar", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                sw.Write(strWr);
                sw.Close();
            }
            else MessageBox.Show("Ya existe un archivo .fons con ese nombre", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show("Archivo creado con éxito.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void allExportBtn_Click(object sender, RoutedEventArgs e)
        {
            AskForFileNameWindow filenamewin = new AskForFileNameWindow();
            filenamewin.ShowDialog();
            if (filenamewin.DialogResult == false) return;
            string fileName = filenamewin.fileName;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string carpeta = "/FilesFONS";
            StringBuilder sb = new StringBuilder();
            sb.Append(desktopPath).Append(carpeta);
            string ruta = sb.ToString();
            sb.Append("/").Append(fileName).Append(".fons");
            string archivo = sb.ToString();
            if (!File.Exists(archivo))
            {
                Directory.CreateDirectory(ruta);
                File.CreateText(archivo).Dispose();
                StreamWriter sw = new StreamWriter(archivo);
                string strWr1 = appModel.DriversString();
                if (strWr1 == null) { MessageBox.Show("No hay suficientes datos de pilotos para exportar", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                string strWr2 = appModel.TrackData.ToStringCustom();
                if (strWr2 == null) { MessageBox.Show("No hay suficientes datos de circuito para exportar", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                sw.Write(strWr2);
                sw.Write(strWr1);
                sw.Close();
            }
            else MessageBox.Show("Ya existe un archivo .fons con ese nombre", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show("Archivo creado con éxito.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void driverImportBtn_Click(object sender, RoutedEventArgs e)
        {

            AskForFileNameWindow filenamewin = new AskForFileNameWindow();
            filenamewin.ShowDialog();
            if (filenamewin.DialogResult == false) return;
            string fileName = filenamewin.fileName;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string carpeta = "/FilesFONS";
            StringBuilder sb = new StringBuilder();
            sb.Append(desktopPath).Append(carpeta);
            string ruta = sb.ToString();
            sb.Append("/").Append(fileName).Append(".fons");
            string archivo = sb.ToString();
            if (!File.Exists(archivo))
            {
                MessageBox.Show("No se ha encontrado un archivo .fons en la carpeta FilesFONS en el escritorio con ese nombre", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            char[] buffer = new char[3];
            System.IO.StreamReader sr = new StreamReader(archivo);
            sr.ReadBlock(buffer, 0, 3);
            string codigo = string.Format("{0}", new string(buffer));
            if (!codigo.Equals("-1\t"))
            {
                MessageBox.Show("El archivo abierto tiene un circuito", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ObservableCollection<Driver> listTemp = new ObservableCollection<Driver>();
            Driver drvTemp;
            ObservableCollection<Tyre> ruedasTemp;
            Tyre ruedaTemp;
            try
            {
                while (!sr.EndOfStream)
                {
                    codigo = sr.ReadLine();
                    if (codigo == null) break;
                    string[] nums = codigo.Split('\t');
                    if (nums.Length < 7)
                    {
                        MessageBox.Show("El archivo de pilotos está incompleto", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    drvTemp = new Driver(int.Parse(nums[0]), nums[2], nums[3], double.Parse(nums[4]), int.Parse(nums[1]), double.Parse(nums[5]));
                    ruedasTemp = new ObservableCollection<Tyre>();
                    for (int i = 6; i < nums.Length; i += 2)
                    {
                        ruedaTemp = new Tyre(int.Parse(nums[i + 1]), int.Parse(nums[i]));
                        ruedasTemp.Add(ruedaTemp);
                    }
                    drvTemp.Tyres = ruedasTemp;
                    listTemp.Add(drvTemp);
                }
                for (int i = 0; i < appModel.Drivers.Count; i++) appModel.Drivers.Remove(appModel.Drivers[0]);
                foreach (Driver dr in listTemp) appModel.Drivers.Add(dr);
            }
            catch (Exception)
            {
                MessageBox.Show("Error durante el formateo de la lectura.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            sr.Close();
            MessageBox.Show("Pilotos creados con éxito.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void trackImportBtn_Click(object sender, RoutedEventArgs e)
        {
            AskForFileNameWindow filenamewin = new AskForFileNameWindow();
            filenamewin.ShowDialog();
            if (filenamewin.DialogResult == false) return;
            string fileName = filenamewin.fileName;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string carpeta = "/FilesFONS";
            StringBuilder sb = new StringBuilder();
            sb.Append(desktopPath).Append(carpeta);
            string ruta = sb.ToString();
            sb.Append("/").Append(fileName).Append(".fons");
            string archivo = sb.ToString();
            if (!File.Exists(archivo))
            {
                MessageBox.Show("No se ha encontrado un archivo .fons en la carpeta FilesFONS en el escritorio con ese nombre", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            char[] buffer = new char[3];
            System.IO.StreamReader sr = new StreamReader(archivo);
            sr.ReadBlock(buffer, 0, 3);
            string codigo = string.Format("{0}", new string(buffer));
            if (!codigo.Equals("-2\t"))
            {
                MessageBox.Show("El archivo abierto no ha guardado ningún circuito", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                codigo = sr.ReadLine();
                string[] nums = codigo.Split('\t');
                if (nums.Length != 19)
                {
                    MessageBox.Show("El archivo del circuito está incompleto", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                appModel.TrackData.LapNum = int.Parse(nums[0]);
                appModel.TrackData.StartingFuel = int.Parse(nums[1]);
                appModel.TrackData.RacePaceDiff = double.Parse(nums[2]);
                appModel.TrackData.GridPosTimeLoss = double.Parse(nums[3]);
                appModel.TrackData.StandingStartTimeLoss = double.Parse(nums[4]);
                appModel.TrackData.FuelTimeLoss = double.Parse(nums[5]);
                appModel.TrackData.FuelMassLoss = double.Parse(nums[6]);
                appModel.TrackData.DRSTimeGain = double.Parse(nums[7]);
                appModel.TrackData.OvertakingDeltaDiff = double.Parse(nums[8]);
                appModel.TrackData.OvertakingTimeLoss = double.Parse(nums[9]);
                appModel.TrackData.OvertakenTimeLoss = double.Parse(nums[10]);
                appModel.TrackData.PitstopLossGreen = double.Parse(nums[11]);
                appModel.TrackData.PitstopLossSafetyCar = double.Parse(nums[12]);
                appModel.TrackData.DeltaVSCTime = double.Parse(nums[13]);
                appModel.TrackData.LapDegHard = double.Parse(nums[14]);
                appModel.TrackData.LapDegMed = double.Parse(nums[15]);
                appModel.TrackData.LapDegSoft = double.Parse(nums[16]);
                appModel.TrackData.LapDiffSoftHard = double.Parse(nums[17]);
                appModel.TrackData.LapDiffSoftMed = double.Parse(nums[18]);
            }
            catch (Exception)
            {
                MessageBox.Show("Error durante el formateo de la lectura.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            sr.Close();
            MessageBox.Show("Circuito creado con éxito.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void allImportBtn_Click(object sender, RoutedEventArgs e)
        {


            AskForFileNameWindow filenamewin = new AskForFileNameWindow();
            filenamewin.ShowDialog();
            if (filenamewin.DialogResult == false) return;
            string fileName = filenamewin.fileName;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string carpeta = "/FilesFONS";
            StringBuilder sb = new StringBuilder();
            sb.Append(desktopPath).Append(carpeta);
            string ruta = sb.ToString();
            sb.Append("/").Append(fileName).Append(".fons");
            string archivo = sb.ToString();
            if (!File.Exists(archivo))
            {
                MessageBox.Show("No se ha encontrado un archivo .fons en la carpeta FilesFONS en el escritorio con ese nombre", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            char[] buffer = new char[3];
            System.IO.StreamReader sr = new StreamReader(archivo);
            sr.ReadBlock(buffer, 0, 3);
            string codigo = new string(buffer);
            if (!codigo.Equals("-2\t"))
            {
                MessageBox.Show("El archivo abierto solo tiene pilotos.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ObservableCollection<Driver> listTemp = new ObservableCollection<Driver>();
            Driver drvTemp;
            ObservableCollection<Tyre> ruedasTemp;
            Tyre ruedaTemp;
            try
            {
                codigo = sr.ReadLine();
                string[] nums = codigo.Split('\t');
                if (nums.Length != 19)
                {
                    MessageBox.Show("El archivo del circuito está incompleto", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                appModel.TrackData.LapNum = int.Parse(nums[0]);
                appModel.TrackData.StartingFuel = int.Parse(nums[1]);
                appModel.TrackData.RacePaceDiff = double.Parse(nums[2]);
                appModel.TrackData.GridPosTimeLoss = double.Parse(nums[3]);
                appModel.TrackData.StandingStartTimeLoss = double.Parse(nums[4]);
                appModel.TrackData.FuelTimeLoss = double.Parse(nums[5]);
                appModel.TrackData.FuelMassLoss = double.Parse(nums[6]);
                appModel.TrackData.DRSTimeGain = double.Parse(nums[7]);
                appModel.TrackData.OvertakingDeltaDiff = double.Parse(nums[8]);
                appModel.TrackData.OvertakingTimeLoss = double.Parse(nums[9]);
                appModel.TrackData.OvertakenTimeLoss = double.Parse(nums[10]);
                appModel.TrackData.PitstopLossGreen = double.Parse(nums[11]);
                appModel.TrackData.PitstopLossSafetyCar = double.Parse(nums[12]);
                appModel.TrackData.DeltaVSCTime = double.Parse(nums[13]);
                appModel.TrackData.LapDegHard = double.Parse(nums[14]);
                appModel.TrackData.LapDegMed = double.Parse(nums[15]);
                appModel.TrackData.LapDegSoft = double.Parse(nums[16]);
                appModel.TrackData.LapDiffSoftHard = double.Parse(nums[17]);
                appModel.TrackData.LapDiffSoftMed = double.Parse(nums[18]);
            }
            catch (Exception)
            {
                MessageBox.Show("Error durante el formateo de la lectura del circuito.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try { 
                sr.ReadBlock(buffer, 0, 3);
                while (!sr.EndOfStream)
                {
                    codigo = sr.ReadLine();
                    if (codigo == null) break;
                    string[] nums = codigo.Split('\t');
                    if (nums.Length < 7)
                    {
                        MessageBox.Show("El archivo de pilotos está incompleto", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    drvTemp = new Driver(int.Parse(nums[0]), nums[2], nums[3], double.Parse(nums[4]), int.Parse(nums[1]), double.Parse(nums[5]));
                    ruedasTemp = new ObservableCollection<Tyre>();
                    for (int i = 6; i < nums.Length; i += 2)
                    {
                        ruedaTemp = new Tyre(int.Parse(nums[i + 1]), int.Parse(nums[i]));
                        ruedasTemp.Add(ruedaTemp);
                    }
                    drvTemp.Tyres = ruedasTemp;
                    listTemp.Add(drvTemp);
                }
                for (int i = 0; i < appModel.Drivers.Count; i++) appModel.Drivers.Remove(appModel.Drivers[0]);
                foreach (Driver dr in listTemp) appModel.Drivers.Add(dr);
            }
            catch (Exception)
            {
                MessageBox.Show("Error durante el formateo de la lectura de los pilotos.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            sr.Close();
            MessageBox.Show("Pilotos y circuito creados con éxito.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #region funciones ultra importantes cruciales para el funcionamiento del programa, NO DESPLEGAR
        /*  Juanjo - Magic Alonso
           
            El Nano es una bala azul que sin cañón
            Dispara en un circuito directo al corazón
            El Nano no es humano, el Nano es inmortal
            Y sale en las revistas junto a Hulk(enberg) y a Max Max Max Super Max Max Super super Max Max Max Super Max Max Super super Max Max Max Super Max Max Super super Max Max Max Super Max Max
            El Nano es un gigante en un cuerpo de mortal
            Y nadie le echa el guante, nadie lo puede alcanzar
            El Nano ah-eh, el Nano ah-oh
            No quiero a Barrichello, Schumacher ni al <Button/>
            Porque es el Nano quien llena a todos de ilusión
            Cuando se sube en su Renault (Magic Alonso)
            El Nano es buena gente, es un tío enrollao'
            Y dentro del circuito es el que parte el CA LA MA RI
            Fernando, te queremos por solo una razón
            Coges un día negro y nos lo llenas de emoción
            Fernando es un gigante en un cuerpo de mortal
            Y nadie le echa el guante, nadie lo puede alcanzar
            El Nano ah-eh, el Nano ah-oh
            No quiero a Barrichello, Schumacher ni al Button
            Porque es el Nano quien llena a todos de ilusión
            Cuando se sube en su Renault (Magic Alonso)
            El Nano ah-eh, el Nano ah-oh
            No quiero a Barrichello, Schumacher ni al <Button/>
            Porque es el Nano quien llena a todos de ilusión
            Cuando se sube en su Renault (Magic Alonso)
            El Nano no nos falla, si hay anuncios no te vayas
            Porque el siempre da la talla, da igual el puesto que salga
            No se arruga, no defrauda, no se encoge porque no conoce el miedo
            Se cayó de pequeño en una marmita de sidra
            Y desde entonces no le frenan ni con pura criptonita
            Porque el Nano es para el pueblo y por el pueblo sin dudarlo, el rey del viento
            El Nano ah-eh-eh-eh-oh
            El Nano ah-eh-eh-eh-oh

            V.S

            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Ik mis geen race, heb schijt aan mijn centen
            Ik zie Max z'n mooiste momenten
            Hij is de king van Barcelona
            Rijd alles zoek van Spa tot Monza
            De geur van rubber en smeltend asfalt
            Ik juich als Max weer een voorbij knalt
            Zijn inhaalacties zijn bijzonder
            Het is een bazend wereldwonder
            Jowf*ckingho, niet meer normaal
            Hij is een held en gaat maximaal
            F*cking bizar, een fenomeen
            Hij gaat naar de top, Max is de nummer 1
            Jowf*ckingho, niet meer normaal
            Hij is een held en gaat maximaal
            F*cking bizar, een fenomeen
            Hij gaat naar de top, Max is de nummer 1
            Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Jaren lang zit je te kijken en te hopen op succes
            Zodra je Max ziet racen op de baan dan denk je yes
            Hij is niet te passeren niemand krijgt het cadeau
            Hij is een pure racer vangt ze op en roept nooo!
            Hij is de nieuwe baas (hij is de nieuwe baas)
            Max is de nieuwe baas (hij is de nieuwe baas)
            Ja wij gaan helemaal los (hij is de nieuwe boss)
            Super Max super Max ik ben apetrots!
            Jowf*ckingho, niet meer normaal
            Hij is een held en gaat maximaal
            F*cking bizar, een fenomeen
            Hij gaat naar de top, Max is de nummer 1
            Jowf*ckingho, niet meer normaal
            Hij is een held en gaat maximaal
            F*cking bizar, een fenomeen
            Hij gaat naar de top, Max is de nummer 1
            Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            Super super Max Max Max
            Super Max Max
            
         */
        #endregion


        private void addIncidentBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                appModel.RaceData.LapStates.Add(new RaceState { LapStart = int.Parse(lapStart.Text), LapEnd = int.Parse(lapEnd.Text), IncidentType = (incidentCombo.Text.Equals("VSC")) ? 1 : (incidentCombo.Text.Equals("SC")) ? 2 : 3 });
            }
            catch (Exception)
            {
                MessageBox.Show("Las vueltas de inicio y de final no están bien marcadas.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void deleteIncidentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (appModel.RaceData.LapStates.IndexOf((RaceState)raceList.SelectedItem) != -1)
                appModel.RaceData.LapStates.RemoveAt(appModel.RaceData.LapStates.IndexOf((RaceState)raceList.SelectedItem));
        }



        private void startSimulation_Click(object sender, RoutedEventArgs e)
        {
            double[,] pilotos;
            double[] numPitStops;
            double[,] pitstops;
            double[,] compuestos;
            double[] banderas;
            double[] circuito;


            try
            {

                double bestTime = appModel.Drivers[0].QTime;
                foreach (Driver driver in appModel.Drivers) 
                    bestTime = (driver.QTime < bestTime) ? driver.QTime : bestTime;


                foreach (Driver dv in appModel.Drivers) 
                    dv.AbilityDiff = dv.QTime - bestTime;
                

                pilotos = new double[appModel.Drivers.Count, 5];
                int i = 0;
                for (i = 0; i < appModel.Drivers.Count; i++)
                {
                    pilotos[i, 0] = appModel.Drivers[i].Dorsal;
                    pilotos[i, 1] = appModel.Drivers[i].AbilityDiff;
                    pilotos[i, 2] = appModel.Drivers[i].StartPos;
                    pilotos[i, 3] = appModel.Drivers[i].Strategy[0].NewTyre.TyreType;//ruedas inicio
                    pilotos[i, 4] = appModel.Drivers[i].Strategy[0].NewTyre.LapsNum;//vueltas de ruedas inicio
                }


                #region NumPitstops
                numPitStops = new double[appModel.Drivers.Count];

                i = 0;
                int j;

                foreach (Driver driver in appModel.Drivers)
                {
                    j = 0;
                    foreach (Pitstop ps in driver.Strategy)
                        if(ps.StopLap != -1)
                            j++;

                    numPitStops[i] = j;
                    i++;
                }

                #endregion


                #region Pitstops
                int maxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMax = (int)numPitStops[0];

                for (i = 1; i < numPitStops.Length; i++)
                    if (maxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMax < (int)numPitStops[i])
                        maxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMax = (int)numPitStops[i];

                pitstops = new double[appModel.Drivers.Count, maxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMaxSuperSuperMaxMaxMaxSuperMaxMax * 4];

                i = 0;
                foreach (Driver driver in appModel.Drivers)
                {

                    j = 0;
                    foreach (Pitstop pitstop in driver.Strategy) 
                    {
                        if (pitstop.StopLap == -1) continue;

                        pitstops[i, j] = pitstop.StopLap+1;
                        pitstops[i, j + 1] = pitstop.NewTyre.TyreType;
                        pitstops[i, j + 2] = pitstop.NewTyre.LapsNum;
                        pitstops[i, j + 3] = 0;

                        j += 4;
                    }

                    i++;
                }
                #endregion


                #region Circuito

                circuito = new double[14];
                circuito[0] = bestTime;
                circuito[1] = appModel.TrackData.RacePaceDiff;
                circuito[2] = appModel.TrackData.GridPosTimeLoss;
                circuito[3] = appModel.TrackData.StandingStartTimeLoss;
                circuito[4] = appModel.TrackData.StartingFuel;
                circuito[5] = appModel.TrackData.FuelTimeLoss;
                circuito[6] = appModel.TrackData.FuelMassLoss;
                circuito[7] = appModel.TrackData.DRSTimeGain;
                circuito[8] = appModel.TrackData.OvertakingDeltaDiff;
                circuito[9] = appModel.TrackData.OvertakingTimeLoss;
                circuito[10] = appModel.TrackData.OvertakenTimeLoss;
                circuito[11] = appModel.TrackData.DeltaVSCTime;
                circuito[12] = appModel.TrackData.PitstopLossGreen;
                circuito[13] = appModel.TrackData.PitstopLossSafetyCar;
                #endregion


                #region Banderas
                banderas = new double[appModel.RaceData.Laps];

                for (i = 0; i < banderas.Length; i++)
                    banderas[i] = 0;

                foreach (RaceState raceState in appModel.RaceData.LapStates)
                    for (i = raceState.LapStart; i <= raceState.LapEnd; i++)
                        if(i<banderas.Length && i>0) banderas[i] = raceState.IncidentType;

                #endregion


                #region Compuestos
                compuestos = new double[appModel.Drivers.Count, 6];

                for (i = 0; i < appModel.Drivers.Count; i++)
                {
                    compuestos[i, 0] = appModel.TrackData.LapDegHard;
                    compuestos[i, 1] = appModel.TrackData.LapDiffSoftHard;
                    compuestos[i, 2] = appModel.TrackData.LapDegMed;
                    compuestos[i, 3] = appModel.TrackData.LapDiffSoftMed;
                    compuestos[i, 4] = appModel.TrackData.LapDegSoft;
                    compuestos[i, 5] = 0;
                }
                #endregion

            }
            catch (Exception)
            {
                MessageBox.Show("Hay algún error en los campos de la simulación. Revise el programa.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                // Invocamos a Matlab construyendo un workspace usando las matrices
                int i, j;


                StringBuilder sba = new StringBuilder();
                StringBuilder sbb = new StringBuilder();
                StringBuilder sbc = new StringBuilder();
                StringBuilder sbd = new StringBuilder();
                StringBuilder sbe = new StringBuilder();
                StringBuilder sbf = new StringBuilder();


                //a
                sba.Append("pilotos=[");
                for (i = 0; i < pilotos.GetLength(0); i++)
                {
                    for (j = 0; j < pilotos.GetLength(1); j++)
                    {
                        sba.Append(doubleToString(pilotos[i, j])).Append(",");
                    }
                    sba.Append(";");
                }
                sba.Append("];");
                //b
                sbb.Append("numPitStops=["); for (i = 0; i < numPitStops.Length; i++) sbb.Append(doubleToString(numPitStops[i])).Append(","); sbb.Append("];");
                //c
                sbc.Append("pitstops=[");
                for (i = 0; i < pitstops.GetLength(0); i++)
                {
                    for (j = 0; j < pitstops.GetLength(1); j++)
                    {
                        sbc.Append(doubleToString(pitstops[i, j])).Append(",");
                    }
                    sbc.Append(";");
                }
                sbc.Append("];");
                //d
                sbd.Append("compuestos=[");
                for (i = 0; i < compuestos.GetLength(0); i++)
                {
                    for (j = 0; j < compuestos.GetLength(1); j++)
                    {
                        sbd.Append(doubleToString(compuestos[i, j])).Append(",");
                    }
                    sbd.Append(";");
                }
                sbd.Append("];");
                //e
                sbe.Append("banderas=["); for (i = 0; i < banderas.Length; i++) sbe.Append(doubleToString(banderas[i])).Append(","); sbe.Append("];");
                //f
                sbf.Append("circuito=["); for (i = 0; i < 14; i++) sbf.Append(doubleToString(circuito[i])).Append(","); sbf.Append("];");
                matlab.Execute(@"cd " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/FilesFONS");


                matlab.Execute(sba.ToString());
                matlab.Execute(sbb.ToString());
                matlab.Execute(sbc.ToString());
                matlab.Execute(sbd.ToString());
                matlab.Execute(sbe.ToString());
                matlab.Execute(sbf.ToString());
                matlab.Execute("save('global')");
                matlab.Execute("funcionCarreraEntera");


                BitmapImage bmimg = new BitmapImage();
                bmimg.BeginInit();
                bmimg.UriSource = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/FilesFONS" + "/grafica.png");
                bmimg.EndInit();
                resImage.Source = bmimg;
            }
            catch (Exception)
            {
                MessageBox.Show("Ha ocurrido algún error durante la ejecución del programa de Matlab.", "SimulaFONS", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        String doubleToString(double num)
        {
            char[] vec = num.ToString().ToCharArray();
            for (int i = 0; i < vec.Length; i++) if (vec[i] == ',') vec[i] = '.';
            return new string(vec);
        }

        private void vaciarImg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}

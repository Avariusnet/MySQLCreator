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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data;
using MySql.Data.MySqlClient;
using System.Net.NetworkInformation;
using System.Net;
using Logging;
using System.IO;
using Export;
using System.Threading;
using Ausgabe;

namespace SQL_Connector
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            Initialisieren();
        }



        private bool verbindungaufbauen()
        {


            Verbindung vbd = new Verbindung();
            string ip = IP.Text;
            string port = PORT.Text;
            string user = USER.Text;
            string pass = PASSWORD.Password;


            bool connect = Verbindung.testverbindung(ip, port, user, pass);

            if (connect)
            {

                List<string> schema = new List<string>();
                SCHEMABOX.Items.Clear();

                COLUMBOX.Items.Clear();
                TABLEBOX.Items.Clear();
                PROGRESSBAR.Minimum = 0;

                PROGRESSBAR.Maximum = schema.Count();

                Abfrage.Visibility = System.Windows.Visibility.Visible;
                SCHEMABOX.Visibility = System.Windows.Visibility.Visible;
                SCHEMATEXT.Visibility = System.Windows.Visibility.Visible;
                SCHEMABOX.IsEnabled = true;
                Abfrage.IsEnabled = true;
                QUERY.IsEnabled = true;
                schema = Verbindung.schemaabfrage(ip, port, user, pass);


                

                int count = 0;
                foreach (string element in schema)
                {
                    if(element == "Error")
                    {
                        Logger.Logs("Can´t login to the SQL Database.");
                        MessageBox.Show("Check your Logindata");
                        Abfrage.Visibility = System.Windows.Visibility.Hidden;
                        SCHEMABOX.Visibility = System.Windows.Visibility.Hidden;
                        SCHEMATEXT.Visibility = System.Windows.Visibility.Hidden;
                        SCHEMABOX.IsEnabled = false;
                        Abfrage.IsEnabled = false;
                        QUERY.IsEnabled = false;
                        return true;
                    }
                    INFOBOX.AppendText("Added Element: " + element + "\n");
                    SCHEMABOX.Items.Add(element);
                    PROGRESSBAR.Value = count * 10;
                    count++;
                }

                OPTIONBOX.IsEnabled = true;
                TABLEBOX.IsEnabled = true;
                DISCONNECT.IsEnabled = true;
                VERBINDEN.IsEnabled = false;
                return true;
            }




            /*
            MessageBox.Show("Verbindung konnte nicht aufgebaut werden.");
            INFOBOX.AppendText("Verbindung konnte nicht hergestellt werden. Falsche IP eingegeben? \n");
            SCHEMABOX.IsEnabled = false;
            Abfrage.IsEnabled = false;
            QUERY.IsEnabled = false;

            PORT.Text = "";
            USER.Text = "";
            PASSWORD.Password = "";
            */
            return false;
            

            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            EXPORT.IsEnabled = true;
            Verbindung vbd = new Verbindung();

            List<string> ergebnis = Ausgabe.Ausgabe.ausgabe(RESULT, QUERY, EXPORT, OPTIONBOX, SEARCHBOX, SCHEMABOX, COLUMBOX, TABLEBOX, IP, PORT, USER, PASSWORD);
            QUERY.Clear();
            foreach (string element in ergebnis)
            {
                if (element == "Error")
                {
                    EXPORT.IsEnabled = false;
                }


                RESULT.AppendText(element);
            }

            Logger.Logs("Query wurde erfolgreich abgeschlossen. " + QUERY.Text);
            INFOBOX.AppendText("Query abgeschlossen\n");
            return;

        }

        private void IP_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (String.IsNullOrEmpty(IP.Text))
                {
                    MessageBox.Show("Please insert an IP Adress or an valid Hostname");
                    return;
                }

                if (IP.Text != "")
                {
                    string ip = IP.Text;
                    bool verbunden = Verbindung.ping(ip);
                    if (verbunden)
                    {
                        INFOBOX.AppendText("Verbindung erfolgreich geprüft. \n");
                        PORT.IsEnabled = true;
                        USER.IsEnabled = true;
                        PASSWORD.IsEnabled = true;
                        VERBINDEN.IsEnabled = true;
                        return;
                    }
                    else
                    {
                        Logging.Logger.Logs("Server mit der IP: " + ip + " nicht erreichbar.");
                        MessageBox.Show("Server nicht erreichbar");
                        return;
                    }

                } 
            }
        }

        private bool loadsettings()
        {
            IP.Text = mySetting.Default.IP;
            PORT.Text = mySetting.Default.PORT;
            USER.Text = mySetting.Default.USERNAME;
            PASSWORD.Password = mySetting.Default.PASSWORD;
            COPYRIGHT.Text = mySetting.Default.Copyright;

            mySetting.Default.Copyright = "Copyright Exitare 2016-2017";
            mySetting.Default.Save();
            
            return true;
        }

        private void Einstellungen_Click(object sender, RoutedEventArgs e)
        {
            var einstellungen = new Window();
            einstellungen.Show();
            this.Close();
        }

        private void VERBINDEN_Click(object sender, RoutedEventArgs e)
        {
            verbindungaufbauen();
        }

        private void EXPORT_Click(object sender, RoutedEventArgs e)
        {
            string data = "";
            string filename = FILENAME.Text;
           

            List<string> ergebnis = Verbindung.GetQueryValues(IP, PORT, TABLEBOX, USER, PASSWORD, QUERY);
            foreach (var element in ergebnis)
            {
                data += element;
            }

            
            if (String.IsNullOrEmpty(filename))
            {
                INFOBOX.AppendText("Missing filename. \n");
            }
            

            List<string> liste = Verbindung.Spaltennamen(IP, PORT, TABLEBOX, USER,PASSWORD,QUERY);
                   
            try
            {
                bool sucess = Export.Export.exportsql(data, filename, TABLEBOX, liste, BEFEHLBOX);

                if (sucess)
                {
                    INFOBOX.AppendText("SQL-Export successfull \n");
                    return;
                }

                if (!sucess)
                {
                    INFOBOX.AppendText("SQL-Export failed. \nView Log for further Information \nNo Data available. \n");
                    return;
                }
            }

            catch(Exception ex)
            {
                Logger.Logs(ex.Message);
                return;
            }
        }


        private void EINSTELLUNGEN_Click(object sender, RoutedEventArgs e)
        {
            Settingspage  ui = new Settingspage();
            Window test = new Window();
            TextBox box = new TextBox();
                     
            test.Show();


        }

        private void UPDATE_Click(object sender, RoutedEventArgs e)
        {
            string programmversion = Statics.Statics.ProgrammVersion;
            
            int update = Update.Update.update(programmversion);
            MessageBox.Show(Update.Update.updatemeldung(update));
           
        }

        public void Initialisieren()
        {
            INFOBOX.IsReadOnly = true;
            COPYRIGHT.Text = mySetting.Default.Copyright;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            loadsettings();
            Verbindung.changevisibilty(OPTIONBOX,SCHEMABOX,TABLEBOX,SEARCHBOX,COLUMBOX,OPTIONTEXT,SCHEMATEXT,SEARCHTEXT,COLUMTEXT,TABLETEXT);
            StreamReader sr = new StreamReader("version.txt");
            string completestring = sr.ReadToEnd();
            char trennzeichen = ',';
            String[] substrings = completestring.Split(trennzeichen);
            Console.WriteLine(substrings[0]);
            Console.WriteLine(substrings[1]);

            VERSION.Text = substrings[0] +  substrings[1];

        }
        private void SCHEMA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TABLEBOX.Items.Clear();
            
            List<string> tables = new List<string>();
           
            tables = Data.Verbindung.tableabfrage(IP, PORT, USER, PASSWORD,SCHEMABOX);
            
            TABLEBOX.IsEnabled = true;
            TABLEBOX.Visibility = System.Windows.Visibility.Visible;
            TABLETEXT.Visibility = System.Windows.Visibility.Visible;
            OPTIONTEXT.Visibility = System.Windows.Visibility.Visible;
            OPTIONBOX.Visibility = System.Windows.Visibility.Visible;
            OPTIONBOX.Visibility = System.Windows.Visibility.Visible;
            foreach (string element in tables)
            {
                if (element == "Error")
                {
                    INFOBOX.Text = "Error beim abfragen der Tables";
                    return;
                }

                TABLEBOX.Items.Add(element);
            }

            
        }

        private void TEST_Click(object sender, RoutedEventArgs e)
        {
            Verbindung.test(TEST);
        }

        private void disconnect_click(object sender, RoutedEventArgs e)
        {
            Verbindung.trennen(SCHEMABOX, SCHEMATEXT, Abfrage, VERBINDEN, DISCONNECT, PORT, USER, PASSWORD,QUERY);
            Verbindung.changevisibilty(OPTIONBOX, SCHEMABOX, TABLEBOX, SEARCHBOX, COLUMBOX, OPTIONTEXT, SCHEMATEXT, SEARCHTEXT, COLUMTEXT,TABLETEXT);
        }

        private void TABLES_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            COLUMBOX.Items.Clear();
            List<string> colums = Verbindung.GetColumValues(IP, PORT, SCHEMABOX, USER, PASSWORD, TABLEBOX);
            foreach (var element in colums)
            {
                if (element == "Error")
                {
                    INFOBOX.Text = "Error: Could not get any Colums";
                    return;
                }

                COLUMBOX.Items.Add(element);
                COLUMBOX.IsEnabled = true;
                COLUMBOX.Visibility = System.Windows.Visibility.Visible;
                COLUMTEXT.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void COLUMBOX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SEARCHTEXT.Visibility = System.Windows.Visibility.Visible;
            SEARCHBOX.Visibility = System.Windows.Visibility.Visible;
        }

    }
}

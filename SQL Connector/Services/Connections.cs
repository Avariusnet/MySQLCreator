using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;
using SQL_Connector;
using System.Windows.Controls;

namespace SQLCreator {

    class Verbindung {

        private string _port;
        private string _ip;
        private string _password;
        private string _user;

        public string Port { get => _port; set => _port = value; }
        public string Ip { get => _ip; set => _ip = value; }
        public string Password { get => _password; set => _password = value; }
        public string User { get => _user; set => _user = value; }


        public static bool Ping(string ip)
        {
            try
            {
                string ipadr = ip;

                IPHostEntry hostEntry;

                hostEntry = Dns.GetHostEntry(ipadr);

                //you might get more than one ip for a hostname since 
                //DNS supports more than one record

                if (hostEntry.AddressList.Length > 0)
                {
                    var newip = hostEntry.AddressList[0];
                    Ping Pingsender = new Ping();
                    // IPAddress adr = IPAddress.Parse(ip);
                    PingReply reply = Pingsender.Send(newip);

                    if (reply.Status == IPStatus.Success)
                    {
                        Logger.Logs("Verbindung konnte erfolgreich geprüft werden.");
                        return true;

                    }

                    return false;

                }

                return false;


            }

            catch
            {
                Logger.Logs("Verbindung konnte nicht erfolgreich hergestellt werden.");
                return false;
            }
        }
        public static string CreateQueryStr(string server, string port, Object databaseName, string user, string pass)
        {
            //build the connection string
            string connStr = "server=" + server + ";port=" + port + ";database=" + databaseName + ";uid=" +
                user + ";password=" + pass + ";";

            //return the connection string
            return connStr;
        }

        public static string CreateDBStr(string server, string port, string user, string pass)
        {
            string connStr = "server=" + server + ";port=" + port + ";uid=" +
                user + ";password=" + pass + ";";

            return connStr;
        }

        public static string CreateSpecificDBStr(string server, string port, string db, string user, string pass)
        {
            string connStr = "server=" + server + ";port= " + port + ";database= " + db + ";uid=" +
                user + ";password=" + pass + ";";


            return connStr;
        }

        public static string CreateTestStr(string server, string port, string user, string pass)
        {

            string connStr = "server=" + server + ";port= " + port + ";uid=" +
                user + ";password=" + pass + ";";


            return connStr;
        }

        /// <summary>
        /// Select all Schemainformations from DB.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static List<string> SelectAllSchemas(string ip, string port, string user, string pass)
        {
            List<string> list = new List<string>();
            try
            {


                MySqlConnection con = new MySqlConnection(CreateDBStr(ip, port, user, pass));
                MySqlCommand cmd = con.CreateCommand();
                string query = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA";
                cmd.CommandText = query;
                MySqlDataReader rdr;

                con.Open();

                rdr = cmd.ExecuteReader();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr.GetValue(i).ToString());
                    }


                }
                Console.WriteLine("Found:" + list.Count);
                Logger.Logs("Downloaded all Schema Informations.");

                rdr.Close();
                return list;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                list.Add("Error");
                return list;
            }


        }


        public static List<string> SelectAllTables(TextBox IP, TextBox PORT, TextBox USER, PasswordBox PASSWORD, ComboBox SCHEMA)
        {
            List<string> list = new List<string>();
            try
            {
                string ip = IP.Text;
                string port = PORT.Text;
                string user = USER.Text;
                string pass = PASSWORD.Password;
                Object dbo = SCHEMA.SelectedItem;
                string db = dbo.ToString();

                MySqlConnection con = new MySqlConnection(CreateSpecificDBStr(ip, port, db, user, pass));
                MySqlCommand cmd = con.CreateCommand();
                string query = "SHOW TABLES;";
                cmd.CommandText = query;

                MySqlDataReader reader;
                con.Open();

                reader = cmd.ExecuteReader();

                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetValue(i).ToString());
                    }
                }
                Logger.Logs("Tables wurden abgefragt.");
                reader.Close();
                return list;
            }


            catch (Exception ex)
            {
                list.Add("Error");
                Console.WriteLine(ex.Message);
                Logger.Logs("Error beim abfragen der Tables");
                Logger.Logs(ex.Message);
                return list;
            }

        }

        public static bool ConnectionTest(string ip, string port, string user, string pass)
        {

            Verbindung verbindung = new Verbindung();

            try
            {
                MySqlConnection connection = new MySqlConnection(Verbindung.CreateTestStr(ip, port, user, pass));
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public static void Disconnect(ComboBox SCHEMA, TextBlock SCHEMATEXT, Button Abfrage, Button VERBINDEN, Button DISCONNECT, TextBox PORT, TextBox USER, PasswordBox PASSWORD, TextBox QUERY)
        {
            VERBINDEN.IsEnabled = true;
            DISCONNECT.IsEnabled = false;

            PORT.IsEnabled = false;
            USER.IsEnabled = false;
            PASSWORD.IsEnabled = false;
            QUERY.IsEnabled = false;
            SCHEMA.Visibility = System.Windows.Visibility.Hidden;
            Abfrage.Visibility = System.Windows.Visibility.Hidden;
            SCHEMATEXT.Visibility = System.Windows.Visibility.Hidden;
        }

        public static void ChangeVisibility(ComboBox OPTIONBOX, ComboBox SCHEMABOX, ComboBox TABLEBOX, TextBox SEARCHPATTERN, ComboBox COLUMBOX, TextBlock OPTIONTEXT, TextBlock SCHEMATEXT, TextBlock SEARCHTEXT, TextBlock COLUMTEXT, TextBlock TABLETEXT)
        {
            OPTIONBOX.Visibility = System.Windows.Visibility.Hidden;
            SCHEMABOX.Visibility = System.Windows.Visibility.Hidden;
            TABLEBOX.Visibility = System.Windows.Visibility.Hidden;
            SEARCHPATTERN.Visibility = System.Windows.Visibility.Hidden;
            COLUMBOX.Visibility = System.Windows.Visibility.Hidden;
            OPTIONBOX.Visibility = System.Windows.Visibility.Hidden;
            SCHEMATEXT.Visibility = System.Windows.Visibility.Hidden;
            SEARCHTEXT.Visibility = System.Windows.Visibility.Hidden;
            OPTIONTEXT.Visibility = System.Windows.Visibility.Hidden;
            COLUMTEXT.Visibility = System.Windows.Visibility.Hidden;
            TABLETEXT.Visibility = System.Windows.Visibility.Hidden;
        }

        public static List<string> GetColumValues(TextBox IP, TextBox PORT, ComboBox SCHEMA, TextBox USER, PasswordBox PASSWORD, ComboBox TABLES)
        {

            List<string> list = new List<string>();
            string ip = IP.Text;
            string port = PORT.Text;
            string user = USER.Text;
            string pass = PASSWORD.Password;
            Object dbo = SCHEMA.SelectedItem;
            string db = dbo.ToString();



            Verbindung verbindung = new Verbindung();
            MySqlConnection connection = new MySqlConnection(Verbindung.CreateQueryStr(ip, port, db, user, pass));
            MySqlCommand cmd = connection.CreateCommand();
            string query = "Show columns from " + TABLES.SelectedItem.ToString() + " from " + SCHEMA.SelectedItem.ToString();
            cmd.CommandText = query;
            MySqlDataReader rdr;

            connection.Open();
            try
            {
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; ++i)
                    {
                        if (rdr.GetName(i).ToString() == "Field")
                        {
                            list.Add(rdr.GetValue(i).ToString());
                        }

                    }

                }
                rdr.Close();
                return list;
            }

            catch
            {
                list.Add("Error");
                Logger.Logs("Error in Columquery");
                return list;
            }
        }

        public static List<string> GetQueryValues(TextBox IP, TextBox PORT, ComboBox SCHEMABOX, TextBox USER, PasswordBox PASSWORD, TextBox QUERY)
        {


            string ip = IP.Text;
            string port = PORT.Text;
            string user = USER.Text;
            string pass = PASSWORD.Password;
            Object dbo = SCHEMABOX.SelectedItem;
            string query = QUERY.Text;
            string db = dbo.ToString();

            List<string> stringpruefung = new List<string>
            {
                "name",
                "subname",
                "ScriptName",
                "AIName",
                "IconName",
                "MaleText",
                "FemaleText",
                "Name",
                "Subject",
                "comment"
            };

            int count = 0;
            string merge = "";
            string data = "";
            List<string> list = new List<string>();


            Verbindung verbindung = new Verbindung();
            MySqlConnection connection = new MySqlConnection(Verbindung.CreateQueryStr(ip, port, db, user, pass));
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            MySqlDataReader rdr;

            connection.Open();
            try
            {
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {

                        Console.WriteLine(rdr[i].ToString());
                        string lueckenfueller = "";
                        string fullstring = rdr[i].ToString();
                        char trenner = ',';

                        String[] substrings = fullstring.Split(trenner);
                        merge = "";
                        count = 0;

                        for (int o = 0; o < substrings.Length; ++o)
                        {
                            ++count;
                            merge += substrings[o];
                        }

                        if (substrings.Length > 1)
                        {
                            merge = substrings[0] + "." + substrings[1];
                        }


                        //Wenn string splitcount = 2 data änderung
                        if (count == 2)
                        {
                            data = "/*" + rdr.GetName(i).ToString() + ": */ " + merge + ", \n";
                        }

                        //wenn eintrag ist letztes feld.
                        else if (i == rdr.FieldCount - 1)
                        {
                            data = "/*" + rdr.GetName(i).ToString() + ": */ " + rdr[i].ToString() + "\n";
                        }

                        //alle anderen einträge
                        else
                        {
                            data = "/*" + rdr.GetName(i).ToString() + ": */ " + rdr[i].ToString() + ", \n";
                        }

                        //stringprüfung für felder mit string werten.
                        foreach (var element in stringpruefung)
                        {
                            if (element == rdr.GetName(i).ToString() && i == rdr.FieldCount - 1)
                            {
                                data = "/*" + rdr.GetName(i).ToString() + ": */ \" " + rdr[i].ToString() + " \" \n";
                            }

                            else if (element == rdr.GetName(i).ToString())
                            {
                                data = "/*" + rdr.GetName(i).ToString() + ": */ \" " + rdr[i].ToString() + " \", \n";
                            }
                        }

                        //wenn feld einen leeren wert zurückgibt wird der hier ersetzt.
                        if (rdr[i].ToString() == "")
                        {
                            if (i == rdr.FieldCount - 1)
                            {
                                lueckenfueller = "0";
                                data = "/*" + rdr.GetName(i).ToString() + ": */ \" " + lueckenfueller + " \" \n";
                            }

                            else
                            {
                                lueckenfueller = "0";
                                data = "/*" + rdr.GetName(i).ToString() + ": */ \" " + lueckenfueller + " \", \n";
                            }
                        }

                        list.Add(data);
                    }
                }

                connection.Close();
                return list;
            }

            catch (Exception ex)
            {
                list.Add("Error");
                Logger.Logs(ex.Message);
                list.Add("Wrong SQL-Syntax. Visit Log for further Information");
                return list;
            }
        }


        public static List<string> ColumnNames(TextBox IP, TextBox PORT, ComboBox SCHEMA, TextBox USER, PasswordBox PASSWORD, TextBox QUERY)
        {

            string ip = IP.Text;
            string port = PORT.Text;
            string user = USER.Text;
            string pass = PASSWORD.Password;
            Object dbo = SCHEMA.SelectedItem;
            string query = QUERY.Text;
            string db = dbo.ToString();

            List<string> list = new List<string>();
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            Verbindung verbindung = new Verbindung();
            MySqlConnection connection = new MySqlConnection(Verbindung.CreateQueryStr(ip, port, db, user, pass));
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            MySqlDataReader rdr;

            connection.Open();
            try
            {
                rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        list.Add(rdr.GetName(i).ToString());
                    }


                }

                connection.Close();
                return list;
            }

            catch (Exception ex)
            {

                Console.WriteLine("Error in List.");
                Logger.Logs(ex.Message);
                list.Add("Wrong SQL-Syntax. Visit Log for further Information");
                return list;
            }

        }


    }

}
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;
using Logging;
using SQL_Connector;
using System.IO;
using System.Linq;
using System.Text;
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


namespace Export
{
    class Export
    {

        public static bool exportsql(string data, string filename, ComboBox SCHEMA, List<string> spaltennamen, ComboBox OPTION)
        {
            string beginstring = "";
            string headerstring = "";
            string valuestring = "";
            string completestring = "";
            Object option = OPTION;
            string sooption = option.ToString();
            List<string> Liste = spaltennamen; 
            string dbnew = "";
            string db = SCHEMA.SelectedIndex.ToString();

            if (String.IsNullOrEmpty(data))
            {
                Logging.Logger.Logs("Empty Resultfield was given!");
                return false;
            }
            


            try
            {
                string fullstring = db;
                char trenner = ' ';

                String[] substrings = fullstring.Split(trenner);

                for(int i = 0; i < substrings.Length; ++i)
                {
                    if(substrings[i] == "from" ||substrings[i] == "FROM")
                    {
                       dbnew = substrings[i + 1];
                    }
                }

                Console.WriteLine(dbnew);
                
                
                string filenames = filename + ".sql";
                StreamWriter sw = new StreamWriter(filenames);
                beginstring = option + " INTO " + dbnew + "( ";
                for (int i = 0; i < Liste.Count; ++i)
                {
                    if(i == Liste.Count - 1)
                    {
                        headerstring += "`" + Liste[i] + "` ";
                    }
                    else
                    {
                        headerstring += "`" + Liste[i] + "`, ";
                    }
                   
                }


                valuestring = " ) VALUES ( \n " + data + " );";

                completestring = beginstring + headerstring + valuestring;
                sw.WriteLine(completestring);
                sw.Close();
                Logging.Logger.Logs("SQL-File " + filenames + " was created sucessfully.");
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.Logs(ex.Message);
                return false;

            }

        }


    }
}

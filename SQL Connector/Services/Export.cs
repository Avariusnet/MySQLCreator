using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;



namespace SQLCreator {
    class Export
    {

        public static bool ExportSQL(string data, string filename, ComboBox SCHEMA, List<string> spaltennamen, ComboBox OPTION)
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
                Logger.Logs("Empty Resultfield was given!");
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
                Logger.Logs("SQL-File " + filenames + " was created sucessfully.");
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

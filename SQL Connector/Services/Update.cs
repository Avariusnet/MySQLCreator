using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SQLCreator {
    class Update
    {
        public static int update(string programmversion)
        {
            string url = "http://88.198.120.178/sqlconnector/version.txt";
            
            List<string> list = new List<string>();
            try
            {
               

                string source = "version.txt";
                string adress = "http://88.198.120.178/sqlconnector/version.txt";
                

                WebClient web = new WebClient();
                web.DownloadFile(adress, source);

                try
                {
                    WebRequest request = WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    response.Close();
                }

                catch
                {
                    Logger.Logs("Updateserver not reachable.");

                    return 400;
                }

                StreamReader sr = new StreamReader("version.txt");

                if (!File.Exists("version.txt"))
                {
                    File.Delete("version.txt");
                }

                string completestring = sr.ReadToEnd();
                char trennzeichen = ',';
                String[] substrings = completestring.Split(trennzeichen);
                Console.WriteLine(substrings[0]);
                Console.WriteLine(substrings[1]);

                Console.WriteLine(completestring);
                sr.Close();
                // int number = substrings[0].Length 1;        
                
                if(substrings[0] == programmversion)
                {
                    Console.WriteLine("Kein Update nötig");
                    Logger.Logs("Kein Update benötigt.");
                    return 202;
                }

                else
                {
                    string progaddress = "http://88.198.120.178/sqlconnector/connector.rar";
                    string progsource = "connector.rar";
                    Console.WriteLine("Update available.");
                    Logger.Logs("Update available");
                    WebClient newversion = new WebClient();
                    newversion.DownloadFile(progaddress ,progsource);
                    return 200;
                }
                
            }

            catch (Exception ex)
            {
                Logger.Logs("Update failed!");
                Console.WriteLine(ex.Message);
                Logger.Logs(ex.Message);
                return 400;
            }

        }


        public static string updatemeldung(int updatezustand)
        {
            if (updatezustand == 400)
            {
                return "Error. Updateserver kann nicht erreicht werden.";
            }

            if (updatezustand == 200)
            {
                return "Update available. Files are already downloaded in your execution folder.";
            }

            if (updatezustand == 202)
            {
                return "No Update available";
            }

            else
            {
                return "Failure!";
            }

        }

    }

}

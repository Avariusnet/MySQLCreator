using System;
using Data;
using System.IO;


namespace Logging
{
    
    class Logger
    {
        
        public static bool Logs (string message)
        {
            string filename = "Log.txt";
            StreamWriter sw = new StreamWriter(filename,true);
                      
            try
            {
                sw.Write(DateTime.Now + " ");
                sw.WriteLine(message);
                sw.Close();
            }

            catch (Exception ex)
            {
                sw.Write(DateTime.Now);
                sw.WriteLine(ex.Message);
                sw.Close();
            }

            return true;
        }

       

    }


}
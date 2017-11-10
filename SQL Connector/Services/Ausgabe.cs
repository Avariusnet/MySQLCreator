using Data;
using Logging;
using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace Ausgabe
{
    class Ausgabe
    {
        public static List<string> ausgabe(TextBox RESULT, TextBox QUERY, Button EXPORT, ComboBox OPTIONBOX, TextBox SEARCHBOX, ComboBox SCHEMABOX, ComboBox COLUMBOX, ComboBox TABLEBOX, TextBox IP, TextBox PORT, TextBox USER, PasswordBox PASSWORD)
        {


            List<string> list = new List<string>();
            EXPORT.IsEnabled = true;
            Verbindung vbd = new Verbindung();
            try
            {
                if(SCHEMABOX.SelectedIndex == -1)
                {
                    Logger.Logs("Missing Schemainformation");
                    list.Add("Missing Schemainformation");
                    return list;
                }


                if (String.IsNullOrEmpty(QUERY.Text))
                {
                    if (OPTIONBOX.SelectedItem.ToString() == "" || SCHEMABOX.SelectedItem.ToString() == "" || TABLEBOX.SelectedItem.ToString() == "")
                    {
                        Logger.Logs("Missing Data. Please fill all required fields.");
                        list.Add("Error! Visit Log for further Information.");
                        return list;
                    }

                    string option = OPTIONBOX.SelectedItem.ToString();
                    char trenner = ' ';
                    String[] splitstrings = option.Split(trenner);
                    foreach (var element in splitstrings)
                    {
                        Console.WriteLine(element);
                    }


                    string tables = TABLEBOX.SelectedItem.ToString();
                    string optionstring = splitstrings[1].ToString();
                    string colum = COLUMBOX.SelectedItem.ToString();

                    string query = "";
                    if (String.IsNullOrEmpty(SEARCHBOX.Text)){
                        query = optionstring + " * FROM " + tables;
                    }
                    else
                    {
                        query = optionstring + " * FROM " + tables + " where " + colum + " = " + SEARCHBOX.Text + ";";
                    }
                   
                    Console.WriteLine(query);
                    QUERY.Text = query;

                    List<string> result = Verbindung.GetQueryValues(IP, PORT, SCHEMABOX, USER, PASSWORD, QUERY);


                }

                RESULT.Clear();
                RESULT.Text = "";

                List<string> ergebnis = Verbindung.GetQueryValues(IP, PORT, SCHEMABOX, USER, PASSWORD, QUERY);
                return ergebnis;


            }
            catch (Exception ex)
            {
                QUERY.Clear();
                Console.WriteLine(ex.Message);
                Logger.Logs(ex.Message);
                list.Add("Error!");
                return list;
            }

        }

    }

}
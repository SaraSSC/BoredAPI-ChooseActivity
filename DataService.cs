using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.IO;
using System.Net;

namespace SecondTryTest
{
    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        private DialogService dialogService;


        public DataService()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\SecondTry.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();
                
                //Inicialmente Activity era varchar(50) daí o erro que aparece ao correu a app
                string query = "create table if not exists Activities (Key int primary key, Activity Text, Type varchar(50), Participants int)";
                command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Error a criar a table", ex.Message);
            }

        }

        public void SaveData<T>(List<T> list)
        {
            try
            {
               
                foreach (var item in list)
                {
                    string query = string.Empty;

                    if (item is Activities)
                    {
                      
                        var activity = item as Activities;
                         query = string.Format("insert into Activities (Key, Activity, Type, Participants) values ({0}, '{1}', '{2}', {3})", activity.Key, activity.Activity, activity.Type, activity.Participants);
                        
                    }


                    command = new SQLiteCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Error a guardar os dados", ex.Message);
            }

        }

        public void GetData<T>(List<T> list)
        {
            List<Activities> activity = new List<Activities>();

            try
            {
                if (list is List<Activities>)
                {
                    string query = "select * from Activities";
                    command = new SQLiteCommand(query, connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var activities = new Activities
                        {
                            Key = int.Parse(reader["Key"].ToString()),
                            Activity = reader["ActivityName"].ToString(),
                            Type = reader["Type"].ToString(),
                            Participants = int.Parse(reader["Participants"].ToString()),
                        };

                        (list as List<Activities>).Add(activities);
                    }

               
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Error a obter os dados", ex.Message);
            }
        }

        


    }
}

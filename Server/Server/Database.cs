using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Server
{
    public class Database//База данных
    {
        public MySqlConnection connection { get; set; }

        public Database(string host, string port, string database, string username, string password)
        {
            string ihost = "37.29.78.130";
            int iport = 3311;
            string idatabase = "olhdata";
            string iusername = "admin";
            string ipassword = "030292";

            string connString = "Server=" + ihost + ";Database=" + idatabase
    + ";port=" + iport + ";User=" + iusername + ";password=" + ipassword;
            Console.WriteLine(connString);
            //Server=37.29.78.130;Database=olhdata;port=030292;User Id=admin;password=030292
            connection = new MySqlConnection(connString);
            connection.Open();
        }

        public List<Data.InfoHangar> GetAnyHangar()//Получить все ангары
        {
            
            MySqlCommand command = new MySqlCommand("SELECT * FROM hangars", connection);
            var read = command.ExecuteReader();
            var infos = new List<Data.InfoHangar>();

            int i = 0;
            while (read.Read())
            {
                int idhangar  = read.GetInt32("idhangar");
                string namehangar = read.GetString("namehangar");
                string with = read.GetString("with");
                string heignt = read.GetString("height");
                string leugth = read.GetString("length");
                string countplane = read.GetString("countplane");
                string nametable = read.GetString("nametable");

                var info = new Data.InfoHangar();
                info.IdHangar = idhangar;
                info.NameHangar = namehangar;
                info.With = with;
                info.Heignt = heignt;
                info.Length = leugth;
                info.CountPlane = countplane;
                info.NameTable = nametable;
                infos.Add(info);
            }
            read.Close();

            return infos;
        }
    }
}

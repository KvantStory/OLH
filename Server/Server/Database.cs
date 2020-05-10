using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Server
{
    public class Database//База данных
    {
        public MySqlConnection connection { get; set; }

        public Database()
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

        public List<Data.InfoPlane> GetPlanes(int id)//Получение самолёта по id по ангару
        {
            MySqlCommand command = new MySqlCommand($"SELECT nametable FROM hangars WHERE idhangar = {id}", connection);
            string nametable = command.ExecuteScalar().ToString();
            command = new MySqlCommand($"SELECT * FROM {nametable}", connection);
            MySqlDataReader read = command.ExecuteReader();
            List<Data.InfoPlane> info = new List<Data.InfoPlane>();
            List<int> ids = new List<int>();
            while (read.Read())
            {
                int idplane = read.GetInt32("idplane");

                ids.Add(idplane);
            }
            string commandstrings = "SELECT * FROM planes WHERE ";

            foreach (int i in ids)
            {
                commandstrings += $"`idplane` = {i} AND ";
            }

            command.Dispose();
            read.Close();

            foreach (int i in ids)
            {

                MySqlCommand commandi = new MySqlCommand($"SELECT * FROM planes WHERE `idplane` = {i}", connection);//LOL

                MySqlDataReader readi = commandi.ExecuteReader();
                while (readi.Read())
                {
                    int idplane = readi.GetInt32("idplane");
                    string name = readi.GetString("name");
                    DateTime starttime = readi.GetDateTime("strarttime");//LOL
                    int with = readi.GetInt32("with");
                    int height = readi.GetInt32("height");
                    DateTime finishtime = readi.GetDateTime("finishtime");
                    DateTime time = readi.GetDateTime("time");
                    int length = readi.GetInt32("length");
                    int x = readi.GetInt32("x");
                    int y = readi.GetInt32("y");
                    int planeheight = readi.GetInt32("planeheight");
                    int money = readi.GetInt32("money");
                    int onedaymoney = readi.GetInt32("onedaymoney");
                    int errormoney = readi.GetInt32("errormoney");

                    info.Add(new Data.InfoPlane(idplane, name, starttime, with, height, finishtime, time, length, x, y, planeheight, money,
                        onedaymoney, errormoney));
                }

                commandi.Dispose();
                readi.Close();
            }
            return info;

            /*
            MySqlCommand command = new MySqlCommand($"SELECT * FROM planes WHERE idplane = {id}", connection);
            MySqlDataReader read = command.ExecuteReader();
            Data.InfoPlane info = new Data.InfoPlane();

            while (read.Read())
            {
                int idplane = read.GetInt32("idplane");
                string name = read.GetString("name");
                DateTime starttime = read.GetDateTime("strarttime");//LOL
                int with = read.GetInt32("with");
                int height = read.GetInt32("height");
                DateTime finishtime = read.GetDateTime("finishtime");
                DateTime time = read.GetDateTime("time");
                int length = read.GetInt32("length");
                int x = read.GetInt32("x");
                int y = read.GetInt32("y");
                int planeheight = read.GetInt32("planeheight");
                int money = read.GetInt32("money");
                int onedaymoney = read.GetInt32("onedaymoney");
                int errormoney = read.GetInt32("errormoney");

                info = new Data.InfoPlane(idplane, name, starttime, with, height, finishtime, time, length, x, y, planeheight, money,
                    onedaymoney, errormoney);
            }
            return info;
            */
        }

        public void SetAnyTime(DateTime date)//Изменить у всех самолётов время (таблица time)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM hangars", connection);
            //TODO
        }

        public List<Data.InfoPlane> GetInfoPlanes()//Получить всю инфу о самолётах
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM hangars", connection);
            MySqlDataReader read = command.ExecuteReader();
            List<Data.InfoPlane> infos = new List<Data.InfoPlane>();

            while (read.Read())
            {
                int idplane = read.GetInt32("idplane");
                string name = read.GetString("name");
                DateTime starttime = read.GetDateTime("starttime");
                int with = read.GetInt32("with");
                int height = read.GetInt32("height");
                DateTime finishtime = read.GetDateTime("finishtime");
                DateTime time = read.GetDateTime("time");
                int length = read.GetInt32("lenght");
                int x = read.GetInt32("x");
                int y = read.GetInt32("y");
                int planeheight = read.GetInt32("planeheight");
                int money = read.GetInt32("money");
                int onedaymoney = read.GetInt32("onedaymoney");
                int errormoney = read.GetInt32("errormoney");

                infos.Add(new Data.InfoPlane(idplane, name, starttime, with, height, finishtime, time, length, x, y, planeheight, money,
                    onedaymoney, errormoney));
            }

            return infos;
        }

        public void AddHagar(string with, string height, string length, string name)//Добавить ангар
        {
            MySqlCommand command = new MySqlCommand($"INSERT INTO `hangars` (`namehangar`, `with`, `height`, `length`, `countplane`, `nametable`) VALUES ('{name}', {with}, {height}, {length}, 0, '{name}table');", connection);
            command.ExecuteNonQuery();
        }

        public List<Data.InfoHangar> GetAnyHangar()//Получить все ангары
        {

            MySqlCommand command = new MySqlCommand("SELECT * FROM hangars", connection);
            MySqlDataReader read = command.ExecuteReader();
            List<Data.InfoHangar> infos = new List<Data.InfoHangar>();

            while (read.Read())
            {
                int idhangar = read.GetInt32("idhangar");
                string namehangar = read.GetString("namehangar");
                string with = read.GetString("with");
                string heignt = read.GetString("height");
                string leugth = read.GetString("length");
                string countplane = read.GetString("countplane");
                string nametable = read.GetString("nametable");

                Data.InfoHangar info = new Data.InfoHangar();
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

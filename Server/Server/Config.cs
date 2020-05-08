using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Server
{
    public class Config//Конфиг!
    {
        //база данных
        public string Host { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public string PortDatabase { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }

        //Какой порт для сервера
        public string Port { get; set; }

        public Config()//Просто загрузка инфы
        {
            string[] settings = File.ReadAllLines(Data.ConfigFile);

            Host = settings[0];
            Password = settings[1];
            Ip = settings[2];
            PortDatabase = settings[3];
            Database = settings[4];
            Username = settings[5];
            Port = settings[6];
        }
    }
}

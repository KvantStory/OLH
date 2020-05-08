using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace Server
{
    static public class Data//Вся инфа
    {
        #region Параметры

        public const string ConfigFile = "config.ini";//Где будет конфиг файл
        public const string LogFile = "log.txt";//Где будет логи

        #endregion

        #region Сам Data

        public static List<ClientInfo> ClientsInfo { get; set; } = new List<ClientInfo>();//Инфо о клиентах
        public static TcpListener Server { get; set; }//Сервер
        public static StreamWriter Loger { get; set; } = new StreamWriter(LogFile);//Логи

        #endregion

        #region Разное

        public class ClientInfo//Инфо о клиенте
        {
            public TypeClient TypeClient { get; set; }//Тип клиента
            public string Name { get; set; }//Имя клиента
            public TcpClient TcpClient { get; set; }//Сокет

            public ClientInfo(TypeClient typeClient, string name, TcpClient tcpClient)
            {
                TypeClient = typeClient;
                Name = name;
                TcpClient = tcpClient;
            }
        }

        public enum TypeClient//Тип клиента
        {
            Admin = 0//Он добавляет все данные
        }

        #endregion
    }
}

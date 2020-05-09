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

        public const string ConfigFile = @"C:\Users\damal\Desktop\Утилиты\config.ini";//Где будет конфиг файл
        public const string LogFile = "log.txt";//Где будет логи

        #endregion

        #region Сам Data

        public static List<ClientInfo> ClientsInfo { get; set; } = new List<ClientInfo>();//Инфо о клиентах
        public static TcpListener Server { get; set; }//Сервер
        public static StreamWriter Loger { get; set; } = new StreamWriter(LogFile);//Логи

        #endregion

        #region Разное

        public class InfoHangar//Инфо о ангарах
        {
            public int IdHangar { get; set; }
            public string NameHangar { get; set; }
            public string With { get; set; }
            public string Heignt { get; set; }
            public string Length { get; set; }
            public string CountPlane { get; set; }
            public string NameTable { get; set; }
        }

        public class ClientCheck//Для проверки клиента
        {
            public bool IsDatabase { get; set; }//Он в базе данных?
            public ClientInfo ClientInfo { get; set; }//Инфа про него

            public ClientCheck(bool isdatabase, ClientInfo clientInfo)
            {
                IsDatabase = isdatabase;
                ClientInfo = clientInfo;
            }
        }

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

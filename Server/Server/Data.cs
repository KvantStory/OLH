using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Server
{
    public static class Data//Вся инфа
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

        public class InfoPlane//Инфо о самолёте
        {
            public InfoPlane(int id, string name, DateTime starttime, int with, int height, DateTime finishtime, DateTime time, int leught, int x, int y,
                int planeheignt, int money, int onedaymoney, bool haveoneplane, int errormoney)
            {
                ID = id;
                Name = name;
                StartTime = starttime;
                With = with;
                Height = height;
                FinishTime = finishtime;
                Time = time;
                Leugth = leught;
                X = x;
                Y = y;
                PlaneHeight = planeheignt;
                Money = money;
                OneDayMoney = onedaymoney;
                HaveOnePlane = haveoneplane;
                ErrorMoney = errormoney;
            }

            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime StartTime { get; set; }
            public int With { get; set; }
            public int Height { get; set; }
            public DateTime FinishTime { get; set; }
            public DateTime Time { get; set; }
            public int Leugth { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int PlaneHeight { get; set; }
            public int Money { get; set; }
            public int OneDayMoney { get; set; }
            public bool HaveOnePlane { get; set; }
            public int ErrorMoney { get; set; }
        }

        public struct InfoHangar//Инфо о ангарах
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

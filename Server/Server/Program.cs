﻿using Konsole;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private static Database database { get; set; }

        private static void Main(string[] args)//Запуск
        {
            ProgressBar prog = new ProgressBar(PbStyle.SingleLine, 5);

            prog.Refresh(1, "Загрузка логов...");
            Data.Loger.AutoFlush = true;

            prog.Refresh(2, "Загрузка конфига...");
            Config config = new Config();

            prog.Refresh(3, "Соединение с базой данной...");
            database = new Database();

            prog.Refresh(4, "Запуск сервера...");
            Data.Server = new TcpListener(IPAddress.Any, int.Parse(config.Port));
            Data.Server.Start();
            Thread thread = new Thread(new ThreadStart(ConnectingClient));
            thread.Start();

            prog.Refresh(5, "Готово!");

            

            while (true)
            {
                string answer = Console.ReadLine();

                if (answer.ToLower() == "stop")
                {
                    try
                    {
                        Functions.OffServer();
                        database.connection.Close();
                    }
                    catch { }
                }
            }
        }

        #region Прослушка клиента

        private static void SendHangars(Data.ClientInfo clientInfo)//Отправка всех ангаров (после подключение клиента)
        {
            //Получение ангаров

            System.Collections.Generic.List<Data.InfoHangar> infos = database.GetAnyHangar();

            foreach (Data.InfoHangar info in infos)
            {
                Task.Delay(1000).Wait();
                clientInfo.TcpClient.Client.Send(Encoding.UTF8.GetBytes($"RUPDP:{info.IdHangar}:{info.NameHangar}:{info.CountPlane}"));
            }
        }

        private static void ListenClient(object obj)//Сама прослушка клиента
        {
            Data.ClientInfo clientInfo = (Data.ClientInfo)obj;
            SendHangars(clientInfo);
            byte[] buffer = new byte[1024];

            while (true)
            {
                Task.Delay(10).Wait();
                int messI = clientInfo.TcpClient.Client.Receive(buffer);
                string answer = Encoding.UTF8.GetString(buffer, 0, messI);

                if (answer.Contains("ADDA"))//Добавление
                {
                    Match regex = Regex.Match(answer, "ADDA:(.*):(.*):(.*):(.*)");
                    string with = regex.Groups[1].Value;
                    string height = regex.Groups[2].Value;
                    string length = regex.Groups[3].Value;
                    string name = regex.Groups[4].Value;

                    database.AddHagar(with, height, length, name);
                    SendHangars(clientInfo);
                }
                else if (answer.Contains("UPDPOZ"))//Загрузка инфы о самолётох
                {
                    Algo algo = new Algo(database.GetInfoPlanes());
                    List<Data.InfoPlane> infos = algo.Work();
                    foreach (Data.InfoPlane i in infos)
                        clientInfo.TcpClient.Client.Send(Encoding.UTF8.GetBytes($"UPDPOZ:{i.ID}:{i.X}:{i.Y}"));
                    
                }
            }
        }

        #endregion

        #region Подключение клиента

        private static Data.ClientCheck CheckClient(TcpClient client)//Проверка клиента
        {
            //TODO
            return new Data.ClientCheck(true, new Data.ClientInfo(Data.TypeClient.Admin, "test", client));
        }

        private static void ConnectingClient()//Ловить клиентов
        {
            while (true)
            {
                Task.Delay(10).Wait();
                TcpClient client = Data.Server.AcceptTcpClient();
                Functions.WriteLine("Новый клиент подключился!", ConsoleColor.Green);

                Data.ClientCheck statusClient = CheckClient(client);
                if (statusClient.IsDatabase)
                {
                    Data.ClientInfo clientInfo = new Data.ClientInfo(statusClient.ClientInfo.TypeClient, statusClient.ClientInfo.Name, client);
                    Thread thread = new Thread(new ParameterizedThreadStart(new ParameterizedThreadStart(ListenClient)));
                    thread.Start(clientInfo);
                }
            }
        }

        #endregion
    }
}

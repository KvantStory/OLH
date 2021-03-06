﻿using Konsole;
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

        #region Прочее

        private static void Main(string[] args)//Запуск
        {
            Console.Title = "Server";
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

            thread = new Thread(new ThreadStart(TikTak));
            thread.Start();

            prog.Refresh(5, "Готово!");
            Console.Beep();

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
                else if (answer.ToLower() == "a")
                {
                    Algo.point[] stp = new Algo.point[1];
                    stp[0].w = 0;
                    stp[0].l = 0;

                    Algo.masSamReady = new Algo.sam[0];

                    Algo.sum = 0;
                    Algo.masSam = new Algo.sam[2];
                    Algo.newSum = 0;
                    Algo.sort(Algo.masSam.Length, stp, Algo.masSamReady, 1000000, Algo.sum);
                    foreach (Algo.sam i in Algo.masSamReady)
                        Console.WriteLine(i.id);
                }
            }
        }

        private static void TikTak()//Чтобы считать каждый день
        {
            Data.SetTime = DateTime.Now;
            while (true)
            {
                Task.Delay(1000).Wait();

                if (DateTime.Now.Day != Data.SetTime.Day)
                {
                    //Изменение времени во всех самолётах


                }
            }
        }

        #endregion

        #region Прослушка клиента

        private static void SendHangars(Data.ClientInfo clientInfo)//Отправка всех ангаров (после подключение клиента)
        {
            //Получение ангаров

            System.Collections.Generic.List<Data.InfoHangar> infos = database.GetAnyHangar();

            foreach (Data.InfoHangar info in infos)
            {
                Task.Delay(1000).Wait();
                clientInfo.TcpClient.Client.Send(Encoding.UTF8.GetBytes($"RUPDA:{info.IdHangar}:{info.NameHangar}:{info.CountPlane}:{info.Length}:{info.With}"));
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
                Console.WriteLine(answer);
                Functions.WriteLine($"Новое подключение от {clientInfo.TcpClient.Client.RemoteEndPoint}", ConsoleColor.Green);

                if (answer.Contains("ADDA"))//Добавление
                {
                    try
                    {
                        Match regex = Regex.Match(answer, "ADDA:(.*):(.*):(.*):(.*)");
                        string with = regex.Groups[1].Value;
                        string height = regex.Groups[2].Value;
                        string length = regex.Groups[3].Value;
                        string name = regex.Groups[4].Value;

                        database.AddHagar(with, height, length, name);
                        SendHangars(clientInfo);
                    }
                    catch (Exception ex)
                    {
                        Functions.WriteLine($"ERROR ADDA: {ex.Message}", ConsoleColor.Red);
                    }
                }
                else if (answer.Contains("RUPDP"))//Загрузка инфы о самолётах
                {
                    try
                    {
                        
                        List<Data.InfoPlane> infos = database.GetInfoPlanes();
                        foreach (Data.InfoPlane i in infos)
                        {
                            //clientInfo.TcpClient.Client.Send(Encoding.UTF8.GetBytes($"UPDPOZ:{i.ID}:{i.X}:{i.Y}"));

                            //clientInfo.TcpClient.Client.Send(Encoding.UTF8.GetBytes($"UPDPOZ:{i.ID}:{i.X}:{i.Y}:{i.Height}:{i.Leugth}:{i.Money}:{i.Name}:{i.OneDayMoney}:{i.PlaneHeight}:" +
                            //    $"{i.StartTime}:{i.Time}:{i.With}:{i.ErrorMoney}:{i.FinishTime}"));

                            clientInfo.TcpClient.Client.Send(Encoding.UTF8.GetBytes($"RUPDP:{i.ID}:{i.Name}:{i.X}:{i.Y}:{i.StartTime.Date}:{i.FinishTime.Date}:" +
                                $"{i.Time.Date}:{i.Leugth}:{i.With}:{i.Money}:{i.OneDayMoney}:{i.ErrorMoney}"));
                        }

                        Functions.WriteLine($"RUPDP от {clientInfo.TcpClient.Client.RemoteEndPoint}", ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        Functions.WriteLine($"ERROR RUPDP: {ex.Message}", ConsoleColor.Red);
                    }
                }
                else if (answer.Contains("UPDA"))//Получение всех ангаров
                {
                    try
                    {
                        SendHangars(clientInfo);
                        Functions.WriteLine($"UPDA от {clientInfo.TcpClient.Client.RemoteEndPoint}", ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        Functions.WriteLine($"ERROR UPDA: {ex.Message}", ConsoleColor.Red);
                    }
                }
                else if (answer.Contains("UPDP"))//Получение данных о самолёте по id ангара
                {
                    try
                    {
                        List<Data.InfoPlane> planes = database.GetPlanes(int.Parse(answer.Substring(5)));//Лютая херня!!!

                        foreach (Data.InfoPlane plane in planes)
                        {
                            Task.Delay(800).Wait();
                            clientInfo.TcpClient.Client.Send(Encoding.UTF8.GetBytes($"RUPDP;{plane.ID};{plane.Name};{plane.X};{plane.Y};{plane.StartTime.Date};{plane.FinishTime.Date};" +
                                    $"{plane.Time.Date};{plane.Leugth};{plane.With};{plane.Money};{plane.OneDayMoney};{plane.ErrorMoney}"));
                        }
                        Functions.WriteLine($"UPDP от {clientInfo.TcpClient.Client.RemoteEndPoint}", ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        Functions.WriteLine($"ERROR UPDP: {ex.Message}", ConsoleColor.Red);
                    }
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

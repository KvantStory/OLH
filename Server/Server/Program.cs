using System;
using Konsole;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static Database database { get; set; }

        static void Main(string[] args)//Запуск
        {
            var prog = new ProgressBar(PbStyle.SingleLine, 5);

            prog.Refresh(1, "Загрузка логов...");
            Data.Loger.AutoFlush = true;

            prog.Refresh(2, "Загрузка конфига...");
            var config = new Config();

            prog.Refresh(3, "Соединение с базой данное...");
            database = new Database(config.Host, config.Password, config.Database, config.Username, config.Password);

            prog.Refresh(4, "Запуск сервера...");
            Data.Server = new TcpListener(IPAddress.Any, int.Parse(config.Port));
            Data.Server.Start();

            prog.Refresh(5, "Готово!");
        }

        #region Подключение клиента

        static void ConnectingClient()//Ловить клиентов
        {
            
        }

        #endregion
    }
}

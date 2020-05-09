using System;
using Konsole;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using Org.BouncyCastle.Asn1.Anssi;
using System.Text;

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

            prog.Refresh(3, "Соединение с базой данной...");
            database = new Database(config.Host, config.Password, config.Database, config.Username, config.Password);

            prog.Refresh(4, "Запуск сервера...");
            Data.Server = new TcpListener(IPAddress.Any, int.Parse(config.Port));
            Data.Server.Start();
            Thread thread = new Thread(new ThreadStart(ConnectingClient));
            thread.Start();

            prog.Refresh(5, "Готово!");

            database.GetAnyHangar();

            while (true)
            {
                string answer = Console.ReadLine();

                if (answer.ToLower() == "stop")
                {
                    Functions.OffServer();
                }
            }
        }

        #region Прослушка клиента

        static void SendHangars(Data.ClientInfo clientInfo)//Отправка всех ангаров (после подключение клиента)
        {
            //Получение ангаров

            var infos = database.GetAnyHangar();

            foreach (Data.InfoHangar info in infos)
            {
                clientInfo.TcpClient.Client.Send(Encoding.UTF8.GetBytes($"RUPDP:{info.IdHangar}:{info.NameHangar}:{info.CountPlane}"));
            }
        }

        static void ListenClient(object obj)//Сама прослушка клиента
        {
            var clientInfo = (Data.ClientInfo)obj;
            SendHangars(clientInfo);

            while (true)
            {
                Task.Delay(10).Wait();


            }
        }

        #endregion

        #region Подключение клиента

        static Data.ClientCheck CheckClient(TcpClient client)//Проверка клиента
        {
            //TODO
            return new Data.ClientCheck(true, new Data.ClientInfo(Data.TypeClient.Admin, "test", client));
        }

        static void ConnectingClient()//Ловить клиентов
        {
            while (true)
            {
                Task.Delay(10).Wait();
                TcpClient client = Data.Server.AcceptTcpClient();
                Functions.WriteLine("Новый клиент подключился!", ConsoleColor.Green);

                var statusClient = CheckClient(client);
                if (statusClient.IsDatabase)
                {
                    var clientInfo = new Data.ClientInfo(statusClient.ClientInfo.TypeClient, statusClient.ClientInfo.Name, client);
                    Thread thread = new Thread(new ParameterizedThreadStart(new ParameterizedThreadStart(ListenClient)));
                    thread.Start(clientInfo);
                }
            }
        }

        #endregion
    }
}

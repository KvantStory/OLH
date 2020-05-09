using System;

namespace Server
{
    public static class Functions//Вот тут функции!
    {
        #region Для сервера

        public static void OffServer()//Отключить сервер
        {
            WriteLine("Отключение всех клиентов...", ConsoleColor.Yellow);
            foreach (Data.ClientInfo client in Data.ClientsInfo)
            {
                client.TcpClient.Close();
                WriteLine($"Клиент {client.Name} отключен!", ConsoleColor.Green);
            }
            Data.Server.Stop();
            Environment.Exit(0);
        }

        #endregion

        #region Дизайн

        public static void WriteLine(string text, ConsoleColor color)//Писать
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            //TODO: Логи
        }

        public static string ReadLine(string text, ConsoleColor color)//Получать
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            string answer = Console.ReadLine();
            Console.ResetColor();
            return answer;
            //TODO: Логи
        }

        #endregion
    }
}

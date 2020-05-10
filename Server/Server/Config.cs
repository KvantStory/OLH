namespace Server
{
    public class Config//Конфиг!
    {
        //база данных
        public string Host { get; set; } = "37.29.78.130";
        public string Password { get; set; } = "030292";
        public string Ip { get; set; }
        public string PortDatabase { get; set; } = "3311";
        public string Database { get; set; } = "olhdata";
        public string Username { get; set; } = "admin";

        //Какой порт для сервера
        public string Port { get; set; } = "2845";

        public Config()//Просто загрузка инфы
        {
            //Надо было узнать Json
        }
    }
}

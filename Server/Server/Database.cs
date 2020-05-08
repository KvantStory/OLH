using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Server
{
    public class Database//База данных
    {
        public MySqlConnection connection { get; set; }

        public Database(string host, string port, string database, string username, string password)
        {
            string connString = "Server=" + host + ";Database=" + database
    + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);
        }
    }
}

using MySql.Data.MySqlClient;
using System;

namespace SeaBattleServer
{
    static class Auth
    {
        static MySqlConnection conn;
        static Auth()
        {
            conn = new MySqlConnection(
                new MySqlConnectionStringBuilder
                {
                    Server = "127.0.0.1",
                    Database = "chat",
                    UserID = "chat",
                    Password = "qwerty"
                }.ToString());
            try
            {
                conn.Open();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {

            }
        }

        internal static object Check(string login, string pwd)
        {
            if (login == "admin" && pwd == "admin")
                return login;
            else
                return null;
        }
    }
}

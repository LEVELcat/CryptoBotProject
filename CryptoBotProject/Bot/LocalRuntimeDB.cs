using CryptoBotProject.Bot.Windows.ManagersWindow;
using CryptoBotProject.WebParse;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CryptoBotProject.Bot
{
    internal class LocalRuntimeDB
    {
        private static LocalRuntimeDB instance;
        public static LocalRuntimeDB Instance
        {
            get
            {
                instance ??= new LocalRuntimeDB();
                return instance;
            }
        }

        private LocalRuntimeDB()
        {

        }

        public void ExecuteNonQuaryCommand(string procedureName, params (string, object)[] parameters)
        {
            MySqlConnection connection = GetDBConnection();
            connection.Open();

            BuildMySqlCommand(procedureName, parameters).ExecuteNonQuery();
        }
        public void ExecuteReaderCommand(out MySqlDataReader dataReader, string procedureName, params (string, object)[] parameters)
        {
            MySqlConnection connection = GetDBConnection();
            connection.Open();

            dataReader = BuildMySqlCommand(procedureName, parameters).ExecuteReader();
        }

        MySqlConnection GetDBConnection(string host = "localhost", int port = 3306, string database = "brokebd", string username = "root", string password = "")
        {
            String connString = $"Server={host};Database={database};port={port};User Id={username};password={password}";
            return new MySqlConnection(connString);
        }
        private static MySqlCommand BuildMySqlCommand(string procedureName, params (string, object)[] parameters)
        {
            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = procedureName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            foreach(var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Item1, param.Item2);
                cmd.Parameters[param.Item1].Direction = System.Data.ParameterDirection.Input;
            }
            return cmd;
        }

        public static async Task<bool> CheckModerRight(string userName)
        {
            bool result = false;

            LocalRuntimeDB.Instance.ExecuteReaderCommand(out MySqlDataReader dataReader,
            "CheckModerator",
                                            ("Username", userName)
                                            );
            var check = bool.TryParse(dataReader["IsModer"].ToString(), out bool res);
            dataReader.DisposeAsync();
            if (check && res)
            {
                result = true;
            }
            return result;
        }
    }
}

using CryptoBotProject.WebParse;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        { }

        public bool GetData( )
        {

        }

        public static MySqlCommand BuildMySqlCommand(string ,)
        {
            MySqlCommand cmd = new MySqlCommand();





        }


    }
}

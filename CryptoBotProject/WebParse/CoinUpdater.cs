using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBotProject.WebParse
{
    sealed partial class CoinUpdater
    {
        private const string URL = "https://api.cryptorank.io/v1/currencies";
        private const string baseUrlParamerters = "?api_key=";
        private string token = "";

        private static CoinUpdater instance;
        public static CoinUpdater Instance
        {
            get
            {
                instance ??= new CoinUpdater();
                return instance;
            }
        }

        delegate void TaskForCycle();
        TaskForCycle taskForCycle;

        private CoinUpdater()
        {

        }

        public static void Start(string token = "34581206d1c2f1e493c83194dc562b5de5c859ad5bb6587f7046fd6a0f26") //TODO: REMOVE TOKEN
        {
            Console.WriteLine("CoinUpdater Start");

            Instance.token = token;
            Instance.Start();
        }

        private void Start()
        {
            taskForCycle = DBUpdateCycle;
            Cycle();
        }


        public static void Stop()
        {
            Instance.taskForCycle = null;
            Instance.token = string.Empty;
            Console.WriteLine("CoinUpdater Stop");
        }

        private async void Cycle()
        {
            while(taskForCycle != null)
            {
                Task clock = Task.Delay(TimeSpan.FromSeconds(10));

                taskForCycle?.Invoke();

                await clock;
            }
        }

        public async void DBUpdateCycle()
        {
            SendToDb();
        }

        public string GetJsonFromAPI(string query = "")
        {
            string result = String.Empty;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(baseUrlParamerters + token + query).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
            return result;
        }

        public void SendToDb()
        {
            string str = GetJsonFromAPI(); //директория прописана относительно исполнительного файла

            List<APIData> list = APIData.ConvertJsonToAPIDatas(str);

            MySqlConnection connection = (new DBMySQLUtils()).GetDBConnection();

            connection.Open();

            foreach (var data in list)
            {
                MySqlCommand command = GenerateCommandForItemAlt(data);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

#if DEBUG
        private static string ReadTXTFileToString(string path) //Читает файл и переводит в строку
        {
            StreamReader streamReader = new StreamReader(path);
            var str = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
            return str;
        }
#endif

        MySqlCommand GenerateCommandForItemAlt(APIData data)// Альтернативная Функция генерирующая комманду для исполнения БДшкой под каждый item
        {
            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = "ImportCurencieData";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@curId", data.id);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curRank", data.rank);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curSlug", data.slug);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curSymbol", data.symbol);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curName", data.name);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curType", data.type);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curCategory", data.category);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curPrice", data.price);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curLow24h", data.low24h);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curHigh24h", data.high24h);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curVolume24h", data.volume24h);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curPercentChange24h", data.percentChange24h);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curPercentChange7d", data.percentChange7d);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curPercentChange30d", data.percentChange30d);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curPercentChange3m", data.percentChange3m);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curPercentChange6m", data.percentChange6m);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curImage16x16", data.image16x16);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curImage60x60", data.image60x60);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@curImage200x200", data.image200x200);
            cmd.Parameters["@curId"].Direction = System.Data.ParameterDirection.Input;

            return cmd;
        }

        class DBMySQLUtils //хуерга для подключения к твоей бд, как в примере твоего кода
        {
            public MySqlConnection GetDBConnection(string host = "localhost", int port = 3306, string database = "brokebd", string username = "root", string password = "Hupihi48chiz666")
            {
                String connString = $"Server={host};Database={database};port={port};User Id={username};password={password}";
                return new MySqlConnection(connString);
            }
        }
    }
}

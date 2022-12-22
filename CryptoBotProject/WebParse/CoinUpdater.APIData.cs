using System.Text.Json;

namespace CryptoBotProject.WebParse
{
    sealed partial class CoinUpdater
    {
        public struct APIData //класс хранящий распарсенные данные
        {
            public int id;
            public int rank;
            public string slug;
            public string symbol;
            public string name;
            public string type;
            public string? category;
            public float price;
            public float high24h;
            public float low24h;
            public float volume24h;
            public float? percentChange24h = null;
            public float? percentChange7d = null;
            public float? percentChange30d = null;
            public float? percentChange3m = null;
            public float? percentChange6m = null;
            public string image16x16;
            public string image60x60;
            public string image200x200;

            public APIData(int id, int rank, string slug, string symbol, string name, string type,
                           string category, float price, float high24h, float low24h, float volume24h,
                           float? percentChange24h, float? percentChange7d, float? percentChange30d,
                           float? percentChange3m, float? percentChange6m, string image16x16,
                           string image60x60, string image200x200)
            {
                this.id = id;
                this.rank = rank;
                this.slug = slug;
                this.symbol = symbol;
                this.name = name;
                this.type = type;
                this.category = category;
                this.price = price;
                this.high24h = high24h;
                this.low24h = low24h;
                this.volume24h = volume24h;
                this.percentChange24h = percentChange24h;
                this.percentChange7d = percentChange7d;
                this.percentChange30d = percentChange30d;
                this.percentChange3m = percentChange3m;
                this.percentChange6m = percentChange6m;
                this.image16x16 = image16x16;
                this.image60x60 = image60x60;
                this.image200x200 = image200x200;
            }

            public static List<APIData> ConvertJsonToAPIDatas(string json) //парсер
            {
                if (json == null || json == String.Empty) return null;

                List<APIData> result = new List<APIData>();

                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    var data = document.RootElement.GetProperty("data");

                    for (int index = 0; index < data.GetArrayLength(); index++)
                    {
                        APIData toAdd = new APIData();

                        var item = data[index];
                        var usd = item.GetProperty("values").GetProperty("USD");
                        var images = item.GetProperty("images");
                        JsonElement buf;

                        toAdd.id = item.GetProperty("id").GetInt32();
                        toAdd.rank = item.GetProperty("rank").GetInt32();
                        toAdd.slug = item.GetProperty("slug").GetString();
                        toAdd.symbol = item.GetProperty("symbol").GetString();
                        toAdd.name = item.GetProperty("name").GetString();
                        toAdd.type = item.GetProperty("type").GetString();

                        if (item.TryGetProperty("category", out buf))
                            toAdd.category = buf.GetString();

                        toAdd.price = usd.GetProperty("price").GetSingle();
                        toAdd.high24h = usd.GetProperty("high24h").GetSingle();
                        toAdd.low24h = usd.GetProperty("low24h").GetSingle();
                        toAdd.volume24h = usd.GetProperty("volume24h").GetSingle();

                        if (usd.TryGetProperty("percentChange24h", out buf))
                            toAdd.percentChange24h = buf.GetSingle();

                        if (usd.TryGetProperty("percentChange7d", out buf))
                            toAdd.percentChange7d = buf.GetSingle();

                        if (usd.TryGetProperty("percentChange30d", out buf))
                            toAdd.percentChange30d = buf.GetSingle();

                        if (usd.TryGetProperty("percentChange3m", out buf))
                            toAdd.percentChange3m = buf.GetSingle();

                        if (usd.TryGetProperty("percentChange6m", out buf))
                            toAdd.percentChange6m = buf.GetSingle();


                        toAdd.image16x16 = images.GetProperty("16x16").GetString();
                        toAdd.image60x60 = images.GetProperty("60x60").GetString();
                        toAdd.image200x200 = images.GetProperty("200x200").GetString();

                        result.Add(toAdd);
                    }
                }
                return result;
            }
        }
    }
}

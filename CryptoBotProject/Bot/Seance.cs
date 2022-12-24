using CryptoBotProject.Bot.Windows;
using Telegram.Bot.Types;

namespace CryptoBotProject.Bot
{
    public class Seance
    {
        private static Dictionary<long, Seance> instances = new Dictionary<long, Seance>();

        private static TimeSpan AfkTime => TimeSpan.FromHours(1);

        public static Seance GetSeance(long id)
        {
            if (instances.ContainsKey(id) == false)
                instances.Add(id, new Seance());

            return instances[id];
        }

        public static async Task CheckTimeoutSeances()
        {
            while(true)
            {
                await Task.Delay(AfkTime);

                lock (instances)
                {
                    List<long> indexes = new List<long>();

                    foreach(var item in instances)
                    {
                        if (item.Value.lastDateTimeUpdate.Add(AfkTime).CompareTo(DateTime.Now) <= 0)
                        {
                            indexes.Add(item.Key);
                        }
                    }
                    foreach(var index in indexes)
                    {
                        instances.Remove(index);
                    }
                }
            }
        }

        private Window activeWindow;

        private DateTime lastDateTimeUpdate;
        
        private Seance() 
        {
            activeWindow = new StartWindow();
        }

        public async void SendUpdate(Update update)
        {
            lastDateTimeUpdate = DateTime.UtcNow;
            await Task.Run(() => activeWindow.WindowsInteract(update));
        }
    }
}

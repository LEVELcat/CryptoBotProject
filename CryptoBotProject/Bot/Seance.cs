using CryptoBotProject.Bot.Windows;
using Telegram.Bot.Types;

namespace CryptoBotProject.Bot
{
    public class Seance
    {
        private static Dictionary<long, Seance> instances = new Dictionary<long, Seance>();

        public static Seance GetSeance(long id)
        {
            if (instances.ContainsKey(id) == false)
                instances.Add(id, new Seance());

            return instances[id];
        }

        static TimeSpan AfkTime => TimeSpan.FromHours(1);

        private Window activeWindow;
        
        private Seance() 
        {
            activeWindow = new Window();
        }


        public void SendMessage(Message message)
        {

        }



    }
}

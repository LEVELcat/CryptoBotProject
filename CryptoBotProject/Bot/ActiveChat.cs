using CryptoBotProject.Bot.Windows;
using CryptoBotProject.Bot.Windows.ManagersWindow;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBotProject.Bot
{
    public class ActiveChat
    {
        private static Dictionary<long, ActiveChat> instances = new Dictionary<long, ActiveChat>();

        public static ActiveChat GetChat(long ChatId)
        {
            if (instances.ContainsKey(ChatId) == false)
                instances.Add(ChatId, new ActiveChat(ChatId));

            return instances[ChatId];
        }

        private static TimeSpan AfkTime => TimeSpan.FromHours(24);
        //private static TimeSpan AfkTime => TimeSpan.FromSeconds(10);

        List<Window> activeWindows = new List<Window>();
        public void ShowWindow<T>()  where T: Window, new()
        {
            foreach(var window in activeWindows)
            {
                if (window is T)
                {
                    window.ShowMessage();
                    return;
                }
            }
            T newWindow = (T)Activator.CreateInstance(typeof(T), new object[] {chatId});
            activeWindows.Add(newWindow);


        }

        private long chatId = -1;

        private DateTime lastDateTimeUpdate = DateTime.Now;

        private ActiveChat(long id)
        {
            this.chatId = id;
        }

        
        public static async Task CheckTimeoutSeances()
        {
            while (true)
            {
                await Task.Delay(AfkTime);

                lock (instances)
                {
                    List<long> indexes = new List<long>();

                    foreach (var item in instances)
                    {
                        if (item.Value.lastDateTimeUpdate.Add(AfkTime).CompareTo(DateTime.Now) <= 0)
                        {
                            indexes.Add(item.Key);
                        }
                    }
                    foreach (var index in indexes)
                    {
                        instances[index].EndSeance(index);
                        instances.Remove(index);
                    }
                }
            }
        }

        private async void EndSeance(long chatId)
        {
            await TelegramBot.Instance.BotClient.SendTextMessageAsync(chatId: chatId, "Сеанс окончен");
        }

        public async void SendUpdate(Update update)
        {
            try
            {
                lastDateTimeUpdate = DateTime.UtcNow;

                if (activeWindows == null) activeWindows = new List<Window>();

                if (update.Message is Message && update.Message.Text != null && update.Message.Text[0] == '/')
                {
                    if (update.Message.Text.Contains("/manager "))
                    {
                        if (update.Message.Text.Split(' ')[1] == "admin")
                        {
                            ShowWindow<StartManagerWindow>();
                        }
                        return;
                    }

                    switch (update.Message.Text)
                    {
                        case "/start" or "/menu":
                            ShowWindow<StartWindow>();
                            return;
                    }
                }
                if (update.CallbackQuery is CallbackQuery callback) activeWindows.Where(x => x.WindowMessageId == callback.Message.MessageId).First().WindowsInteract(update);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\n" + e.Message);
            }
        }
    }
}

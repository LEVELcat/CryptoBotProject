﻿using CryptoBotProject.Bot.Windows;
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

        private Dictionary<int, Window> activeWindows = new Dictionary<int, Window>();

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

                if (activeWindows == null) activeWindows = new Dictionary<int, Window>();

                if (update.Message is Message && update.Message.Text != null && update.Message.Text[0] == '/')
                {
                    switch (update.Message.Text)
                    {
                        case "/start":
                            Window buf = new StartWindow(chatId);
                            activeWindows.Add(buf.WindowMessageId, buf);
                            return;
                    }
                }
                if (update.CallbackQuery is CallbackQuery callback) activeWindows[callback.Message.MessageId].WindowsInteract(update);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\n" + e.Message);
            }




        }
    }
}
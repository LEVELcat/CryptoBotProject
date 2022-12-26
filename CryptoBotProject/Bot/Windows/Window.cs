using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBotProject.Bot.Windows
{
    abstract class Window
    {
        public virtual void WindowsInteract(Update update)
        {

        }
    }

    class StartWindow : Window
    {
        static InlineKeyboardButton[] buttons = 
        {
            InlineKeyboardButton.WithCallbackData("Test1", "Test1CallBack"),
            InlineKeyboardButton.WithCallbackData("Test2", "Test2CallBack")
        };

        public StartWindow(long id)
        {
            TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: id,
                text: "Это стартовое окно",
                parseMode: ParseMode.Html,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                );
        }

        public override void WindowsInteract(Update update)
        {
            if (update.Message is not Message) return;

            //TelegramBot.Instance.BotClient.SendTextMessageAsync(
            //    chatId: update.Message.Chat.Id,
            //    text: this.ToString()
            //    );





        }
    }

    class TestWindow : Window
    {
        public TestWindow() { }

        public override void WindowsInteract(Update update)
        {
            base.WindowsInteract(update);
        }


    }
}

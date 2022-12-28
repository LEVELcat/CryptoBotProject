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
        protected long chatId = 0;
        public int WindowMessageId { get; protected set; } = 0;

        public virtual void WindowsInteract(Update update)
        {

        }
    }

    class StartWindow : Window
    {
        static InlineKeyboardButton[] buttons = 
        {
            InlineKeyboardButton.WithCallbackData("Test1", "Test1CallBack")
        };

        static InlineKeyboardButton[] buttons1 =
        {
            InlineKeyboardButton.WithCallbackData("Test1", "Test1CallBack"),
            InlineKeyboardButton.WithCallbackData("Test1", "Test1CallBack")
        };

        public StartWindow(long chatId)
        {
            this.chatId = chatId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Это стартовое окно",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;
        }

        ~StartWindow()
        {
            try
            {
                TelegramBot.Instance.BotClient.DeleteMessageAsync(
                chatId: chatId,
                messageId: WindowMessageId
                );
            }
            catch
            {

            }
        }

        public override void WindowsInteract(Update update)
        {
            if (update.CallbackQuery is not CallbackQuery)
            {
                if (update.Message is Message)
                {

                }

                return;
            }

                


            long id = update.CallbackQuery.From.Id;

            WindowMessageId = update.CallbackQuery.Message.MessageId;

            if (update.CallbackQuery.Data == "Test1CallBack")
            {
                //TelegramBot.Instance.BotClient.SendTextMessageAsync(
                //    chatId: id,
                //    text: "Это стартовое окно",
                //    parseMode: ParseMode.Markdown,
                //    replyMarkup: new InlineKeyboardMarkup(buttons)
                //    );

                TelegramBot.Instance.BotClient.EditMessageTextAsync(
                    chatId: id,
                    messageId: WindowMessageId,
                    text: "Нажата кнопка",
                    replyMarkup: new InlineKeyboardMarkup(buttons1)
                    );

            }
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

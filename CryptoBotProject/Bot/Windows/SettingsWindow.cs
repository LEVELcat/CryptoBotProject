using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace CryptoBotProject.Bot.Windows
{
    class SettingsWindow : Window
    {
        static InlineKeyboardButton[] buttons =
{
            InlineKeyboardButton.WithCallbackData("Смена языка", "SettingsWindow_ChangeLaunguage"),
            InlineKeyboardButton.WithCallbackData("Смена региона", "SettingsWindow_ChangeRegion"),
            InlineKeyboardButton.WithCallbackData("Изменение места жительства", "SettingsWindow_ChangeLocation"),
            InlineKeyboardButton.WithCallbackData("Изменение паспотрных данных", "SettingsWindow_ChangePassortData")
        };

        public SettingsWindow(long chatId)
        {
            this.ChatId = chatId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Это окно с настройками",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;
        }
        public SettingsWindow()
        {
            throw new Exception("Don't use this constructor");
        }
        ~SettingsWindow()
        {
            try
            {
                TelegramBot.Instance.BotClient.DeleteMessageAsync(
                chatId: ChatId,
                messageId: WindowMessageId
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\n" + e.Message);
            }
        }
        public override void WindowsInteract(Update update)
        {
            if (update.CallbackQuery is not CallbackQuery)
            {
                return;
            }

            switch (update.CallbackQuery.Data)
            {
                case "SettingsWindow_ChangeLaunguage":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть сменая языка со списком кнопок",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "SettingsWindow_ChangeRegion":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть смена региона?",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "SettingsWindow_ChangeLocation":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть смена региона?",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "SettingsWindow_ChangePassortData":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть изменение паспотрных данных"
                        );
                    break;
            }

        }

        public override void ShowMessage()
        {
            int pastMessageId = WindowMessageId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: ChatId,
                text: "Это окно с настройками",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;

            TelegramBot.Instance.BotClient.DeleteMessageAsync(ChatId, pastMessageId);

        }

    }
}

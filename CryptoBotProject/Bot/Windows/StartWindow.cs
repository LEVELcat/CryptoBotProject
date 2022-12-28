﻿using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBotProject.Bot.Windows
{
    class StartWindow : Window
    {
        static InlineKeyboardButton[] buttons = 
        {
            InlineKeyboardButton.WithCallbackData("Баланс", "StartWindow_Balance"),
            InlineKeyboardButton.WithCallbackData("Настройки", "StartWindow_Settings"),
            InlineKeyboardButton.WithCallbackData("Информация", "StartWindow_FAQ"),
            InlineKeyboardButton.WithCallbackData("Криптовалюты", "StartWindow_GetCryptoCoins")
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
                return;
            }

            if (update.CallbackQuery.Data == "Test1CallBack")
            {

            }
        }
    }
}

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBotProject.Bot.Windows
{
    class StartWindow : Window
    {
        static InlineKeyboardButton[][] buttons =
        {
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Баланс", "StartWindow_Balance") },
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Баланс", "StartWindow_Balance") },
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Настройки", "StartWindow_Settings") },
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Информация", "StartWindow_FAQ") },
            new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Криптовалюты", "StartWindow_GetCryptoCoins") }
        };

        public StartWindow(long chatId)
        {
            this.ChatId = chatId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите интересующее окно",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;
        }
        public StartWindow()
        {
            throw new Exception("Don't use this constructor");
        }

        ~StartWindow()
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

            switch(update.CallbackQuery.Data) 
            {
                case "StartWindow_FAQ":
                    ActiveChat.GetChat(ChatId).ShowWindow<InformationWindow>();
                    break;
                case "StartWindow_Balance":
                    ActiveChat.GetChat(ChatId).ShowWindow<BalanceWindow>();
                    break;
                case "StartWindow_GetCryptoCoins":
                    ActiveChat.GetChat(ChatId).ShowWindow<CryptoListWindow>();
                    break;
                case "StartWindow_Settings":
                    ActiveChat.GetChat(ChatId).ShowWindow<SettingsWindow>();
                    break;
            }
        }

        public override void ShowMessage()
        {
            int pastMessageId = WindowMessageId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
            chatId: ChatId,
            text: "Выберите интересующее окно",
            parseMode: ParseMode.Markdown,
            replyMarkup: new InlineKeyboardMarkup(buttons)
            ).Result.MessageId;

            TelegramBot.Instance.BotClient.DeleteMessageAsync(ChatId, pastMessageId);

        }
    }
}

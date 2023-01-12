using MySql.Data.MySqlClient;
using MySql.Data.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBotProject.Bot.Windows
{
    class BalanceWindow : Window
    {
        static InlineKeyboardButton[] buttons =
        {
            InlineKeyboardButton.WithCallbackData("Статус заявок на вывод", "BalanceWindow_StatusOfWithdrawalRequests"),
            InlineKeyboardButton.WithCallbackData("Статус заявок на пополнение", "BalanceWindow_StatusOfReplenishmentRequests"),
            InlineKeyboardButton.WithCallbackData("Отображение полного списка валют", "BalanceWindow_DisplayFullListOfCurrencies"),
            InlineKeyboardButton.WithCallbackData("Пополнение баланса", "BalanceWindow_ReplenishmentOfBalance")
        };

        enum StateMent
        {
            Balance,
            FullBalance,
            Withdraw,
            Replenishment
        }

        public BalanceWindow(long chatId)
        {
            this.ChatId = chatId;

            LocalRuntimeDB.Instance.ExecuteReaderCommand(out MySqlDataReader dataReader, 
                                                         "GetUSDTBalance", 
                                                        ("ChatId", this.ChatId)
                                                        );
            var usdtBalance = dataReader["USDT_Balance"].ToString();

            dataReader.DisposeAsync();

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Баланс USDT: " + usdtBalance,
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;
        }
        public BalanceWindow()
        {
            throw new Exception("Don't use this constructor");
        }

        ~BalanceWindow()
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
                case "BalanceWindow_StatusOfWithdrawalRequests":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть заявки на вывод",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "BalanceWindow_StatusOfReplenishmentRequests":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть заявки на пополнение",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "BalanceWindow_DisplayFullListOfCurrencies":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть полный список валют",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "BalanceWindow_ReplenishmentOfBalance":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть пополнение баланса",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
            }
        }

        public override void ShowMessage()
        {
            int pastMessageId = WindowMessageId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: ChatId,
                text: "Это окно с балансом",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;

            TelegramBot.Instance.BotClient.DeleteMessageAsync(ChatId, pastMessageId);

        }
    }
}

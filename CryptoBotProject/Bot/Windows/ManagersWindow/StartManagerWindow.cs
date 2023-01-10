using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBotProject.Bot.Windows.ManagersWindow
{
    class StartManagerWindow : Window
    {
        static InlineKeyboardButton[] buttons =
{
            InlineKeyboardButton.WithCallbackData("Заявки на вывод баланса", "StartManagerWindow_StatusOfWithdrawalRequests"),
            InlineKeyboardButton.WithCallbackData("Заявки на пополнение баланса", "StartManagerWindow_StatusOfReplenishmentRequests"),
            InlineKeyboardButton.WithCallbackData("Клиенты", "StartManagerWindow_GetClients"),
            InlineKeyboardButton.WithCallbackData("Закупки", "StartManagerWindow_GetSellBuys")
        };
        public StartManagerWindow(long chatId)
        {
            this.ChatId = chatId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Это окно менеджера",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;
        }
        public StartManagerWindow()
        {
            throw new Exception("Don't use this constructor");
        }

        ~StartManagerWindow()
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
                case "StartManagerWindow_StatusOfWithdrawalRequests":
                    ActiveChat.GetChat(ChatId).ShowWindow<StatusOfWithdrawalWindow>();
                    break;
                case "StartManagerWindow_StatusOfReplenishmentRequests":
                    ActiveChat.GetChat(ChatId).ShowWindow<StatusOfReplenishmentWindow>();
                    break;
                case "StartManagerWindow_GetClients":
                    ActiveChat.GetChat(ChatId).ShowWindow<ClientWindow>();
                    break;
                case "StartManagerWindow_GetSellBuys":
                    ActiveChat.GetChat(ChatId).ShowWindow<BuySellRequestWindow>();
                    break;
            }
        }
    }
}

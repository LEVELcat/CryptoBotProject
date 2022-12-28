using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace CryptoBotProject.Bot.Windows.ManagersWindow
{
    class StatusOfReplenishmentWindow : Window
    {
        static InlineKeyboardButton[] buttons =
{
            InlineKeyboardButton.WithCallbackData("<<", "InformationWindow_TermsOfCooperation"),
            InlineKeyboardButton.WithCallbackData(">>", "InformationWindow_License"),
        };

        public StatusOfReplenishmentWindow(long chatId)
        {
            this.ChatId = chatId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Это окно со списком заявок на пополнения баланса",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;
        }

        ~StatusOfReplenishmentWindow()
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

            }

        }
    }
}

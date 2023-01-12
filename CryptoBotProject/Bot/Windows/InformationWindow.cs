using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBotProject.Bot.Windows
{
    class InformationWindow : Window
    {
        static InlineKeyboardButton[] buttons =
        {
            InlineKeyboardButton.WithCallbackData("Условия сотрудничества", "InformationWindow_TermsOfCooperation"),
            InlineKeyboardButton.WithCallbackData("Лицензия", "InformationWindow_License"),
            InlineKeyboardButton.WithCallbackData("Пользовательское соглашение", "InformationWindow_UserAgreement"),
            InlineKeyboardButton.WithCallbackData("Инструкция по использования", "InformationWindow_InstructionsForUse")
        };

        public InformationWindow(long chatId)
        {
            this.ChatId = chatId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Это окно с информацией",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;
        }
        public InformationWindow()
        {
            throw new Exception("Don't use this constructor");
        }

        ~InformationWindow()
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
                case "InformationWindow_TermsOfCooperation":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть Условия сотрудничества",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "InformationWindow_License":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть Лицензия",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "InformationWindow_UserAgreement":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть Пользовательское соглашение",
                        replyMarkup: new InlineKeyboardMarkup(buttons)
                        );
                    break;
                case "InformationWindow_InstructionsForUse":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: "Тут должно быть Иструкция пользования",
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
                text: "Это окно с информацией",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(buttons)
                ).Result.MessageId;

            TelegramBot.Instance.BotClient.DeleteMessageAsync(ChatId, pastMessageId);

        }



    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBotProject.Bot.Windows
{
    public abstract class Window
    {
        public long ChatId { get; protected set; }  =  0;
        public int WindowMessageId { get; protected set; } = 0;

        public virtual void WindowsInteract(Update update)
        {
             
        }
    }

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
                    break;
                case "InformationWindow_UserAgreement":
                    break;
                case "InformationWindow_InstructionsForUse":
                    break;
            }

        }



    }

}

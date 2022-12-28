using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBotProject.Bot.Windows
{
    class CryptoListWindow : Window
    {
        static InlineKeyboardButton[] buttons =
        {
            InlineKeyboardButton.WithCallbackData("<<", "CryptoListWindow_BACK"),
            InlineKeyboardButton.WithCallbackData(">>", "CryptoListWindow_FORWARD")
        };

        public CryptoListWindow(long chatId)
        {
            this.ChatId = chatId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Это окно криптовалют лист {debugCurentList}",
                parseMode: ParseMode.Markdown,
                replyMarkup: GeneratekeyboardButtonList()
                ).Result.MessageId;
        }
        ~CryptoListWindow()
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
                case "CryptoListWindow_BACK":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: $"Тут лист с криптой №{debugCurentList--}",
                        replyMarkup: GeneratekeyboardButtonList()
                        );
                    break;
                case "CryptoListWindow_FORWARD":
                    TelegramBot.Instance.BotClient.EditMessageTextAsync(
                        chatId: ChatId,
                        messageId: WindowMessageId,
                        text: $"Тут лист с криптой №{debugCurentList++}",
                        replyMarkup: GeneratekeyboardButtonList()
                        );
                    break;
                default:
                    if(update.CallbackQuery.Data.Contains("CryptoListWindow_"))
                    {
                        TelegramBot.Instance.BotClient.EditMessageTextAsync(
                            chatId: ChatId,
                            messageId: WindowMessageId,
                            text: $"Тут лист с криптой №{update.CallbackQuery.Data.Split('_')[1]}",
                            replyMarkup: GeneratekeyboardButtonList()
                            );
                    }
                    break;
            }
        }

        private short debugCurentList = 0;
        private short debugMaxList = 5;
        private InlineKeyboardMarkup GeneratekeyboardButtonList()
        {
            List<InlineKeyboardButton> result = new List<InlineKeyboardButton>();

            if (debugCurentList > 0) result.Add(buttons[0]);

            for(short index = 1; index <= debugMaxList; index++)
            {
                result.Add(InlineKeyboardButton.WithCallbackData($"Coin №{index}", $"CryptoListWindow_{index}"));
            }

            if (debugCurentList < debugMaxList) result.Add(buttons[1]);

            return new InlineKeyboardMarkup(result);
        }
    }
}

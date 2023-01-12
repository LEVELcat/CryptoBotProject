using MySql.Data.MySqlClient;
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
                text: $"Список криптовалют {CurentList}",
                parseMode: ParseMode.Markdown,
                replyMarkup: GeneratekeyboardButtonList()
                ).Result.MessageId;
        }
        public CryptoListWindow()
        {
            throw new Exception("Don't use this constructor");
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
                    {
                        CurentList--;

                        TelegramBot.Instance.BotClient.EditMessageTextAsync(
                            chatId: ChatId,
                            messageId: WindowMessageId,
                            text: $"Список криптовалют ",
                            replyMarkup: GeneratekeyboardButtonList()
                        );
                    }
                    break;
                case "CryptoListWindow_FORWARD":
                    {
                        CurentList++;

                        TelegramBot.Instance.BotClient.EditMessageTextAsync(
                                                chatId: ChatId,
                                                messageId: WindowMessageId,
                                                text: $"Список криптовалют ",
                                                replyMarkup: GeneratekeyboardButtonList()
                                                );

                    }
                    break;
                default:
                    {
                        if (update.CallbackQuery.Data.Contains("CryptoListWindow_"))
                        {
                            string coin = update.CallbackQuery.Data.Split('_')[1];

                            LocalRuntimeDB.Instance.ExecuteReaderCommand(out MySqlDataReader dataReader,
                                "GetCryptoCoin",
                                ("Name", coin)
                            );

                            string resultText = string.Empty;
                            resultText += dataReader["Name"];
                            resultText += "Цена" + dataReader["Price"];
                            resultText += "Нижайшая цена за 24 часа" + dataReader["PriceLow24h"];
                            resultText += "Наивысшая цена за 24 часа" + dataReader["PriceHihg24h"];
                            resultText += "Объем торгов за 24 часа" + dataReader["Volume24h"];
                            resultText += "Изменение цены за 24 часа" + dataReader["PercentChange24h"];
                            resultText += "Изменение цены за неделю" + dataReader["PercentChange7d"];
                            resultText += "Изменение цены за 30 дней" + dataReader["PercentChange30d"];
                            resultText += "Изменение цены за 3 месяца" + dataReader["PercentChange3m"];
                            resultText += "Изменение цены за 6 месяцев" + dataReader["PercentChange6m"];
                            resultText += "Последнее обновление" + dataReader["LastUpdateTime"];



                            dataReader.Dispose();

                            TelegramBot.Instance.BotClient.EditMessageTextAsync(
                                chatId: ChatId,
                                messageId: WindowMessageId,
                                text: $"Информация о коине " + resultText,
                                replyMarkup: GeneratekeyboardButtonList()
                                );
                        }
                    }
                    break;
            }
        }

        private short CurentList = 0;
        private InlineKeyboardMarkup GeneratekeyboardButtonList()
        {
            MySqlDataReader dataReader;

            do
            {
                if (CurentList == 0) CurentList++;

                LocalRuntimeDB.Instance.ExecuteReaderCommand(out dataReader,
                    "GetCryptoList",
                    ("Size", 5),
                    ("Offset", CurentList * 5)
                    );

                if (dataReader.HasRows == false) CurentList--;
            } while (dataReader.HasRows);

            List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
            inlineKeyboardButtons.Add(buttons.ToList());

            List<string> strings = new List<string>();

            while (dataReader.Read())
            {
                inlineKeyboardButtons.Add(new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(dataReader["CoinName"].ToString(), "CryptoListWindow_" + dataReader["CoinName"].ToString()) });
                dataReader.NextResult();
            }
            dataReader.Dispose();


            return new InlineKeyboardMarkup(inlineKeyboardButtons);
        }

        public override void ShowMessage()
        {
            int pastMessageId = WindowMessageId;

            WindowMessageId = TelegramBot.Instance.BotClient.SendTextMessageAsync(
                chatId: ChatId,
                text: $"Это окно криптовалют лист {CurentList}",
                parseMode: ParseMode.Markdown,
                replyMarkup: GeneratekeyboardButtonList()
                ).Result.MessageId;

            TelegramBot.Instance.BotClient.DeleteMessageAsync(ChatId, pastMessageId);

        }
    }
}

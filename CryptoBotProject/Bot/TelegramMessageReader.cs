namespace CryptoBotProject.Bot
{
    internal class TelegramMessageReader
    {
        static TelegramMessageReader instance;
        public static TelegramMessageReader Instance
        {
            get
            {
                instance ??= new TelegramMessageReader();
                return instance;
            }
        }

        private TelegramMessageReader()
        {

        }

        public void Start()
        {

        }
    }
}

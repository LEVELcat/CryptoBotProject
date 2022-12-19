namespace CryptoBotProject.Bot
{
    internal class MessageReader
    {
        static MessageReader instance;
        public static MessageReader Instance
        {
            get
            {
                instance ??= new MessageReader();
                return instance;
            }
        }

        private MessageReader()
        {

        }

        public void Start()
        {

        }
    }
}

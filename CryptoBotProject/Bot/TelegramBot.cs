using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBotProject.Bot
{
    class TelegramBot
    {
        private static TelegramBot instance;
        public static TelegramBot Instance
        {
            get
            {
                instance ??= new TelegramBot();
                return instance;
            }
        }

        protected TelegramBot()
        {

        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}

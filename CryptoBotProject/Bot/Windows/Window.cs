using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

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

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

namespace CryptoBotProject.Bot.Windows
{
    abstract class Window
    {
        public Window()
        {

        }

        public virtual void WindowsInteract(Update update)
        {

        }
    }

    class StartWindow : Window
    {
        public StartWindow() { }

        public override void WindowsInteract(Update update)
        {
            
        }
    }
}

using Telegram.Bot.Types;

namespace CryptoBotProject.Bot.Windows
{
    public abstract class Window
    {
        public long ChatId { get; set; } = 0;
        public int WindowMessageId { get; protected set; } = 0;

        public virtual void WindowsInteract(Update update)
        {

        }
    }
}

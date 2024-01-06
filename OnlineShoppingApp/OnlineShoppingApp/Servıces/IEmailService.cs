using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Servıces
{
    public interface IEmailService
    {
        void Send(MessageViewModel message);
    }
}

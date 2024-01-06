using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class Subscriber: BaseEntity<int>
    {
        public string Email { get; set; }
    }
}

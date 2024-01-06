using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class Contact : BaseEntity<int>
    {
        public string Tittle { get; set; }
        public string AddresTittle { get; set; }
        public string AddresLine { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string WorkTime { get; set; }
    }
}

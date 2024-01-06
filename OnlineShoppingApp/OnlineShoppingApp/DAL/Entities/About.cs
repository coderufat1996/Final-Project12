using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class About : BaseEntity<int>
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public string Offer { get; set; }
        public string ImagePath { get; set; }
    }
}

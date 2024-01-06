using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class Deal : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Offer { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
    }
}

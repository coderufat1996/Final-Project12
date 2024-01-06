using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class Benefit : BaseEntity<int>
    {
        public string ImagePath { get; set; }
        public string Title { get; set; }
    }
}

using OnlineShoppingApp.DAL.Entities.Common;
using OnlineShoppingApp.DAL.Entities.Enums;

namespace OnlineShoppingApp.DAL.Entities
{
    public class Banner : BaseEntity<int>
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public BannerPageType BannerPageType { get; set; }
    }
}

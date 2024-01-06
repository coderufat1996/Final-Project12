using OnlineShoppingApp.DAL.Entities.Enums;

namespace OnlineShoppingApp.Areas.Admin.ViewModels.Banner
{
    public class CreateViewModel
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public BannerPageType BannerPageType { get; set; }
    }
}

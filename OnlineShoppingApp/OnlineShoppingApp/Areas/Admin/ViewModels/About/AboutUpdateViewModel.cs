namespace OnlineShoppingApp.Areas.Admin.ViewModels.About
{
    public class AboutUpdateViewModel
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public string Offer { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Photo { get; set; }
    }
}

namespace OnlineShoppingApp.Areas.Admin.ViewModels.Deal
{
    public class CreateViewModel
    {
        public string Title { get; set; }
        public string Offer { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
        public string Url { get; set; }
    }
}

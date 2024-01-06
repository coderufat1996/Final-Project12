namespace OnlineShoppingApp.Areas.Admin.ViewModels.Deal
{
    public class UpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Offer { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
    }
}

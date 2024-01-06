namespace OnlineShoppingApp.Areas.Admin.ViewModels.BlogImage
{
    public class UpdateViewModel
    {
        public int Id { get; set; }
        public int blogId { get; set; }
        public string ImagePath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}

namespace OnlineShoppingApp.Areas.Admin.ViewModels.BlogImage
{
    public class CreateViewModel
    {
        public int blogId { get; set; }
        public IFormFile? Photo { get; set; }
    }
}

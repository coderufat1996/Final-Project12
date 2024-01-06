namespace OnlineShoppingApp.Areas.Admin.ViewModels.ProductImage
{
    public class CreateViewModel
    {
        public int productId { get; set; }
        public IFormFile? Photo { get; set; }
    }
}

namespace OnlineShoppingApp.Areas.Admin.ViewModels.ProductImage
{
    public class UpdateViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}

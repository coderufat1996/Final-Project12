namespace OnlineShoppingApp.Areas.Admin.ViewModels.Product
{
    public class UpdateViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? MainImagePath { get; set; }
        public IFormFile? MainImage { get; set; }
        public decimal Price { get; set; }
        public int Rate { get; set; }
        public List<int>? SizeIds { get; set; }
        public List<ProductSizeViewModel>? ProductSizes { get; set; }
        public int? ProductCategoryId { get; set; }
        public List<ProductCategoryViewModel>? ProductCategories { get; set; }
    }
}

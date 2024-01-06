namespace OnlineShoppingApp.Areas.Admin.ViewModels.Product
{
    public class ProductCreateViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? MainImage { get; set; }
        public decimal Price { get; set; }
        public int Rate { get; set; }
        public List<int>? SizeIds { get; set; }
        public List<ProductSizeViewModel>? ProductSizes { get; set; }
        public int? ProductCategoryId { get; set; }
        public List<ProductCategoryViewModel>? ProductCategories { get; set; }
        public List<IFormFile> Images { get; set; }
    }
    public class ProductSizeViewModel
    {
        public ProductSizeViewModel(int id, string tittle)
        {
            Id = id;
            Tittle = tittle;
        }

        public int Id { get; set; }
        public string Tittle { get; set; }
    }

    public class ProductCategoryViewModel
    {
        public ProductCategoryViewModel(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }
        public string Title { get; set; }
    }
}

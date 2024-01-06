namespace OnlineShoppingApp.Areas.Admin.ViewModels.ProductImage
{
    public class ListViewModel
    {
        public int ProductId { get; set; }
        public List<ItemViewModel> Images { get; set; }
    }

    public class ItemViewModel
    {
        public ItemViewModel(int id, string imagePath)
        {
            Id = id;
            ImagePath = imagePath;
        }

        public int Id { get; set; }
        public string ImagePath { get; set; }
    }
}

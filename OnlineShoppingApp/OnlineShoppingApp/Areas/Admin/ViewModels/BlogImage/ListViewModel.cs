namespace OnlineShoppingApp.Areas.Admin.ViewModels.BlogImage
{
    public class ListViewModel
    {
        public int blogId { get; set; }
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

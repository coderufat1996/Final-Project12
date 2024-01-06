using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.ViewModels
{
    public class ShopItemViewModel
    {
        public Product? Product { get; set; }
        public List<Product>? Products { get; set; }
        public int? SizeId { get; set; }
        public List<SizeViewModel>? Sizes { get; set; }

    }

    public class SizeViewModel
    {
        public SizeViewModel(int id, string tittle)
        {
            Id = id;
            Tittle = tittle;
        }

        public int Id { get; set; }
        public string Tittle { get; set; }
    }
}

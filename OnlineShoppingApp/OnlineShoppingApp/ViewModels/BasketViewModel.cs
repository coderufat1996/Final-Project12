namespace OnlineShoppingApp.ViewModels
{
    public class BasketViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public decimal BasketPrice { get; set; }
        public int CategoryId { get; set; }
        public int? SizeId { get; set; }
        public int BasketCount { get; set; }
    }
}

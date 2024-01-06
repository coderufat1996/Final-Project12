namespace OnlineShoppingApp.Areas.Admin.ViewModels.Benefit
{
    public class UpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}

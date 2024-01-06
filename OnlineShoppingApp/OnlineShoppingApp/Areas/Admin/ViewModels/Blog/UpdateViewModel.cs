namespace OnlineShoppingApp.Areas.Admin.ViewModels.Blog
{
    public class UpdateViewModel
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string MainImagePath { get; set; }
        public IFormFile MainImage { get; set; }
    }
}

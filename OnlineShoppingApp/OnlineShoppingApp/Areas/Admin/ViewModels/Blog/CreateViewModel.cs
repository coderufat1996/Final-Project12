namespace OnlineShoppingApp.Areas.Admin.ViewModels.Blog
{
    public class CreateViewModel
    {
        public string Tittle { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}

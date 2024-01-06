namespace OnlineShoppingApp.ViewModels
{
    public class BannerViewModel
    {
        public BannerViewModel(string tittle, string description, string imagePath)
        {
            Tittle = tittle;
            Description = description;
            ImagePath = imagePath;
        }

        public string Tittle { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}

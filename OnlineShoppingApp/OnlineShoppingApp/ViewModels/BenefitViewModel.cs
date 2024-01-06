namespace OnlineShoppingApp.ViewModels
{
    public class BenefitViewModel
    {
        public BenefitViewModel(string title, string imagePath)
        {
            Title = title;
            ImagePath = imagePath;
        }

        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}

namespace OnlineShoppingApp.DAL.Entities.Enums
{
    public enum BannerPageType
    {
        Home = 1,
        Shop = 2,
        Blog = 3,
        About = 4,
        Contact = 5,
        Basket = 6,
    }

    public static class BannerTypeExecute
    {
        public static string GetBannerType(this BannerPageType bannerPageType)
        {
            switch (bannerPageType)
            {
                case BannerPageType.Home:
                    return "Home";
                case BannerPageType.Shop:
                    return "Shop";
                case BannerPageType.Blog:
                    return "Blog";
                case BannerPageType.About:
                    return "About";
                case BannerPageType.Contact:
                    return "Concat";
                case BannerPageType.Basket:
                    return "Basket";
                default:
                    throw new Exception("This status code not found");

            }
        }
    }
}

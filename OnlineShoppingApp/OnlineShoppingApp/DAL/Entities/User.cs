using Microsoft.AspNetCore.Identity;

namespace OnlineShoppingApp.DAL.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}

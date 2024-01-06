using Microsoft.AspNetCore.Identity;
using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.Areas.Admin.ViewModels.Role
{
    public class UpdateViewModel
    {
        public List<IdentityRole> Roles { get; set; }
        public IList<string> UserRoles { get; set; }
        public User User { get; set; }
    }
}

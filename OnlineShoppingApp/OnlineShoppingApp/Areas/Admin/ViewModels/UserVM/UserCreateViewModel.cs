using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Areas.Admin.ViewModels.UserVM
{
    public class UserCreateViewModel
    {
        public string FullName { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }

}

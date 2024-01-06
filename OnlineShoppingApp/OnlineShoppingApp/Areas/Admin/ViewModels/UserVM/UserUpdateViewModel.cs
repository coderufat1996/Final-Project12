using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Areas.Admin.ViewModels.UserVM
{
    public class UserUpdateViewModel
    {
        public string Id { get; set; }
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }

}

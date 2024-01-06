using OnlineShoppingApp.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string? ConfirmPasswod { get; set; }
    }
}

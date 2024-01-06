﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.ViewModels
{
    public class LoginViewModel
    {
        [Required, StringLength(100)]
        public string UserNameOrEmail { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

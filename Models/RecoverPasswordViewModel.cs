﻿namespace SuperShop.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IWill_MvcApplication.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Display(Name = "Keep me Logged In")]
        public bool LoggedIn { get; set; }

    }
}
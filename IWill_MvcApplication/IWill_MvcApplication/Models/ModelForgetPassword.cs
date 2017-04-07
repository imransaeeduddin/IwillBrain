using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IWill_MvcApplication.Models
{
    public class ModelForgetPassword
    {
        //[Required(ErrorMessage = "Email Address is required")]
        [Display(Name = "Email Address")]
        //[RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        //ErrorMessage = "Please enter correct email address")]
        public string EmailAddress { get; set; }

    }

    public class ModelForgetPasswordVerification
    {

        public string Email { get; set; }
        public string RequestedUrl{ get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "Confirm Password required")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }
    }

}
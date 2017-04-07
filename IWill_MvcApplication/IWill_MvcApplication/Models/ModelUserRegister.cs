using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IWill_MvcApplication.Models
{
    public class ModelUserRegister
    {
        public List<ModelUserRegQuestion> LstQuestion { get; set; }
        public LoginViewModel LoginModel { get; set; }

        public long UID { get; set; }

        [Required(ErrorMessage ="First Name is Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        public string BackFileName { get; set; }

        [Display(Name = "Gender")]
        public byte GID { get; set; }
        //public Nullable<int> Year { get; set; }
        //public Nullable<int> Month { get; set; }
        //public Nullable<int> Day { get; set; }
        public string  Gender{ get; set; }
        public string CityName{ get; set; }
        public string CountryName { get; set; }



        [Display(Name = "About Me")]
        public string AboutMe { get; set; }

        [Display(Name = "Web Url")]
        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Please enter a valid Web Url")]
        public string WebUrl { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Please enter a valid Facebook Url")]
        [Display(Name = "Facebook Url")]
        public string FacebookUrl { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Please enter a valid Plus Google Url")]
        [Display(Name = "Plus Google Url")]
        public string PlusGoogleUrl { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Please enter a valid Git Hub Url")]
        [Display(Name = "Git Hub Url")]

        public string GitHubUrl { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Please enter a valid Git Hub Url")]
        [Display(Name = "Linked In Url")]
        public string LinkedInUrl { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Please enter a valid Git Hub Url")]
        [Display(Name = "Twitter Url")]
        public string TwitterUrl { get; set; }

        [Display(Name = "Upload Picture")]
        [DataType(DataType.Upload)]
        //[Url(ErrorMessage = "Please enter a valid url")]
        public HttpPostedFileBase file { get; set; }

        public string fileName { get; set; }

        [DataType(DataType.ImageUrl)]
        [Url(ErrorMessage = "Please enter a valid url")]
        public string SUrl { get; set; }


        [Required(ErrorMessage = "Email Address is required")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please enter correct email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone Number")]
        //[DataType(DataType.PhoneNumber)]
        //[Phone(ErrorMessage ="insert a valid number")]
        //[RegularExpression("[^0-9]", ErrorMessage = "only Numeric is accepted")]
        //[Required]

        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter proper contact details.")]
        public string ContactNumber { get; set; }
        public string EmergenyNumber { get; set; }
        //public Nullable<bool> IsStudent { get; set; }
        //public Nullable<bool> IsActive { get; set; }
        //public Nullable<bool> IsDeleted { get; set; }
        public string Nationality { get; set; }

        //[Required(ErrorMessage = "Please Select the Religion")]



        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Religion { get; set; }

        [Display(Name = "City")]
        //[Required(ErrorMessage = "Please Select the City")]

        public long CTID { get; set; }

        [Display(Name = "Country")]
        //[Required(ErrorMessage = "Please Select the Country")]
        public long CTRYID { get; set; }
        public int MartialStatus { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password required")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        //public Nullable<int> Age { get; set; }
        //public Nullable<System.DateTime> CreatedOn { get; set; }
        //public Nullable<long> CreatedBy { get; set; }
        //public Nullable<System.DateTime> UpdatedOn { get; set; }
        //public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }


        [Display(Name = "Mother Language")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string MotherLanguage { get; set; }
        //public string FirstLoginUrl { get; set; }
        //public Nullable<bool> IsFirstLoggedIn { get; set; }

        public string RegisterBy { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Date Of Birth Required")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth{ get; set; }

        [Display(Name = "Is Student")]
        public bool IsStudent { get; set; }
        public string BackImgTop { get; set; }
        public string BackImgLeft { get; set; }
        public string FrontImgTop { get; set; }
        public string FrontImgLeft { get; set; }
        public string Designation{ get; set; }

        public LoginViewModel LoginViewModel { get; set; }

        public List<ModelUserFollower> LstFollower { get; set; }

    }

    public class ModelUserFollower
    {
        public string FollwerFirstName { get; set; }

        public string FollwerLastName { get; set; }

        public string FollwerDesignation { get; set; }

        public string PicUrl { get; set; }
        public Nullable<long> FollwerID { get; set; }
    }
}
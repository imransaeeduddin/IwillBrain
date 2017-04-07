using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IWill_MvcApplication.Models
{
    public class ModelUserProfile
    {
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string PlusGoogleUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string ContactNumber { get; set; }
        public long CTID { get; set; }
        public long CTRYID { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MotherLanguage { get; set; }
        public string Religion { get; set; }
        public string Designation { get; set; }



    }

    public partial class ModelUserEducation
    {
        public Nullable<long> EducationType { get; set; }
        public string NameOfInstitute { get; set; }
        public Nullable<long> EducationFromYear { get; set; }

        public Nullable<int> EducationFromMonth { get; set; }

        public Nullable<long> EducationToYear { get; set; }
        public Nullable<int> EducationToMonth { get; set; }
        public Nullable<bool> EducationIsPresent { get; set; }

        public string MajorSubjects { get; set; }
        public string CourseTitle { get; set; }

        public string EducationLocation { get; set; }
        public string EducationDescription { get; set; }




        public Nullable<long> UEDID { get; set; }
        public long FkUID { get; set; }
        public Nullable<long> FkETID { get; set; }


        public Nullable<int> TotalDuration { get; set; }


        public Nullable<int> FromDay { get; set; }
        public Nullable<int> ToDay { get; set; }

        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }

        public string UEpicUrl { get; set; }


        public virtual User User { get; set; }
    }

    public partial class ModelUserExperience
    {
        public string CompanyName { get; set; }
        public string UserPost { get; set; }
        public string Location { get; set; }
        public Nullable<int> FromMonth { get; set; }
        public Nullable<int> FromYear { get; set; }

        public Nullable<int> ToYear { get; set; }
        public Nullable<int> ToMonth { get; set; }

        public bool? IsUpdate { get; set; }
        public Nullable<bool> IsPresent { get; set; }
        public string UEXID { get; set; }
        public Nullable<long> FkUID { get; set; }
        public string Description { get; set; }

        public Nullable<int> TotalExperience { get; set; }


        public Nullable<int> FromDay { get; set; }

        public Nullable<int> ToDay { get; set; }


        public string CompLogoUrl { get; set; }

        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }

        // public virtual User User { get; set; }

    }

}
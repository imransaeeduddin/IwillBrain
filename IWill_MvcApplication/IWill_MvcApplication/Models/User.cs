//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IWill_MvcApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.CourseCoaches = new HashSet<CourseCoach>();
            this.UserEducations = new HashSet<UserEducation>();
            this.UserEnrollCourses = new HashSet<UserEnrollCourse>();
            this.UserExperiences = new HashSet<UserExperience>();
            this.UserGivenQuesAns = new HashSet<UserGivenQuesAn>();
            this.UserQuizAttemps = new HashSet<UserQuizAttemp>();
            this.UserRoles = new HashSet<UserRole>();
            this.UserFollowers = new HashSet<UserFollower>();
            this.UserFollowers1 = new HashSet<UserFollower>();
        }
    
        public long UID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Nullable<byte> FkGenderID { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Day { get; set; }
        public string AboutMe { get; set; }
        public string WebUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string PlusGoogleUrl { get; set; }
        public string GitHubUrl { get; set; }
        public string PicUrl { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string EmergenyNumber { get; set; }
        public Nullable<bool> IsStudent { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public Nullable<long> FkCityID { get; set; }
        public Nullable<long> FkCountryID { get; set; }
        public Nullable<bool> MartialStatus { get; set; }
        public string Password { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string MotherLanguage { get; set; }
        public string FirstLoginUrl { get; set; }
        public Nullable<bool> IsFirstLoggedIn { get; set; }
        public string LinkedInUrl { get; set; }
        public Nullable<System.Guid> ActivationCode { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<System.DateTime> LoggedIn { get; set; }
        public Nullable<decimal> RegQuesPercentage { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string UserIP { get; set; }
        public Nullable<int> BackImgLeft { get; set; }
        public Nullable<int> FrontImgTop { get; set; }
        public Nullable<int> FrontImgLeft { get; set; }
        public Nullable<bool> IsBasicInfoRegistered { get; set; }
        public Nullable<bool> IsPersonalInfoRegistered { get; set; }
        public Nullable<bool> IsRegistrationQuestRegistered { get; set; }
        public string Address { get; set; }
        public Nullable<int> BackImgTop { get; set; }
        public string Designation { get; set; }
        public string TwitterUrl { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseCoach> CourseCoaches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserEducation> UserEducations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserEnrollCourse> UserEnrollCourses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserExperience> UserExperiences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserGivenQuesAn> UserGivenQuesAns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserQuizAttemp> UserQuizAttemps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFollower> UserFollowers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFollower> UserFollowers1 { get; set; }
    }
}

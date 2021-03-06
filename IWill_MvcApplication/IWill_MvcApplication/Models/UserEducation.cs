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
    
    public partial class UserEducation
    {
        public long UEDID { get; set; }
        public long FkUID { get; set; }
        public Nullable<long> FkETID { get; set; }
        public Nullable<long> FromYear { get; set; }
        public Nullable<long> ToYear { get; set; }
        public string NameOfInstitute { get; set; }
        public Nullable<int> TotalDuration { get; set; }
        public Nullable<int> FromMonth { get; set; }
        public Nullable<int> ToMonth { get; set; }
        public Nullable<int> FromDay { get; set; }
        public Nullable<int> ToDay { get; set; }
        public string Descriptipn { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<bool> IsPresent { get; set; }
        public string UEpicUrl { get; set; }
        public string Location { get; set; }
        public string MajorSubjects { get; set; }
        public string CourseTitle { get; set; }
    
        public virtual EducationType EducationType { get; set; }
        public virtual User User { get; set; }
    }
}

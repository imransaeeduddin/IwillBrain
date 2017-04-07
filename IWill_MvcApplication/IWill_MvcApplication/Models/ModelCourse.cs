using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IWill_MvcApplication.Models
{
    public class ModelCourseCategory
    {
        public string CCName { get; set; }
        public string BackImage { get; set; }
        public int? BackLeft { get; set; }
        public int? BackTop { get; set; }

    }

    public class ModelCourseCategorySelect2
    {
        public long id { get; set; }
        public string text { get; set; }
        
        
        

    }
}
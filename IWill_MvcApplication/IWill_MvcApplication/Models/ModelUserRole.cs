using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IWill_MvcApplication.Models
{
    public class ModelUserRole
    {

        
        public long UID { get; set; }

        public string UName { get; set; }

        public string EmailAddress { get; set; }
        public long RID { get; set; }

        //public List<SelectList> 

        public string RName { get; set; }


    }
}
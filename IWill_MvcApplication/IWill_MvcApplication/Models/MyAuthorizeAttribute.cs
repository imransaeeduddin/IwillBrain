using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IWill_MvcApplication.Models
{
    public class MyAuthorizeAttribute : AuthorizeAttribute

    {
        public string UserRole { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
        
            var IsAuthorized = base.AuthorizeCore(httpContext);
            if (!IsAuthorized) return false;

            //if (UserRole.Contains(httpContext.user))
        }
    }
}
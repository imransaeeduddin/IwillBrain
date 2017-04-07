using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IWill_MvcApplication.Models;

namespace IWill_MvcApplication.Controllers
{
    public class AuthorizeUserAccess : AuthorizeAttribute
    {

        public string UserRole { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //if (Helper.User.RoleName == null || Helper.User.RoleName.Length == 0)
            //    return false;
            
            var IsAuthorized = base.AuthorizeCore(httpContext);
            if (!IsAuthorized) return false;

            if (UserRole == null)
                return false;

            if (Helper.User.IsFirstLoggedIn == false)
                return false;

            if (Helper.User.RoleName == "Admin")
                return true;

            if (UserRole.Split(',').Contains(Helper.User.RoleName))
                return true;

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if (Helper.User.RoleName == null || Helper.User.RoleName.Length == 0)
            //    filterContext.Result = new RedirectResult("~/Home/Iindex");

            //base.HandleUnauthorizedRequest(filterContext);
            if (Helper.User == null)
                filterContext.Result = new RedirectResult("~/Home/Iindex");
            else
            {
                if (Helper.User.IsFirstLoggedIn == false)
                    filterContext.Result = new RedirectResult("~/Error/NotFirstLoggedInView");
                else
                    filterContext.Result = new RedirectResult("~/Error/UnAuthorizeUserView");
            }


        }

    }
}
using IWill_MvcApplication.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;


namespace IWill_MvcApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //if (!Request.IsAuthenticated)
           // {
                //HttpContext.Current.RewritePath("Home/Iindex");

                //    string con = Request.RequestContext.RouteData.Values["Controller"].ToString();
                //    string act = Request.RequestContext.RouteData.Values["Action"].ToString();

                //    if (con == "Employee" && act == "Index")
                //        return;
                //RouteData routeData = RouteTable.Routes.GetRouteData(
                //new HttpContextWrapper(HttpContext.Current));
                //Session["action"] = routeData.GetRequiredString("action");
                //Session["controller"] = routeData.GetRequiredString("controller");
                //    bool isvalid = Helper.IsvalidRequest(action, controller);

                //    Response.RedirectPermanent("Employee/Iindex");
           // }


        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {

            //if (Request.IsAuthenticated)
            //{
            //    string con = Request.RequestContext.RouteData.Values["Controller"].ToString();
            //    string act = Request.RequestContext.RouteData.Values["Action"].ToString();

            //    if (con == "Employee" && act == "Index")
            //        return;
            //    RouteData routeData = RouteTable.Routes.GetRouteData(
            //     new HttpContextWrapper(HttpContext.Current));
            //    var action = routeData.GetRequiredString("action");
            //    var controller = routeData.GetRequiredString("controller");
            //    bool isvalid = Helper.IsvalidRequest(action, controller);

            //    Response.RedirectPermanent("Employee/Index");
            //}

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (authCookie != null)
            //{
            //    // Get the forms authentication ticket.
            //    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    var identity = new GenericIdentity(authTicket.Name, "Forms");
            //    var principal = new MyPrincipal(identity);
            //    // Get the custom user data encrypted in the ticket.

            //    try
            //    {
            //         var idn = Context.User.Identity;
            //    }
            //    catch (Exception ex)
            //    {

            //        throw;
            //    }

            //    string userData = ((FormsIdentity)(Context.User.Identity)).Ticket.UserData;
            //    // Deserialize the json data and set it on the custom principal.
            //    var serializer = new JavaScriptSerializer();
            //    principal.User = (UserLoginData)serializer.Deserialize(userData, typeof(UserLoginData));
            //    // Set the context user.
            //    Context.User = principal;

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                // Get the forms authentication ticket.
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new GenericIdentity(authTicket.Name, "Forms");
                var principal = new MyPrincipal(identity);
                // Get the custom user data encrypted in the ticket.
                string userData = ((FormsIdentity)(Context.User.Identity)).Ticket.UserData;
                // Deserialize the json data and set it on the custom principal.
                var serializer = new JavaScriptSerializer();
                principal.User = (UserLoginData)serializer.Deserialize(userData, typeof(UserLoginData));
                // Set the context user.
                Context.User = principal;


            }
           

        }

        //public void Application_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!Request.IsAuthenticated)
        //        {
        //            throw new InvalidCredentialException("The user is not authenticated.");
        //        }
        //    }
        //    catch (InvalidCredentialException ex)
        //    {
        //        var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        //        Response.Redirect(urlHelper.Action("Iindex", "Home"));
        //    }
        //}

        //protected void Application_AuthorizeRequest(Object sender, EventArgs e)
        //{
        //    if (this.User.Identity.IsAuthenticated)
        //    {
        //        //log this
        //    }
        //    else
        //    {
        //        var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        //               Response.Redirect(urlHelper.Action("Iindex", "Home"));
        //    }
        //}
    }
}

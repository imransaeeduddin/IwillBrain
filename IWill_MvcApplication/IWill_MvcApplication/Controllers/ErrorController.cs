using IWill_MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IWill_MvcApplication.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [AllowAnonymous]
        public ActionResult UnAuthorizeUserView()
        {
            try
            {
                if (Helper.User == null)
                    return RedirectToAction("~/Home/Iindex");
                else
                    return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"]= "Error: " + ex.Message;
                return View();

            }


        }
        [AllowAnonymous]
        public ActionResult PageNotFound()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"]= "Error: " + ex.Message;
                return View();

            }

        }

        [AllowAnonymous]
        public ActionResult NotFirstLoggedInView()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"]= "Error: " + ex.Message;
                return View();

            }

        }



    }
}
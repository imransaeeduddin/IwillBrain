using IWill_MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IWill_MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        private IwillDbEntities DBC = new IwillDbEntities();
        public ActionResult Index()
        {
            try
            {
                if (Helper.User != null)
                {
                    var UserEmail = Helper.User.Email;
                    var data = DBC.Users.Where(i => i.EmailAddress == UserEmail.Trim()).FirstOrDefault();
                    if (data != null && (data.IsFirstLoggedIn == false || data.IsFirstLoggedIn == null))
                        TempData["NotActivated"] = "Dear " + data.FirstName + ", you are not activated yet ,get activated by your Email ";
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"]= "Error: " + ex.Message;
                return View();
            }

        }
        public ActionResult Iindex()
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

        public ActionResult Courses()
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

        //public ActionResult IwillHome()
        //{
        //    try
        //    {

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Error"]= "Error: " + ex.Message;
        //        return View();
        //    }

        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CustomFileUpload()
        {
            ViewBag.Message = "Your custom file uploading here ";

            return View();
        }
        
    }
}
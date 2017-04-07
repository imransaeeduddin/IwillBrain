using IWill_MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace IWill_MvcApplication.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        [AuthorizeUserAccess(UserRole = "Admin")]
        public ActionResult VCreateCourseCategory()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {

                return Json("Error: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult CourseCategoryFileUpload()
        {
            var path = "";
            try
            {
                string CurrentUserIP = Request.UserHostAddress;
                // CheckUserPicUploadCount(UserIP);
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {

                    var count = Dbc.TempPics.Where(i => i.UserIP == CurrentUserIP && EntityFunctions.TruncateTime(i.CreatedOn.Value) == EntityFunctions.TruncateTime(DateTime.Now) && i.CreatedBy == Helper.User.UID).ToList().Count;

                    if (count > 10)
                    {

                        return Json("Create Category, image upload limit exceeded", JsonRequestBehavior.AllowGet);
                    }
                }
                string fileName = "";
                string Ext = "";
                string PicPath = "";
                if (Request.Files.Count == 0)
                    return Json("nofiles", JsonRequestBehavior.AllowGet);

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    fileName = Path.GetFileName(file.FileName);
                    Ext = Path.GetExtension(file.FileName);

                    //fileName = "UserImg_" + Guid.NewGuid().ToString().Trim() + Ext;
                    fileName = Guid.NewGuid().ToString().Trim() + Ext;

                    var fileLocation = Server.MapPath("~/Images/Temp/");
                    FileInfo fileDiresctory = new FileInfo(fileLocation);
                    if (!fileDiresctory.Exists)
                        fileDiresctory.Directory.Create();

                    path = Path.Combine(fileLocation, fileName);
                    if (!System.IO.File.Exists(path))
                    {
                        string aa = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        file.SaveAs(path);
                        using (IwillDbEntities Dbc = new IwillDbEntities())
                        {
                            var tempPic = new TempPic() { TempPicName = fileName, CreatedOn = DateTime.Now, CreatedBy = Helper.User.UID, UserIP = Request.UserHostAddress };

                            Dbc.TempPics.Add(tempPic);
                            Dbc.SaveChanges();
                        }
                    }
                    else
                        return Json("file already exists", JsonRequestBehavior.AllowGet);

                }
                return Json("saved^" + fileName, JsonRequestBehavior.AllowGet);
                //return Json("saved^", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("notsaved " + path, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult RemoveBackgroundPic(string CategoryName, string FileName, string IsBackPicEdit)
        {
            try
            {

                using (IwillDbEntities Dbc = new IwillDbEntities())
                {

                    if (IsBackPicEdit == "true")
                    {
                        var BackPic = Dbc.CourseCategories.Where(i => i.CreatedBy == Helper.User.UID && i.CCName.ToLower() == CategoryName.ToLower().Trim()).FirstOrDefault();
                        var OldPicLocation = Server.MapPath("~/Images/UserImages/" + BackPic.CImagePath);
                        System.IO.File.Delete(OldPicLocation);

                        BackPic.CImagePath = null;
                        BackPic.BackImgLeft = null;
                        BackPic.BackImgTop = null;

                    }
                    else
                    {
                        var fullPath = Server.MapPath("~/Images/Temp/" + FileName);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);

                            var tempPic = Dbc.TempPics.Where(i => i.TempPicName == FileName).FirstOrDefault();
                            Dbc.TempPics.Remove(tempPic);

                        }
                        else
                            return Json("Error: not deleted", JsonRequestBehavior.AllowGet);

                    }
                    Dbc.SaveChanges();
                    return Json("deleted", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                return Json("Error: on delete<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]
        //[AuthorizeUserAccess(UserRole = "Admin")]
        //public JsonResult SaveBackgroundPic(string FileName, string BCimgTOP, string BCimgLeft, string IsBackPicEdit)
        //{
        //    try
        //    {

        //        using (IwillDbEntities Dbc = new IwillDbEntities())
        //        {
        //            //DP image  upload on server

        //            var TempLocation = Server.MapPath("~/Images/Temp/" + FileName);
        //            var DestLocation = Server.MapPath("~/Images/UserImages/" + FileName);
        //            System.IO.File.Copy(TempLocation, DestLocation);
        //            System.IO.File.Delete(TempLocation);
        //            var data = Dbc.CourseCategories.Where(i => i.CreatedBy == Helper.User.UID).FirstOrDefault();
        //            if (data != null)
        //            {

        //                if (IsBackPicEdit.Trim() == "true" && data.CImagePath != null)
        //                {
        //                    var OldPicLocation = Server.MapPath("~/Images/UserImages/" + data.CImagePath);
        //                    System.IO.File.Delete(OldPicLocation);
        //                }

        //                data.CImagePath = FileName;
        //                data.BackImgTop = Convert.ToInt32(BCimgTOP);
        //                data.BackImgLeft = Convert.ToInt32(BCimgLeft);
        //                Dbc.SaveChanges();
        //            }

        //        }
        //        return Json("saved", JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json("Error: on delete<br> " + ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}       

        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult CreateCategory(string CCName, string BackImage, string BackLeft, string BackTop, string IsEditPic) //ModelCourseCategory ModCorsCat)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {



                    if (IsEditPic == "false")
                    {
                        var data = new CourseCategory();
                        var TempLocation = Server.MapPath("~/Images/Temp/" + BackImage);
                        var DestLocation = Server.MapPath("~/Images/UserImages/" + BackImage);
                        System.IO.File.Copy(TempLocation, DestLocation);
                        System.IO.File.Delete(TempLocation);

                        data.CCName = CCName;
                        data.CImagePath = BackImage;
                        data.BackImgLeft = Convert.ToInt32(BackLeft);
                        data.BackImgTop = Convert.ToInt32(BackTop);
                        data.CreatedOn = DateTime.Now;
                        data.CreatedBy = Helper.User.UID;
                        data.IsActive = true;
                        Dbc.CourseCategories.Add(data);
                        Dbc.SaveChanges();
                        TempData["success"] = "Course Category Back Image has been Created";

                        return Json("saved", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var TempLocation = Server.MapPath("~/Images/Temp/" + BackImage);
                        var DestLocation = Server.MapPath("~/Images/UserImages/" + BackImage);
                        System.IO.File.Copy(TempLocation, DestLocation);
                        System.IO.File.Delete(TempLocation);

                        var data = Dbc.CourseCategories.Where(i => i.CCName.ToLower() == CCName.ToLower().Trim() && i.CreatedBy == Helper.User.UID).FirstOrDefault();
                        if (data != null)
                        {

                            data.CImagePath = BackImage;
                            data.BackImgLeft = Convert.ToInt32(BackLeft);
                            data.BackImgTop = Convert.ToInt32(BackTop);
                            data.CreatedOn = DateTime.Now;
                            Dbc.SaveChanges();
                            //TempData["success"] = "Course Category Back Image has been updated";
                            return Json("updated", JsonRequestBehavior.AllowGet);
                        }
                        else
                            throw new Exception("data not found to update course category");
                    }
                }


                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on save<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult GetCategories()//ModelCourseCategory ModCorsCat)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {

                    List<string> data = Dbc.CourseCategories.Select(s => s.CCName).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);

                }
                //return Json("saved", JsonRequestBehavior.AllowGet);

                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on save<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult Select2GetCategories()//ModelCourseCategory ModCorsCat)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {

                    List<ModelCourseCategorySelect2> data = Dbc.CourseCategories.Select(s =>
                                                        new ModelCourseCategorySelect2
                                                        {
                                                            id = s.CCID,
                                                            text = s.CCName
                                                        }).ToList();

                    return Json(data, JsonRequestBehavior.AllowGet);

                }
                //return Json("saved", JsonRequestBehavior.AllowGet);

                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on save<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult GetCourseCategory(string CategoryName)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {

                    ModelCourseCategory data = Dbc.CourseCategories.Where(i => i.CCName.ToLower().Trim().Replace(" ", "") == CategoryName.ToLower().Trim().Replace(" ", "")).
                                        Select(s => new ModelCourseCategory()
                                        {
                                            BackImage = s.CImagePath,
                                            CCName = s.CCName,
                                            BackLeft = s.BackImgLeft,
                                            BackTop = s.BackImgTop
                                        }).FirstOrDefault();

                    if (data != null)
                        return Json(data, JsonRequestBehavior.AllowGet);
                    else return Json("nothing", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                return Json("Error: on save<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult UpdateBackImagePostion(string CategoryName, string BCimgTOP, string BCimgLeft)
        {
            try
            {

                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    //DP image  upload on server

                    var data = Dbc.CourseCategories.Where(i => i.CreatedBy == Helper.User.UID && i.CCName.ToLower() == CategoryName.ToLower().Trim()).FirstOrDefault();
                    if (data != null)
                    {
                        data.BackImgTop = Convert.ToInt32(BCimgTOP);
                        data.BackImgLeft = Convert.ToInt32(BCimgLeft);
                        Dbc.SaveChanges();
                    }
                    else
                        return Json("Error: invalid Pic Name found", JsonRequestBehavior.AllowGet);

                }
                return Json("updated", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json("Error: on update<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        

            [HttpPost]
        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult CreateAndAssignCourse(List<int> values)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {

                }

                return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on save<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
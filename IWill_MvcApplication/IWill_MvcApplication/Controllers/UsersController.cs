using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IWill_MvcApplication.Models;
using System.Web.Security;
using System.Configuration;
using TweetSharp;
using Twitterizer;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Net.Mail;
using ASPSnippets.LinkedInAPI;
using System.Web.Hosting;
using static IWill_MvcApplication.Models.Helper;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace IWill_MvcApplication.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {

        private IwillDbEntities DBC = new IwillDbEntities();

        #region Mange LogIn
        [AllowAnonymous]
        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error in Login " + ex.Message;
                return View();
            }

        }

        [AllowAnonymous]
        public ActionResult MyIWillProfile()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error in Login " + ex.Message;
                return View();
            }

        }

        [AllowAnonymous]
        public ActionResult nav_register()
        {
            try
            {
                // ModelUserRegister ModReg = new ModelUserRegister();

                //if (TempData["Gdata"] != null)
                //{
                //    var data = (GmailLoginData)TempData["Gdata"];

                //    ModReg.EmailAddress = data.email;
                //    ModReg.FirstName = data.given_name;
                //    ModReg.LastName = data.family_name;
                //    ModReg.PlusGoogleUrl = data.link;
                //    ModReg.GID = Helper.AuthenticateGender(data.gender);
                //    ModReg.SUrl = data.picture;
                //    ModReg.RegisterBy = "Gmail";
                //}


                //if (TempData["Fdata"] != null)
                //{
                //    var data = (FacebookLoginData)TempData["Fdata"];

                //    ModReg.EmailAddress = data.Email;
                //    ModReg.FirstName = data.FirstName;
                //    ModReg.LastName = data.LastName;

                //    ModReg.GID = Helper.AuthenticateGender(data.Gender);
                //    ModReg.SUrl = data.Picture;
                //    ModReg.RegisterBy = "Facebook";

                //}

                //if (TempData["Ldata"] != null)
                //{
                //    var data = (LinkedInLoginData)TempData["Ldata"];

                //    ModReg.EmailAddress = data.Email;
                //    ModReg.FirstName = data.FirstName;
                //    ModReg.LastName = data.LastName;
                //    ModReg.SUrl = data.PictureUrl;
                //    ModReg.LinkedInUrl = data.ProfileUrl;
                //    ModReg.CTID = Helper.GetCityID(data.CityName);
                //    ModReg.CTRYID = Helper.GetCountryID(data.CountryName);
                //    //ModReg.GID = 2;
                //    ModReg.RegisterBy = "Linked In";
                //}

                ////List<SelectListItem> GenderList =  new SelectList( DBC.Genders.ToList());
                ////GenderList.Insert(0, new SelectListItem() { Text="Select", Value="0" };);
                //if (ModReg.RegisterBy != null)
                //    ViewBag.RegisterBy = ModReg.RegisterBy;

                //ViewBag.Gender = new SelectList(DBC.Genders.ToList(), "GID", "Name");



                //ViewBag.Country = new SelectList(DBC.Countries, "CTRYID", "CTName");

                //if (TempData["Model"] != null)
                //    ModReg = (ModelUserRegister)TempData["Model"];
                //return View(ModReg);

                return View();
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error in Login " + ex.Message;
                return View();
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("DoLogIn")]
        [AllowAnonymous]
        //public ActionResult DoLogIn(string Email, string Password, bool LoggedIn)
        public JsonResult DoLogIn(string Email, string Password, bool LoggedIn)
        {
            try
            {
                var EncPass = Helper.Encrypt(Password);
                var data = DBC.Users.Where(i => i.EmailAddress == Email && i.Password == EncPass).FirstOrDefault();
                if (data == null)
                {
                    ViewData["Error"] = "Error: Invalid password";

                    //return View("LogIn");
                    return Json("Error: Invalid user name or password", JsonRequestBehavior.AllowGet);
                }
                if (data.LoggedIn != null)
                {
                    data.LastLogin = data.LoggedIn;
                    data.LoggedIn = DateTime.Now;

                    int DaysDiff = ((TimeSpan)(data.LoggedIn - DateTime.Now)).Days;
                    if (DaysDiff > 29)
                    {
                        TempData["LoginDays"] = "Its a very long time to come back again in our web site. ";
                    }
                }
                DBC.SaveChanges();

                UserLoginData user = new UserLoginData();
                user.UID = data.UID;
                user.UName = data.FirstName;
                user.LName = data.LastName;
                user.Email = data.EmailAddress;
                user.Image = data.PicUrl;
                //save image position
                user.BackImgTop = data.BackImgTop;
                user.BackImgLeft = data.BackImgLeft;
                user.FrontImgTop = data.BackImgTop;
                user.FrontImgLeft = data.FrontImgLeft;

                user.RoleID = Convert.ToInt64(data.UserRoles.FirstOrDefault().Role.RoleID);
                user.RoleName = data.UserRoles.FirstOrDefault().Role.RName;
                user.IsFirstLoggedIn = (data.IsFirstLoggedIn == null || data.IsFirstLoggedIn == false) ? false : true;

                Helper.CreateCookie(user, Response);

                //return RedirectToAction("Index", "Home");

                return Json("Index", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                //ViewData["Error"]= "Error: " + ex.Message + "/n " + ex.InnerException;
                //return RedirectToAction("LogIn", "Users");
                return Json("Error: " + ex.Message, JsonRequestBehavior.AllowGet);
            }


        }


        // [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Iindex", "Home");
        }

        #endregion

        #region Facebook Login Integration

        //private Uri RedirectUri
        //{
        //    get
        //    {
        //        var uriBuilder = new UriBuilder(Request.Url);
        //        uriBuilder.Query = null;
        //        uriBuilder.Fragment = null;
        //        uriBuilder.Path = Url.Action("FacebookCallback");
        //        return uriBuilder.Uri;

        //    }
        //}

        [AllowAnonymous]
        public ActionResult Facebook()
        {
            try
            {
                var fb = new FacebookClient();

                var LoginUrl = fb.GetLoginUrl(new
                {
                    client_id = ConfigurationManager.AppSettings["FB_ClientID"], // "302514596817933",
                    client_secret = ConfigurationManager.AppSettings["FB_Secret_Key"],// "b8d757b7b5ba5869260077f4f7a20fef",
                    redirect_uri = ConfigurationManager.AppSettings["FacebookRedirectUrl"],//RedirectUri.AbsoluteUri,
                    response_type = "code",
                    scope = "email"
                });

                return Redirect(LoginUrl.AbsoluteUri);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Login By Facebook" + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }

        }

        [AllowAnonymous]
        public ActionResult FacebookCallback(string code)
        {
            try
            {
                //var redirect = RedirectUri.AbsoluteUri;
                var fb = new FacebookClient();
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = ConfigurationManager.AppSettings["FB_ClientID"], // "302514596817933",
                    client_secret = ConfigurationManager.AppSettings["FB_Secret_Key"],// "b8d757b7b5ba5869260077f4f7a20fef",
                    redirect_uri = ConfigurationManager.AppSettings["FacebookRedirectUrl"],//RedirectUri.AbsoluteUri,
                    code = code,
                });


                var accessToken = result.access_token;
                Session["AccessToken"] = accessToken;
                fb.AccessToken = accessToken;

                dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

                FacebookLoginData FData = new FacebookLoginData();

                FData.Email = me.email;
                FData.FirstName = me.first_name;
                FData.LastName = me.last_name;
                FData.Picture = me.picture.data.url;
                FData.Gender = me.gender;
                //FData.Verified_email = me.verified;

                var data = DBC.Users.Where(i => i.EmailAddress == FData.Email.Trim()).FirstOrDefault();
                if (data != null)
                {
                    UserLoginData user = new UserLoginData();
                    user.UName = data.FirstName;
                    user.Email = data.EmailAddress;

                    Helper.CreateCookie(user, Response);
                    //FormsAuthentication.SetAuthCookie(Email, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Fdata"] = FData;
                    return RedirectToAction("Register", "Users");
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Login By Facebook" + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }



        }

        #endregion

        #region Gmail Login Integration
        [AllowAnonymous]
        public ActionResult GmailLoginRedirect()
        {
            try
            {
                return View();
            }


            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Login By Facebook" + ex.Message;
                return RedirectToAction("Home", "Iindex");

            }

        }
        [AllowAnonymous]
        public ActionResult GmailCallBack()
        {


            //string Email_address = "";
            //string Google_ID = "";
            //string firstName = "";
            //string LastName = "";

            try
            {

                if (Request["access_token"] != null)
                {
                    String URI = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + Request.QueryString["access_token"].ToString();

                    WebClient webClient = new WebClient();
                    var Ddata = webClient.DownloadString(URI);
                    GmailLoginData Gdata = new GmailLoginData();
                    JsonConvert.PopulateObject(Ddata, Gdata);


                    var data = DBC.Users.Where(i => i.EmailAddress == Gdata.email.Trim()).FirstOrDefault();
                    if (data != null)
                    {
                        UserLoginData user = new UserLoginData();
                        user.UName = data.FirstName;
                        user.Email = data.EmailAddress;
                        user.RoleID = data.UserRoles.FirstOrDefault().FKRID;
                        user.RoleName = data.UserRoles.FirstOrDefault().Role.RName;

                        Helper.CreateCookie(user, Response);
                        //FormsAuthentication.SetAuthCookie(Email, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Gdata"] = Gdata;
                        return RedirectToAction("Register", "Users");
                    }


                    //Stream stream = webClient.OpenRead(URI);
                    //string b;

                    ///*I have not used any JSON parser because I do not want to use any extra dll/3rd party dll*/
                    //using (StreamReader br = new StreamReader(stream))
                    //{
                    //    b = br.ReadToEnd();
                    //}

                    //b = b.Replace("id", "").Replace("email", "");
                    //b = b.Replace("given_name", "");
                    //b = b.Replace("family_name", "").Replace("link", "").Replace("picture", "");
                    //b = b.Replace("gender", "").Replace("locale", "").Replace(":", "");
                    //b = b.Replace("\"", "").Replace("name", "").Replace("{", "").Replace("}", "");


                    //Array ar = b.Split(",".ToCharArray());
                    //for (int p = 0; p < ar.Length; p++)
                    //{
                    //    ar.SetValue(ar.GetValue(p).ToString().Trim(), p);

                    //}
                    //Email_address = ar.GetValue(1).ToString();
                    //Google_ID = ar.GetValue(0).ToString();
                    //firstName = ar.GetValue(4).ToString();
                    //LastName = ar.GetValue(5).ToString();


                }
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Login By Gmail" + ex.Message;
                return RedirectToAction("Home", "Iindex");

            }
        }

        #endregion

        #region Twitter Login Registration
        //public ActionResult Twitter()
        //{
        //    TwitterService service = new TwitterService();
        //    AuthRequestToken 
        //    return View();
        //}

        //public ActionResult TwitterCallback()
        //{
        //    return View();
        //}

        private Uri TwitterRedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("TwitterCallback");
                return uriBuilder.Uri;

            }
        }

        [AllowAnonymous]

        public ActionResult Twitter()
        {
            try
            {
                var consumerKey = ConfigurationManager.AppSettings["TW_API_Key"];
                var consumerSecret = ConfigurationManager.AppSettings["TW_SCR_Key"];
                var TwitterRedirectUrl = ConfigurationManager.AppSettings["TwitterRedirectUrl"];


                //Step 1: Get Request Token
                OAuthTokenResponse RequestToken = OAuthUtility.GetRequestToken(consumerKey, consumerSecret, TwitterRedirectUrl.ToString());

                //Step 2: Redirect User to Requested Token
                Response.Redirect("http://twitter.com/oauth/authorize?oauth_token=" + RequestToken.Token, true);

                return RedirectToAction("Index", "Home");



            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Login By Twitter " + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }
        }

        [AllowAnonymous]
        public ActionResult TwitterCallback()
        {
            try
            {

                var consumerKey = ConfigurationManager.AppSettings["TW_API_Key"];
                var consumerSecret = ConfigurationManager.AppSettings["TW_SCR_Key"];

                string RequestToken = Request["oauth_token"];
                string Verifier = Request["oauth_verifier"];

                var token = OAuthUtility.GetAccessToken(consumerKey, consumerSecret, RequestToken, Verifier);

                OAuthTokens accesstoken = new OAuthTokens()
                {
                    AccessToken = token.Token,
                    AccessTokenSecret = token.TokenSecret,
                    ConsumerKey = consumerKey,
                    ConsumerSecret = consumerSecret
                };
                string UserName = token.ScreenName;
                decimal UserID = token.UserId;


                //TwitterResponse<TwitterStatus> response = TwitterStatus.Update(accesstoken, "this is imran saeed");
                //if (response.Result == RequestResult.Success)
                //{

                //}

                //TwitterResponse<TwitterUser> usr = TwitterUser.Show(accesstoken, UserName);
                //TwitterUser res = usr.ResponseObject;
                //string email = res.

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: Login By Twitter " + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }
        }

        #endregion

        #region  Linked In Integration
        [AllowAnonymous]
        public ActionResult LinkedIn()
        {
            try
            {

                //LinkedInConnect.APIKey = "77m5vkhs1id4kg";
                //LinkedInConnect.APISecret = "FhtiACbATE0UIoVF";
                //LinkedInConnect.RedirectUrl = "http://localhost:52507/Users/LinkedInCallBack";
                LinkedInConnect.APIKey = ConfigurationManager.AppSettings["LI_ClientID"];
                LinkedInConnect.APISecret = ConfigurationManager.AppSettings["LI_Secret_Key"];
                LinkedInConnect.RedirectUrl = ConfigurationManager.AppSettings["LinkedInRedirectUrl"];
                LinkedInConnect.Authorize();
                return RedirectToAction("Home", "Iindex");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Login By Linked In \n" + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }

        }
        [AllowAnonymous]
        public ActionResult LinkedInCallBack()
        {
            try
            {
                if (LinkedInConnect.IsAuthorized)
                {

                    DataSet ds = LinkedInConnect.Fetch();
                    LinkedInLoginData Ldata = new LinkedInLoginData()
                    {
                        ID = ds.Tables["person"].Rows[0]["id"].ToString(),
                        FirstName = ds.Tables["person"].Rows[0]["first-name"].ToString(),
                        LastName = " " + ds.Tables["person"].Rows[0]["last-name"].ToString(),
                        Email = ds.Tables["person"].Rows[0]["email-address"].ToString(),
                        HeadLine = ds.Tables["person"].Rows[0]["headline"].ToString(),
                        Industry = ds.Tables["person"].Rows[0]["industry"].ToString(),
                        ProfileUrl = ds.Tables["person"].Rows[0]["public-profile-url"].ToString(),
                        CityName = ds.Tables["location"].Rows[0]["name"].ToString(),
                        CountryName = ds.Tables["location"].Rows[1]["name"].ToString(),
                        PictureUrl = ds.Tables["person"].Rows[0]["picture-url"].ToString()
                    };
                    var data = DBC.Users.Where(i => i.EmailAddress == Ldata.Email.Trim()).FirstOrDefault();
                    if (data != null)
                    {
                        UserLoginData user = new UserLoginData();
                        user.UName = data.FirstName;
                        user.Email = data.EmailAddress;

                        Helper.CreateCookie(user, Response);
                        //FormsAuthentication.SetAuthCookie(Email, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Ldata"] = Ldata;
                        return RedirectToAction("Register", "Users");
                    }

                }
                else
                {
                    ViewData["Error"] = "Error: Ivalid Linked In User";
                    return RedirectToAction("Home", "Iindex");
                }

            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Login By Linked In \n" + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }

        }

        #endregion

        #region Registration
        [AllowAnonymous]
        public ActionResult Register()
        {
            try
            {
                ModelUserRegister ModReg = new ModelUserRegister();

                if (TempData["Gdata"] != null)
                {
                    var data = (GmailLoginData)TempData["Gdata"];

                    ModReg.EmailAddress = data.email;
                    ModReg.FirstName = data.given_name;
                    ModReg.LastName = data.family_name;
                    ModReg.PlusGoogleUrl = data.link;
                    ModReg.GID = Helper.AuthenticateGender(data.gender);
                    ModReg.SUrl = data.picture;
                    ModReg.RegisterBy = "Gmail";
                }


                if (TempData["Fdata"] != null)
                {
                    var data = (FacebookLoginData)TempData["Fdata"];

                    ModReg.EmailAddress = data.Email;
                    ModReg.FirstName = data.FirstName;
                    ModReg.LastName = data.LastName;

                    ModReg.GID = Helper.AuthenticateGender(data.Gender);
                    ModReg.SUrl = data.Picture;
                    ModReg.RegisterBy = "Facebook";

                }

                if (TempData["Ldata"] != null)
                {
                    var data = (LinkedInLoginData)TempData["Ldata"];

                    ModReg.EmailAddress = data.Email;
                    ModReg.FirstName = data.FirstName;
                    ModReg.LastName = data.LastName;
                    ModReg.SUrl = data.PictureUrl;
                    ModReg.LinkedInUrl = data.ProfileUrl;
                    ModReg.CTID = Helper.GetCityID(data.CityName);
                    ModReg.CTRYID = Helper.GetCountryID(data.CountryName);
                    //ModReg.GID = 2;
                    ModReg.RegisterBy = "Linked In";
                }

                //List<SelectListItem> GenderList =  new SelectList( DBC.Genders.ToList());
                //GenderList.Insert(0, new SelectListItem() { Text="Select", Value="0" };);
                if (ModReg.RegisterBy != null)
                    ViewBag.RegisterBy = ModReg.RegisterBy;

                ViewBag.Gender = new SelectList(DBC.Genders.ToList(), "GID", "Name");



                ViewBag.Country = new SelectList(DBC.Countries, "CTRYID", "CTName");

                if (TempData["Model"] != null)
                    ModReg = (ModelUserRegister)TempData["Model"];
                return View(ModReg);
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: Ivalid Linked In User <br>" + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }

        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetRegister(ModelUserRegister mod)
        //{
        //    try
        //    {
        //        var user = new User();
        //        Guid ActivationCode = Guid.NewGuid();
        //        if (IsEmailIDRegistered(mod.EmailAddress))
        //        {
        //            ViewData["Error"]= "Error: " + mod.EmailAddress + "is already registered, try another";
        //            TempData["Model"] = mod;
        //            return RedirectToAction("Register", "Users");
        //        }
        //        user.EmailAddress = mod.EmailAddress;
        //        user.FirstName = mod.FirstName;
        //        user.MiddleName = mod.MiddleName;
        //        user.LastName = mod.LastName;
        //        user.Password = Helper.Encrypt(mod.Password); // here encryotion is required 
        //        user.FkGenderID = mod.GID;
        //        user.WebUrl = mod.WebUrl;
        //        user.FacebookUrl = mod.FacebookUrl;
        //        user.PlusGoogleUrl = mod.PlusGoogleUrl;
        //        user.LinkedInUrl = mod.LinkedInUrl;
        //        //user.PicUrl = mod.file.FileName;
        //        user.ContactNumber = mod.ContactNumber;
        //        user.FkCityID = mod.CTID;
        //        user.FkCountryID = mod.CTRYID;
        //        user.Religion = mod.Religion;
        //        user.MotherLanguage = mod.MotherLanguage;
        //        user.AboutMe = mod.AboutMe;
        //        user.ActivationCode = ActivationCode;
        //        user.IsStudent = mod.IsStudent;
        //        //DateTime UserDate;
        //        //if (DateTime.TryParse(mod.DateOfBirth.ToString(), out UserDate))
        //        //    throw new Exception("invalid date of birth");

        //        user.DateOfBirth = mod.DateOfBirth;

        //        // DP image  upload on server 
        //        //var TempLocation = Server.MapPath("~/Images/Temp/" + mod.fileName);
        //        //var DestLocation = Server.MapPath("~/Images/UserImages/" + mod.fileName);
        //        //System.IO.File.Copy(TempLocation, DestLocation);
        //        //System.IO.File.Delete(TempLocation);
        //        //user.PicUrl = mod.fileName;

        //        //// BC image  upload on server 
        //        //var TempBCLocation = Server.MapPath("~/Images/Temp/" + mod.BackFileName);
        //        //var DestBCLocation = Server.MapPath("~/Images/UserImages/" + mod.BackFileName);
        //        //System.IO.File.Copy(TempBCLocation, DestBCLocation);
        //        //System.IO.File.Delete(TempLocation);
        //        //user.BackgroundImageUrl= mod.BackFileName;


        //        user.IsFirstLoggedIn = false;
        //        user.IsActive = true;
        //        user.UserIP = Request.UserHostAddress;
        //        DBC.Users.Add(user);
        //        DBC.SaveChanges();

        //        var id = DBC.Users.Local[0].UID;
        //        UserRole Urole = new UserRole();
        //        Urole.FKUID = id;

        //        Urole.FKRID = user.IsStudent == true ? Helper.GetPrimaryRoleID((int)ERoles.Student) : GetPrimaryRoleID((int)ERoles.Coach);
        //        DBC.UserRoles.Add(Urole);
        //        DBC.SaveChanges();
        //        SendAccountEmailVerification(ActivationCode, mod.EmailAddress, mod.FirstName);
        //        TempData["IsStudent"] = mod.IsStudent.ToString();
        //        TempData["UserID"] = id.ToString();

        //        return RedirectToAction("ViewGetRegestrationQuestion", "UserRegQuestion");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Error"]= "Error: " + ex.Message;
        //        if (mod.fileName != null)
        //        {
        //            var TempLocation = Server.MapPath("~/Images/Temp/" + mod.fileName);
        //            if (System.IO.File.Exists(TempLocation))
        //            {
        //                System.IO.File.Delete(TempLocation);

        //            }
        //        }
        //        return RedirectToAction("Register", "Users");
        //    }

        //}

        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        //ModelViewRegQuestion modQues
        public JsonResult GetRegister(ModelUserRegister mod)
        {
            DbContextTransaction transaction = null;
            try
            {

                Guid ActivationCode = Guid.NewGuid();

                if (IsEmailIDRegistered(mod.EmailAddress))
                    return Json("Error: " + mod.EmailAddress + "is already registered, try another", JsonRequestBehavior.AllowGet);

                if (mod.EmailAddress == null)
                    return Json("Error: email can not be null", JsonRequestBehavior.AllowGet);

                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    using (transaction = Dbc.Database.BeginTransaction())
                    {
                        #region Save User Data their Role 

                        var user = new User();
                        user.EmailAddress = mod.EmailAddress;
                        user.FirstName = mod.FirstName;

                        user.LastName = mod.LastName;
                        user.Password = Helper.Encrypt(mod.Password); // here encryotion is required 
                        user.FkGenderID = mod.GID;

                        user.ContactNumber = mod.ContactNumber;
                        user.FkCityID = mod.CTID;
                        user.FkCountryID = mod.CTRYID;
                        user.Religion = mod.Religion;
                        user.MotherLanguage = mod.MotherLanguage;
                        user.ActivationCode = ActivationCode;
                        user.IsStudent = mod.IsStudent;
                        user.Designation = mod.Designation;
                        user.DateOfBirth = mod.DateOfBirth;
                        user.IsFirstLoggedIn = false;
                        user.IsActive = true;
                        user.UserIP = Request.UserHostAddress;
                        DBC.Users.Add(user);
                        DBC.SaveChanges();

                        var id = DBC.Users.Local[0].UID;
                        UserRole Urole = new UserRole();
                        Urole.FKUID = id;

                        Urole.FKRID = user.IsStudent == true ? Helper.GetPrimaryRoleID((int)ERoles.Student) : GetPrimaryRoleID((int)ERoles.Coach);
                        DBC.UserRoles.Add(Urole);
                        DBC.SaveChanges();

                        #endregion

                        //Save User Regestration questions 
                        int QuesWeight = mod.LstQuestion.Where(i => i.FkUQType != 2).ToList().Count;
                        float CountRight = 0;

                        foreach (var ques in mod.LstQuestion)
                        {
                            float CountTotOption = 0F;
                            float CountRightOptions = 0F;
                            float PerOptionWeight = 0.00F;

                            UserGivenQuesAn data = new UserGivenQuesAn();
                            data.FkUserID = id;
                            data.FkUQID = ques.UQID;
                            data.CreatedOn = DateTime.Now;
                            data.FkQTID = ques.FkUQType;
                            data.Question = ques.UQuestion;
                            if (ques.FkUQType == 2)
                            {
                                data.Answer = ques.Answer;
                                Dbc.UserGivenQuesAns.Add(data);
                                Dbc.SaveChanges();
                            }

                            else// if (ques.FkUQType == 1 || ques.FkUQType == 3)
                            {
                                CountTotOption = ques.UserQuestionDetail.Count;
                                //For multiple options
                                CountRightOptions = Dbc.UserQuestionDetails.Where(i => i.FkQID == ques.UQID && i.IsRight == true && i.IsActive == true).ToList().Count;
                                PerOptionWeight = CountRightOptions / (CountRightOptions * CountRightOptions);

                                foreach (var option in ques.UserQuestionDetail)
                                {

                                    data.OptionName = option.QuesOptionName;
                                    data.FkUQDID = option.UQDID;
                                    var RightAnsDetl = Dbc.UserQuestionDetails.Where(i => i.UQDID == option.UQDID && i.IsRight == true && i.IsActive == true).FirstOrDefault();
                                    if (RightAnsDetl != null)
                                        data.IsOptionRight = RightAnsDetl.IsRight;
                                    else data.IsOptionRight = false;

                                    if (ques.FkUQType == 1) // checking for MCQz
                                    {

                                        if (RightAnsDetl != null && option.IsRight)
                                        {
                                            data.IsUserRight = option.IsRight;
                                            CountRight = CountRight + 1;
                                        }
                                        else data.IsUserRight = option.IsRight;

                                    }

                                    if (ques.FkUQType == 3)
                                    {

                                        if (RightAnsDetl != null && option.IsRight)
                                        {
                                            data.IsUserRight = option.IsRight;
                                            CountRight += PerOptionWeight;
                                        }
                                        else data.IsUserRight = option.IsRight;

                                    }

                                    Dbc.UserGivenQuesAns.Add(data);
                                    Dbc.SaveChanges();
                                }
                            }
                        }

                        float Perce = (CountRight / QuesWeight) * 100;
                        var userdata = Dbc.Users.Where(i => i.UID == id).FirstOrDefault();
                        userdata.RegQuesPercentage = Convert.ToDecimal(Perce);
                        Dbc.SaveChanges();
                        transaction.Commit();

                        // TempData["Reg"] = "Regestered";
                        //RedirectToAction("Users", "UserProfile");
                    }
                }

                SendAccountEmailVerification(ActivationCode, mod.EmailAddress, mod.FirstName);

                return Json("saved ", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //ViewData["Error"] = "Error: " + ex.Message;
                //if (mod.fileName != null)
                //{
                //    var TempLocation = Server.MapPath("~/Images/Temp/" + mod.fileName);
                //    if (System.IO.File.Exists(TempLocation))
                //    {
                //        System.IO.File.Delete(TempLocation);

                //    }
                //}
                //   return RedirectToAction("Register", "Users");
                transaction.Rollback();
                return Json("Error: " + ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [AllowAnonymous]
        public ActionResult VSuccessfullyRegistered()
        {
            try
            {
                if (Request.QueryString["Reg"] != null)
                {
                    ViewBag.Message = "registered";
                    return View();
                }
                else
                {
                    ViewData["Error"] = "Error: Invalid Access";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: " + ex.Message;
                return View();
            }

        }

        //[AuthorizeUserAccess(UserRole = "Coach,Student")]
        //[AllowAnonymous]
        //public ActionResult ResendActivationEmail()
        //{
        //    User data = null;
        //    if (!string.IsNullOrEmpty(Helper.User.Email))
        //    {
        //        data = DBC.Users.Where(i => i.EmailAddress == Helper.User.Email).FirstOrDefault();
        //        SendAccountEmailVerification(data.ActivationCode, data.EmailAddress, data.FirstName);
        //        ViewData["Error"] = "Check your Email,  you have successfully send Verificaion link to your Email Addres";
        //        return RedirectToAction("UserProfile", "Users");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Users");
        //    }
        //}

        [AllowAnonymous]
        [HttpGet]
        public JsonResult ResendActivationEmail()
        {
            User data = null;
            if (!string.IsNullOrEmpty(Helper.User.Email))
            {
                data = DBC.Users.Where(i => i.EmailAddress == Helper.User.Email).FirstOrDefault();
                SendAccountEmailVerification(data.ActivationCode, data.EmailAddress, data.FirstName);
                //ViewData["Error"] = "Check your Email,  you have successfully send Verificaion link to your Email Addres";
                // return RedirectToAction("UserProfile", "Users");
                return Json("Check your Email,  you have successfully send Verificaion link to your Email Addres", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error: invalid Email", JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Login", "Users");
            }

        }
        public void SendAccountEmailVerification(Guid? ActivationCode, string EmaillAddress, string FirstName)
        {
            //string code = UserDataActivationCode.ToString();


            string path = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            string Verifypath = "Users/VerifyUserByEmail/?VerificationCode=" + ActivationCode.ToString() + "&Email=" + EmaillAddress;
            string absolutepath = path + Verifypath;
            string physicalpath = Request.PhysicalApplicationPath + "Content/EmailTemplates/VerifyNewUser.txt";
            StreamReader sr = new StreamReader(physicalpath);
            string Body = sr.ReadToEnd();
            Body = Body.Replace("<%UserName>", FirstName);
            Body = Body.Replace("<%VerificationUrl%>", absolutepath);

            Helper.SendMessage("New User Registration Verification", "", EmaillAddress, Body);

            TempData["UserEmail"] = EmaillAddress.Trim();
        }
        #endregion

        #region Picture Uplaod
        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult FrontPicUpload()
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

                        return Json("User image upload limit exceeded for today", JsonRequestBehavior.AllowGet);
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
            }
            catch (Exception ex)
            {

                return Json("notsaved " + ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult RemoveTempFrontPic(string FileName)
        {
            try
            {
                var fullPath = Server.MapPath("~/Images/Temp/" + FileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    using (IwillDbEntities Dbc = new IwillDbEntities())
                    {
                        var tempPic = Dbc.TempPics.Where(i => i.TempPicName == FileName).FirstOrDefault();
                        Dbc.TempPics.Remove(tempPic);
                        Dbc.SaveChanges();
                    }
                    return Json("deleted", JsonRequestBehavior.AllowGet);
                }
                return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on delete<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult SaveFrontPic(string FileName)
        {
            try
            {

                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    //DP image  upload on server

                    var TempLocation = Server.MapPath("~/Images/Temp/" + FileName);
                    var DestLocation = Server.MapPath("~/Images/UserImages/" + FileName);
                    System.IO.File.Copy(TempLocation, DestLocation);
                    System.IO.File.Delete(TempLocation);
                    var data = Dbc.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                    if (data != null)
                    {
                        // if (IsEdit.Trim() == "true" && data.BackgroundImageUrl != null)
                        // {
                        //    var OldPicLocation = Server.MapPath("~/Images/UserImages/" + data.BackgroundImageUrl);
                        //    System.IO.File.Delete(OldPicLocation);
                        // }
                        data.PicUrl = FileName;
                        //  data.BackImgTop = Convert.ToInt32(BCimgTOP);
                        //  data.BackImgLeft = Convert.ToInt32(BCimgLeft);
                        Dbc.SaveChanges();
                    }
                    else
                        return Json("Error: user not found", JsonRequestBehavior.AllowGet);

                }
                return Json("saved", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json("Error: on delete<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult RemoveFrontPic(string FileName, string Email)
        {
            try
            {
                var fullPath = Server.MapPath("~/Images/UserImages/" + FileName);
                var data = DBC.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                if (data == null)
                    throw new Exception("user not found to delete");

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    if (data != null)
                        data.PicUrl = null;


                }
                else
                    data.PicUrl = null;
                DBC.SaveChanges();
                return Json("deleted", JsonRequestBehavior.AllowGet);


                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on delete<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult UserBackgroundFileUpload()
        {
            var path = "";
            try
            {
                string CurrentUserIP = Request.UserHostAddress;
                // CheckUserPicUploadCount(UserIP);
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {

                    //var value = Dbc.TempPics.Where(i => i.UserIP == CurrentUserIP).FirstOrDefault().CreatedOn;
                    //var count = Dbc.TempPics.Where(i => i.UserIP == CurrentUserIP && i.CreatedOn.Value.Date == DateTime.Now.Date).ToList().Count;
                    var count = Dbc.TempPics.Where(i => i.UserIP == CurrentUserIP && EntityFunctions.TruncateTime(i.CreatedOn.Value) == EntityFunctions.TruncateTime(DateTime.Now) && i.CreatedBy == Helper.User.UID).ToList().Count;

                    if (count > 10)
                    {

                        return Json("Cannot Register, image upload limit exceeded", JsonRequestBehavior.AllowGet);
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
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult RemoveBackgroundPic(string FileName, string IsBackPicEdit)
        {
            try
            {

                using (IwillDbEntities Dbc = new IwillDbEntities())
                {

                    if (IsBackPicEdit == "true")
                    {
                        var BackPic = Dbc.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                        var OldPicLocation = Server.MapPath("~/Images/UserImages/" + BackPic.BackgroundImageUrl);
                        System.IO.File.Delete(OldPicLocation);

                        BackPic.BackgroundImageUrl = null;
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

        [HttpGet]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult SaveBackgroundPic(string FileName, string BCimgTOP, string BCimgLeft, string IsBackPicEdit)
        {
            try
            {

                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    //DP image  upload on server

                    var TempLocation = Server.MapPath("~/Images/Temp/" + FileName);
                    var DestLocation = Server.MapPath("~/Images/UserImages/" + FileName);
                    System.IO.File.Copy(TempLocation, DestLocation);
                    System.IO.File.Delete(TempLocation);
                    var data = Dbc.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                    if (data != null)
                    {

                        if (IsBackPicEdit.Trim() == "true" && data.BackgroundImageUrl != null)
                        {
                            var OldPicLocation = Server.MapPath("~/Images/UserImages/" + data.BackgroundImageUrl);
                            System.IO.File.Delete(OldPicLocation);
                        }

                        data.BackgroundImageUrl = FileName;
                        data.BackImgTop = Convert.ToInt32(BCimgTOP);
                        data.BackImgLeft = Convert.ToInt32(BCimgLeft);
                        Dbc.SaveChanges();
                    }

                }
                return Json("saved", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json("Error: on delete<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult UpdateBackImagePostion(string FileName, string BCimgTOP, string BCimgLeft, string IsEdit)
        {
            try
            {

                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    //DP image  upload on server

                    var data = Dbc.Users.Where(i => i.UID == Helper.User.UID && i.BackgroundImageUrl == FileName.Trim()).FirstOrDefault();
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





        #endregion

        #region Supportive Fucntion

        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public bool IsEmailIDRegistered(string EmailAddress)
        {

            var data = DBC.Users.Where(i => i.EmailAddress == EmailAddress.Trim()).FirstOrDefault();
            if (data != null)
                return true;
            return false;


        }

        //public List<City> GetAllCities(long id)
        //{
        //    try
        //    {

        //        List<City> data = DBC.Cities.Where(i => i.FkCTRYID == id).ToList().Select(i => new City() { CTID = i.CTID, CName = i.CName }).ToList();
        //        if (data.Count > 0)

        //        return Json("", JsonRequestBehavior.AllowGet);


        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("error " + ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        [AllowAnonymous]
        //[AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult GetAllCities(long id)
        {
            try
            {

                List<City> data = DBC.Cities.Where(i => i.FkCTRYID == id).ToList().Select(i => new City() { CTID = i.CTID, CName = i.CName }).ToList();
                if (data.Count > 0)
                    return Json(data, JsonRequestBehavior.AllowGet);
                return Json("", JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json("error " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult IsEmailRegistered(string id)
        {
            try
            {
                if (id == null)
                    return Json("empty", JsonRequestBehavior.AllowGet);

                var data = DBC.Users.Where(i => i.EmailAddress == id.Trim()).FirstOrDefault();
                if (data != null)
                    return Json("yes", JsonRequestBehavior.AllowGet);
                else
                    return Json("no", JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {

                return Json("error " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Verify User by Email and Reset Password
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult VerifyUserByEmail()
        {
            try
            {

                var VerificationCode = Request["VerificationCode"];
                var Email = Request["Email"];
                var Code = new Guid(Request["VerificationCode"].ToString().Trim());
                var data = DBC.Users.Where(i => i.EmailAddress == Email.Trim() && i.ActivationCode == Code).FirstOrDefault();
                if (data != null)
                {
                    data.IsFirstLoggedIn = true;
                    DBC.SaveChanges();
                }
                return RedirectToAction("NewUserProfile", "Users");


            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Invalid Verification <br>" + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }
        }

        [AllowAnonymous]
        public ActionResult PasswordRest()
        {
            try
            {

                var VerificationCode = Request["VerificationCode"];
                var Email = Request["Email"];
                var Code = new Guid(Request["VerificationCode"].ToString().Trim());
                var data = DBC.Users.Where(i => i.EmailAddress == Email.Trim() && i.ActivationCode == Code).FirstOrDefault();
                ModelForgetPasswordVerification mod = new ModelForgetPasswordVerification() { Email = data.EmailAddress };

                if (data != null)
                {
                    return View(mod);
                }
                else
                {
                    ViewData["Error"] = "Error: Password Reset Verification";
                    return RedirectToAction("Home", "Iindex");
                }


            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: Password Reset Verification <br>" + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }
        }


        [AllowAnonymous]
        public ActionResult ForgetEmail()
        {
            return View();
        }



        [HttpPost]
        public ActionResult SendPasswordChangeEmail(ModelForgetPassword mod)
        {
            try
            {
                var data = DBC.Users.Where(i => i.EmailAddress == mod.EmailAddress).FirstOrDefault();
                if (data == null)
                {
                    ViewData["Error"] = "Error: there is no user associated with this email";
                    return RedirectToAction("ForgetEmail", "Users");
                }

                string code = data.ActivationCode.ToString();


                string path = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

                string Verifypath = "Users/PasswordRest/?VerificationCode=" + code + "&Email=" + mod.EmailAddress;
                string absolutepath = path + Verifypath;
                string physicalpath = Request.PhysicalApplicationPath + "Content/EmailTemplates/PasswordReset.txt";
                StreamReader sr = new StreamReader(physicalpath);
                string Body = sr.ReadToEnd();
                Body = Body.Replace("<%UserName>", data.FirstName);
                Body = Body.Replace("<%VerificationUrl%>", absolutepath);

                Helper.SendMessage("New Password Reset", "", mod.EmailAddress, Body);

                ViewData["Error"] = "you have successfully send an email to reset your password";
                return RedirectToAction("ForgetEmail", "Users");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("ForgetEmail", "Users");
            }
        }

        public ActionResult ResetUserPassword(ModelForgetPasswordVerification mod)
        {
            try
            {
                var VerificationCode = Request["VerificationCode"];
                var Email = Request["Email"];
                var data = DBC.Users.Where(i => i.EmailAddress == mod.Email).FirstOrDefault();
                if (data == null)
                {
                    ViewData["Error"] = "Error: Invalid access to reset password";
                    return RedirectToAction("PasswordRest", "User");
                }
                data.Password = Helper.Encrypt(mod.Password);
                DBC.SaveChanges();

                ViewData["Error"] = "you have successfully changed your password";
                return View("PasswordRest");
                //return RedirectToAction("PasswordRest", "Users");
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: New Password change <br>" + ex.Message;
                return RedirectToAction("Home", "Iindex");
            }
        }

        #endregion

        #region UserProfile

        [AuthorizeUserAccess(UserRole = "Coach,Student")]

        public ActionResult UserProfile()

        {
            try
            {
                string UserEmail = "";

                if (TempData["UserEmail"] != null)
                    UserEmail = TempData["UserEmail"].ToString();

                if (string.IsNullOrEmpty(UserEmail))
                    UserEmail = Helper.User.Email;

                var data = DBC.Users.Where(i => i.EmailAddress == UserEmail.Trim()).FirstOrDefault();
                if (data == null)
                {
                    ViewData["Error"] = "Error: No User Profile found!";
                    return View();
                }
                if (data != null && (data.IsFirstLoggedIn == false || data.IsFirstLoggedIn == null))
                {
                    TempData["NotActivated"] = "Dear " + data.FirstName + ", you are not activated yet ,get activated by your Email ";
                    return View("NewUserProfile");
                    //return RedirectToAction("Index", "Home");
                }



                ModelUserRegister mod = new ModelUserRegister()
                {
                    FirstName = data.FirstName,
                    MiddleName = data.MiddleName,
                    LastName = data.LastName,
                    DateOfBirth = Convert.ToDateTime(data.DateOfBirth),
                    ContactNumber = data.ContactNumber,
                    FacebookUrl = data.FacebookUrl,
                    AboutMe = data.AboutMe,
                    EmergenyNumber = data.EmergenyNumber,
                    fileName = data.PicUrl,
                    //GID = Convert.ToByte(data.FkGenderID),
                    //CTID = Convert.ToInt64(data.FkCityID),
                    //CTRYID = Convert.ToInt64(data.FkCountryID),
                    Gender = Helper.GetGender(data.FkGenderID),
                    CityName = Helper.GetCityName(data.FkCityID),
                    CountryName = Helper.GetCountryName(data.FkCountryID),
                    BackFileName = data.BackgroundImageUrl,
                    BackImgTop = data.BackImgTop.ToString(),
                    BackImgLeft = data.BackImgLeft.ToString(),
                    FrontImgLeft = data.FrontImgLeft.ToString(),
                    FrontImgTop = data.FrontImgTop.ToString(),

                    //IsStudent = Convert.ToBoolean(data.IsStudent),
                    EmailAddress = data.EmailAddress,
                    LinkedInUrl = data.LinkedInUrl,
                    Nationality = data.Nationality,
                    //MartialStatus = data.MartialStatus , 
                    MotherLanguage = data.MotherLanguage,
                    PlusGoogleUrl = data.PlusGoogleUrl,
                    WebUrl = data.WebUrl,
                    Religion = data.Religion,
                    GitHubUrl = data.GitHubUrl

                };


                return View(mod);
            }
            catch (Exception ex)
            {
                ModelUserRegister mod = new ModelUserRegister();

                ViewData["Error"] = "Error: User Profile <br>" + ex.Message;
                return View(mod);
            }

        }

        [AuthorizeUserAccess(UserRole = "Coach,Student")]

        public ActionResult NewUserProfile()

        {
            try
            {
                //string UserEmail = "";

                //if (TempData["UserEmail"] != null)
                //    UserEmail = TempData["UserEmail"].ToString();

                //if (string.IsNullOrEmpty(UserEmail))
                //    UserEmail = Helper.User.Email;

                var data = DBC.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                if (data == null)
                    throw new Exception("user not found");

                ModelUserRegister mod = new ModelUserRegister()
                {
                    FirstName = data.FirstName,
                    MiddleName = data.MiddleName,
                    LastName = data.LastName,
                    DateOfBirth = Convert.ToDateTime(data.DateOfBirth),
                    ContactNumber = data.ContactNumber,
                    FacebookUrl = data.FacebookUrl,
                    AboutMe = data.AboutMe,
                    EmergenyNumber = data.EmergenyNumber,
                    fileName = data.PicUrl,
                    GID = Convert.ToByte(data.FkGenderID),
                    CTID = Convert.ToInt64(data.FkCityID),
                    CTRYID = Convert.ToInt64(data.FkCountryID),
                    Gender = Helper.GetGender(data.FkGenderID),
                    CityName = Helper.GetCityName(data.FkCityID),
                    CountryName = Helper.GetCountryName(data.FkCountryID),
                    BackFileName = data.BackgroundImageUrl,
                    BackImgTop = data.BackImgTop.ToString(),
                    BackImgLeft = data.BackImgLeft.ToString(),
                    FrontImgLeft = data.FrontImgLeft.ToString(),
                    FrontImgTop = data.FrontImgTop.ToString(),
                    Designation = data.Designation,
                    //IsStudent = Convert.ToBoolean(data.IsStudent),
                    EmailAddress = data.EmailAddress,
                    LinkedInUrl = data.LinkedInUrl,
                    Nationality = data.Nationality,
                    //MartialStatus = data.MartialStatus , 
                    MotherLanguage = data.MotherLanguage,
                    PlusGoogleUrl = data.PlusGoogleUrl,
                    WebUrl = data.WebUrl,
                    Religion = data.Religion,
                    GitHubUrl = data.GitHubUrl,
                    TwitterUrl = data.TwitterUrl

                };

                var UserFllwers = DBC.UserFollowers.Where(i => i.FkUID == Helper.User.UID && i.IsFollowed == true).OrderByDescending(i => i.CreatedOn)
                    .Select(s => new ModelUserFollower()
                    {
                        FollwerDesignation = s.User1.Designation,
                        FollwerFirstName = s.User1.FirstName,
                        FollwerLastName = s.User1.LastName,
                        PicUrl = s.User1.PicUrl,
                        FollwerID = s.FkFollowerID

                    }).Take(3).ToList();

                var UserFllwngs = DBC.UserFollowers.Where(i => i.FkFollowerID == Helper.User.UID && i.IsFollowed == true).OrderByDescending(i => i.CreatedOn)
                    .Select(s => new ModelUserFollower()
                    {
                        FollwerDesignation = s.User.Designation,
                        FollwerFirstName = s.User.FirstName,
                        FollwerLastName = s.User.LastName,
                        PicUrl = s.User.PicUrl,
                        FollwerID = s.UFID

                    }).Take(3).ToList();

                ViewBag.UserFllwers = UserFllwers;
                ViewBag.UserFllwngs = UserFllwngs;
                ViewBag.Country = new SelectList(DBC.Countries, "CTRYID", "CTName");

                if (mod.CTRYID > 0)
                {
                    ViewBag.Country = new SelectList(DBC.Countries, "CTRYID", "CTName", mod.CTRYID);
                    List<City> SlectedCities = DBC.Cities.Where(i => i.FkCTRYID == mod.CTRYID).ToList().Select(i => new City() { CTID = i.CTID, CName = i.CName }).ToList();
                    if (SlectedCities.Count > 0)
                        ViewBag.City = new SelectList(SlectedCities, "CTID", "CName", mod.CTID);

                }

                return View(mod);
            }
            catch (Exception ex)
            {
                ModelUserRegister mod = new ModelUserRegister();

                ViewData["Error"] = "Error: User Profile <br>" + ex.Message;
                return View(mod);
            }

        }



        [AuthorizeUserAccess(UserRole = "Coach,Student")]

        public ActionResult ViewUserProfile()

        {
            try
            {

                long? UID = null;

                if (Request.QueryString["UID"] == null)
                    throw new Exception("user not found");
                else
                    UID = Convert.ToInt64(Request["UID"]);

                var data = DBC.Users.Where(i => i.UID == UID).FirstOrDefault();
                if (data == null)
                    throw new Exception("user not found");

                ModelUserRegister mod = new ModelUserRegister()
                {
                    FirstName = data.FirstName,
                    MiddleName = data.MiddleName,
                    LastName = data.LastName,
                    DateOfBirth = Convert.ToDateTime(data.DateOfBirth),
                    ContactNumber = data.ContactNumber,
                    FacebookUrl = data.FacebookUrl,
                    AboutMe = data.AboutMe,
                    EmergenyNumber = data.EmergenyNumber,
                    fileName = data.PicUrl,
                    GID = Convert.ToByte(data.FkGenderID),
                    CTID = Convert.ToInt64(data.FkCityID),
                    CTRYID = Convert.ToInt64(data.FkCountryID),
                    Gender = Helper.GetGender(data.FkGenderID),
                    CityName = Helper.GetCityName(data.FkCityID),
                    CountryName = Helper.GetCountryName(data.FkCountryID),
                    BackFileName = data.BackgroundImageUrl,
                    BackImgTop = data.BackImgTop.ToString(),
                    BackImgLeft = data.BackImgLeft.ToString(),
                    FrontImgLeft = data.FrontImgLeft.ToString(),
                    FrontImgTop = data.FrontImgTop.ToString(),
                    Designation = data.Designation,
                    //IsStudent = Convert.ToBoolean(data.IsStudent),
                    EmailAddress = data.EmailAddress,
                    LinkedInUrl = data.LinkedInUrl,
                    Nationality = data.Nationality,
                    //MartialStatus = data.MartialStatus , 
                    MotherLanguage = data.MotherLanguage,
                    PlusGoogleUrl = data.PlusGoogleUrl,
                    WebUrl = data.WebUrl,
                    Religion = data.Religion,
                    GitHubUrl = data.GitHubUrl,
                    TwitterUrl = data.TwitterUrl,
                    UID = data.UID
                };

                var UserFllwers = DBC.UserFollowers.Where(i => i.FkUID == UID && i.IsFollowed == true).OrderByDescending(i => i.CreatedOn)
                    .Select(s => new ModelUserFollower()
                    {
                        FollwerDesignation = s.User1.Designation,
                        FollwerFirstName = s.User1.FirstName,
                        FollwerLastName = s.User1.LastName,
                        PicUrl = s.User1.PicUrl,
                        FollwerID = s.FkFollowerID

                    }).Take(3).ToList();

                var UserFllwngs = DBC.UserFollowers.Where(i => i.FkFollowerID == UID && i.IsFollowed == true).OrderByDescending(i => i.CreatedOn)
                    .Select(s => new ModelUserFollower()
                    {
                        FollwerDesignation = s.User.Designation,
                        FollwerFirstName = s.User.FirstName,
                        FollwerLastName = s.User.LastName,
                        PicUrl = s.User.PicUrl,
                        FollwerID = s.UFID

                    }).Take(3).ToList();

                ViewBag.UserFllwers = UserFllwers;
                ViewBag.UserFllwngs = UserFllwngs;
                ViewBag.Country = new SelectList(DBC.Countries, "CTRYID", "CTName");

                if (mod.CTRYID > 0)
                {
                    ViewBag.Country = new SelectList(DBC.Countries, "CTRYID", "CTName", mod.CTRYID);
                    List<City> SlectedCities = DBC.Cities.Where(i => i.FkCTRYID == mod.CTRYID).ToList().Select(i => new City() { CTID = i.CTID, CName = i.CName }).ToList();
                    if (SlectedCities.Count > 0)
                        ViewBag.City = new SelectList(SlectedCities, "CTID", "CName", mod.CTID);

                }

                return View(mod);
            }
            catch (Exception ex)
            {
                ModelUserRegister mod = new ModelUserRegister();

                ViewData["Error"] = "Error: User Profile <br>" + ex.Message;
                return View(mod);
            }

        }



        [HttpPost]
        public JsonResult UpdateUserSocialLinksLocationContact(ModelUserProfile ModData)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    var data = Dbc.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                    if (data == null)
                        throw new Exception("data not found");

                    data.ContactNumber = ModData.ContactNumber;
                    data.FkCityID = ModData.CTID;
                    data.FkCountryID = ModData.CTRYID;

                    data.FacebookUrl = ModData.FacebookUrl;
                    data.TwitterUrl = ModData.TwitterUrl;
                    data.PlusGoogleUrl = ModData.PlusGoogleUrl;
                    data.LinkedInUrl = ModData.LinkedInUrl;
                    ModData.CityName = GetCityName(ModData.CTID);
                    ModData.CountryName = GetCountryName(ModData.CTRYID);
                    Dbc.SaveChanges();

                    return Json(ModData, JsonRequestBehavior.AllowGet);
                }


                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on update User social Links , contact details and location <br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public ActionResult EditProfile()
        {
            try
            {

                if (!HttpContext.User.Identity.IsAuthenticated)
                    throw new Exception("Invalid user or not authenticated");

                var data = DBC.Users.Where(i => i.EmailAddress == Helper.User.Email).FirstOrDefault();
                if (data == null)
                    throw new Exception("no data found ");


                ModelUserRegister mod = new ModelUserRegister()
                {
                    FirstName = data.FirstName,
                    MiddleName = data.MiddleName,
                    LastName = data.LastName,
                    DateOfBirth = Convert.ToDateTime(data.DateOfBirth),
                    ContactNumber = data.ContactNumber,
                    FacebookUrl = data.FacebookUrl,
                    AboutMe = data.AboutMe,
                    EmergenyNumber = data.EmergenyNumber,
                    fileName = data.PicUrl,
                    Gender = Helper.GetGender(data.FkGenderID),
                    CityName = Helper.GetCityName(data.FkCityID),
                    CountryName = Helper.GetCountryName(data.FkCountryID),
                    EmailAddress = data.EmailAddress,
                    LinkedInUrl = data.LinkedInUrl,
                    Nationality = data.Nationality,
                    CTID = Convert.ToInt64(data.FkCityID),
                    CTRYID = Convert.ToInt64(data.FkCountryID),
                    GID = Convert.ToByte(data.FkGenderID),
                    MotherLanguage = data.MotherLanguage,
                    PlusGoogleUrl = data.PlusGoogleUrl,
                    WebUrl = data.WebUrl,
                    Religion = data.Religion,
                    GitHubUrl = data.GitHubUrl

                };

                ViewBag.Gender = new SelectList(DBC.Genders.ToList(), "GID", "Name", mod.GID);

                if (mod.CTRYID > 0)
                {
                    ViewBag.Country = new SelectList(DBC.Countries, "CTRYID", "CTName", mod.CTRYID);
                    List<City> SlectedCities = DBC.Cities.Where(i => i.FkCTRYID == mod.CTRYID).ToList().Select(i => new City() { CTID = i.CTID, CName = i.CName }).ToList();
                    if (SlectedCities.Count > 0)
                        ViewBag.City = new SelectList(SlectedCities, "CTID", "CName", mod.CTID);
                }
                return View(mod);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUserProfile(ModelUserRegister mod)
        {
            try
            {

                var user = DBC.Users.Where(i => i.EmailAddress == mod.EmailAddress).FirstOrDefault();
                if (user == null)
                    throw new Exception("Error: invalid user ");


                //user.EmailAddress = mod.EmailAddress;
                user.FirstName = mod.FirstName;
                user.MiddleName = mod.MiddleName;
                user.LastName = mod.LastName;
                //  user.Password = Helper.Encrypt(mod.Password); // here encryotion is required 
                user.FkGenderID = mod.GID;
                user.WebUrl = mod.WebUrl;
                user.FacebookUrl = mod.FacebookUrl;
                user.PlusGoogleUrl = mod.PlusGoogleUrl;
                user.LinkedInUrl = mod.LinkedInUrl;
                //user.PicUrl = mod.file.FileName;
                user.ContactNumber = mod.ContactNumber;
                user.FkCityID = mod.CTID;
                user.FkCountryID = mod.CTRYID;
                user.Religion = mod.Religion;
                user.MotherLanguage = mod.MotherLanguage;
                user.AboutMe = mod.AboutMe;
                //user.ActivationCode = ActivationCode;
                user.IsStudent = mod.IsStudent;
                user.DateOfBirth = mod.DateOfBirth;
                user.PicUrl = mod.fileName;
                // user.IsFirstLoggedIn = false;
                user.UpdatedBy = mod.UID;
                user.UpdatedOn = DateTime.Now;

                DBC.SaveChanges();
                // SendAccountEmailVerification(ActivationCode, mod.EmailAddress, mod.FirstName);
                TempData["success"] = "Dear " + mod.FirstName + ",  your profile has been successfull updated";
                return RedirectToAction("NewUserProfile", "Users");

                //var res = "Dear " + mod.FirstName + ",  your profile has been successfully updated";
                //return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //var res = "Error: Cannot update User Profile" + ex.Message;
                //return Json(res, JsonRequestBehavior.AllowGet);
                ViewData["Error"] = "Error: Dear " + mod.FirstName + ",  your profile has not updated" + ex.Message;
                return RedirectToAction("NewUserProfile", "Users");
            }

        }


        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult UpdateAboutMe(string AboutMe)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    var obj = Dbc.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                    if (obj == null)
                        throw new Exception("user not found ");
                    obj.AboutMe = AboutMe;
                    Dbc.SaveChanges();
                    return Json("updated^" + AboutMe, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                return Json("Error: on getting list of User Education <br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult UpdateProfile(ModelUserProfile ModProfle)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    var obj = Dbc.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                    if (obj == null)
                        throw new Exception("user not found ");
                    obj.FirstName = ModProfle.FirstName;
                    obj.LastName = ModProfle.LastName;
                    obj.Designation = ModProfle.Designation;
                    obj.Religion = ModProfle.Religion;
                    obj.MotherLanguage = ModProfle.MotherLanguage;

                    Dbc.SaveChanges();
                    return Json(ModProfle, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                return Json("Error: on getting list of User Education <br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult UserEducationList()
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {

                    List<ModelUserEducation> EmpHis = DBC.UserEducations.Where(i => i.FkUID == Helper.User.UID).Select(
                                                                                                                s => new ModelUserEducation()
                                                                                                                {
                                                                                                                    EducationType = s.FkETID,
                                                                                                                    NameOfInstitute = s.NameOfInstitute,
                                                                                                                    CourseTitle = s.CourseTitle,
                                                                                                                    FkETID = s.FkETID,
                                                                                                                    MajorSubjects = s.MajorSubjects,
                                                                                                                    EducationLocation = s.Location,
                                                                                                                    EducationFromYear = s.FromYear,
                                                                                                                    EducationFromMonth = s.FromMonth,
                                                                                                                    EducationToYear = s.ToYear,
                                                                                                                    EducationToMonth = s.ToMonth,
                                                                                                                    EducationIsPresent = s.IsPresent,
                                                                                                                    EducationDescription = s.Descriptipn,
                                                                                                                    FkUID = s.FkUID,
                                                                                                                    UEDID = s.UEDID
                                                                                                                }).OrderByDescending(i => i.EducationFromYear).ToList();
                    return Json(EmpHis, JsonRequestBehavior.AllowGet);

                }


                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on getting list of User Ecperience <br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult AddEducation(ModelUserEducation ModUsrEdu)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    if (ModUsrEdu.UEDID != null)
                    {
                        long ID = Convert.ToInt64(ModUsrEdu.UEDID);
                        var data = Dbc.UserEducations.Where(i => i.UEDID == ID).FirstOrDefault();
                        if (data == null)
                            throw new Exception("user experience data does not  found");

                        data.NameOfInstitute = ModUsrEdu.NameOfInstitute;
                        data.FromMonth = ModUsrEdu.EducationFromMonth;
                        data.ToMonth = ModUsrEdu.EducationToMonth;
                        //FromDay = ModUsrEdu.FromDay,
                        //ToDay = ModUsrEdu.ToDay,
                        data.FromYear = ModUsrEdu.EducationFromYear;
                        data.ToYear = ModUsrEdu.EducationToYear;
                        data.IsPresent = ModUsrEdu.EducationIsPresent;
                        data.MajorSubjects = ModUsrEdu.MajorSubjects;
                        data.Location = ModUsrEdu.EducationLocation;
                        data.Descriptipn = ModUsrEdu.EducationDescription;
                        data.FkUID = Helper.User.UID;
                        data.FkETID = ModUsrEdu.EducationType;
                        data.CourseTitle = ModUsrEdu.CourseTitle;
                    }
                    else
                    {
                        UserEducation UserEduc = new UserEducation()
                        {
                            NameOfInstitute = ModUsrEdu.NameOfInstitute,
                            FromMonth = ModUsrEdu.EducationFromMonth,
                            ToMonth = ModUsrEdu.EducationToMonth,
                            //FromDay = ModUsrEdu.FromDay,
                            //ToDay = ModUsrEdu.ToDay,
                            FromYear = ModUsrEdu.EducationFromYear,
                            ToYear = ModUsrEdu.EducationToYear,
                            IsPresent = ModUsrEdu.EducationIsPresent,
                            MajorSubjects = ModUsrEdu.MajorSubjects,
                            Location = ModUsrEdu.EducationLocation,
                            Descriptipn = ModUsrEdu.EducationDescription,
                            FkUID = Helper.User.UID,
                            FkETID = ModUsrEdu.EducationType,
                            CourseTitle = ModUsrEdu.CourseTitle

                        };
                        Dbc.UserEducations.Add(UserEduc);
                    }
                    Dbc.SaveChanges();
                }
                return Json("saved", JsonRequestBehavior.AllowGet);

                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on save<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult DeleteEducation(long EID)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    // long ID = Convert.ToInt64(ModUsrExp.UEXID);

                    var UserEdu = Dbc.UserEducations.Where(i => i.UEDID == EID).FirstOrDefault();
                    if (UserEdu == null)
                        throw new Exception("No User Education found");

                    Dbc.UserEducations.Remove(UserEdu);
                    Dbc.SaveChanges();
                }
                return Json("deleted", JsonRequestBehavior.AllowGet);

                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: on updation <br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult UserExprnceList()
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {

                    List<ModelUserExperience> EmpHis = DBC.UserExperiences.Where(i => i.FkUID == Helper.User.UID).Select(
                                                                                                                s => new ModelUserExperience()
                                                                                                                {
                                                                                                                    CompanyName = s.CompanyName,
                                                                                                                    UserPost = s.UserPost,
                                                                                                                    Location = s.Location,
                                                                                                                    FromYear = s.FromYear,
                                                                                                                    FromMonth = s.FromMonth,
                                                                                                                    ToYear = s.ToYear,
                                                                                                                    ToMonth = s.ToMonth,
                                                                                                                    IsPresent = s.IsPresent,
                                                                                                                    Description = s.Description,
                                                                                                                    FkUID = s.FkUID,
                                                                                                                    UEXID = s.UEXID.ToString()
                                                                                                                }).OrderByDescending(i => i.FromYear).ToList();
                    return Json(EmpHis, JsonRequestBehavior.AllowGet);

                }


                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Error: on getting list of User Ecperience <br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult AddExperience(ModelUserExperience ModUsrExp)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    if (ModUsrExp.UEXID != null)
                    {
                        long ID = Convert.ToInt64(ModUsrExp.UEXID);
                        var data = Dbc.UserExperiences.Where(i => i.UEXID == ID).FirstOrDefault();
                        if (data == null)
                            throw new Exception("user experience data does not  found");

                        data.CompanyName = ModUsrExp.CompanyName;
                        data.UserPost = ModUsrExp.UserPost;
                        data.Location = ModUsrExp.Location;
                        data.FromYear = ModUsrExp.FromYear;
                        data.FromMonth = ModUsrExp.FromMonth;
                        data.ToYear = ModUsrExp.ToYear;
                        data.ToMonth = ModUsrExp.ToMonth;
                        data.FromDay = ModUsrExp.FromDay;
                        data.ToDay = ModUsrExp.ToDay;
                        data.IsPresent = ModUsrExp.IsPresent;
                        data.Description = ModUsrExp.Description;

                    }
                    else
                    {
                        UserExperience UserExperience = new UserExperience()
                        {
                            CompanyName = ModUsrExp.CompanyName,
                            UserPost = ModUsrExp.UserPost,
                            Location = ModUsrExp.Location,
                            FromYear = ModUsrExp.FromYear,
                            FromMonth = ModUsrExp.FromMonth,
                            ToYear = ModUsrExp.ToYear,
                            ToMonth = ModUsrExp.ToMonth,
                            FromDay = ModUsrExp.FromDay,
                            ToDay = ModUsrExp.ToDay,
                            IsPresent = ModUsrExp.IsPresent,
                            Description = ModUsrExp.Description,
                            FkUID = Helper.User.UID
                        };
                        Dbc.UserExperiences.Add(UserExperience);
                    }

                    Dbc.SaveChanges();
                }
                return Json("saved", JsonRequestBehavior.AllowGet);

                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: on save<br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Coach,Student")]
        public JsonResult DeleteEmployment(long EID)
        {
            try
            {
                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    // long ID = Convert.ToInt64(ModUsrExp.UEXID);

                    var UserExprnce = Dbc.UserExperiences.Where(i => i.UEXID == EID).FirstOrDefault();
                    if (UserExprnce == null)
                        throw new Exception("No User Education found");

                    Dbc.UserExperiences.Remove(UserExprnce);
                    Dbc.SaveChanges();
                }
                return Json("deleted", JsonRequestBehavior.AllowGet);

                //return Json("Error: not deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: on updation <br> " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        public string LastColummSession
        {
            get
            {
                if (Session["LastCoumn"] == null)
                    return null;
                return (string)Session["LastCoumn"];
            }
            set
            {
                Session["LastCoumn"] = value;
            }
        }
        public string SortOrderSession
        {
            get
            {
                if (Session["sortOrder"] == null)
                    return null;
                return (string)Session["sortOrder"];
            }
            set
            {
                Session["sortOrder"] = value;
            }
        }


        #region UserRole

        [AuthorizeUserAccess(UserRole = "Admin")]
        [HttpGet]
        public ActionResult AssignUserRoles(string CurrnetColumn, string UserName, string RoleID, string IsActive, int? PageNumber)
        {
            try
            {

                if (CurrnetColumn != null)
                {

                    if (CurrnetColumn != LastColummSession) // this condition runs when you click another column for sorting
                    {
                        LastColummSession = CurrnetColumn;
                        SortOrderSession = "Asc";
                    }
                    else
                    {
                        if (SortOrderSession == "Asc") // when you press the same column again for sorting 
                            SortOrderSession = "Des";
                        else SortOrderSession = "Asc";
                    }

                }
                else
                {
                    if (LastColummSession != null) // this condition occurs when navigate to another page 
                        CurrnetColumn = LastColummSession;

                    else  // set the criteria when first time dataload

                    {
                        SortOrderSession = "Asc";
                        CurrnetColumn = "FirstName";
                        LastColummSession = "FirstName";
                    }
                }
                bool? IsActiv = null;
                if (IsActive == "0" || string.IsNullOrEmpty(IsActive))
                    IsActiv = null;
                else if (IsActive == "1") IsActiv = true; else IsActiv = false;

                int? RID = null;
                if (!string.IsNullOrEmpty(RoleID))
                    RID = Convert.ToInt32(RoleID);
                IPagedList<spSearhUser_Result> data = null;

                switch (CurrnetColumn)
                {
                    case "FirstName":
                        if (SortOrderSession.Equals("Asc"))
                            data = DBC.spSearhUser(UserName, RID, IsActiv).OrderBy(i => i.FirstName).ToList().ToPagedList(PageNumber ?? 1, 2);
                        else
                            data = DBC.spSearhUser(UserName, RID, IsActiv).OrderByDescending(i => i.FirstName).ToList().ToPagedList(PageNumber ?? 1, 2);
                        break;

                    case "EmailAddress":
                        if (SortOrderSession.Equals("Asc"))
                            data = DBC.spSearhUser(UserName, RID, IsActiv).OrderBy(i => i.EmailAddress).ToList().ToPagedList(PageNumber ?? 1, 2);
                        else
                            data = DBC.spSearhUser(UserName, RID, IsActiv).OrderByDescending(i => i.EmailAddress).ToList().ToPagedList(PageNumber ?? 1, 2);
                        break;

                    case "Default":
                        data = DBC.spSearhUser(UserName, RID, IsActiv).OrderBy(i => i.FirstName).ToList().ToPagedList(PageNumber ?? 1, 2);
                        break;
                }
                return View(data);
            }
            catch (Exception ex)
            {
                IPagedList<spSearhUser_Result> data = null;
                ViewData["Error"] = "Error: there is an error " + ex.Message;
                return View(data);
            }
        }

        [AuthorizeUserAccess(UserRole = "Admin")]
        [HttpPost]
        public ActionResult UpdateRolesToUsers(string UID, string RID)
        {
            try
            {
                Int64 UserID = Convert.ToInt64(UID);
                var data = DBC.UserRoles.Where(i => i.FKUID == UserID).FirstOrDefault();
                if (data == null)
                    return Json("Error: user not found", JsonRequestBehavior.AllowGet);

                data.FKRID = Convert.ToInt64(RID);
                DBC.SaveChanges();

                return Json("updated", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: there is an error " + ex.Message, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult GetUserQuesForRegistration()
        {
            try
            {
                bool IsStudent = Convert.ToBoolean(Request.QueryString["IsStudent"]);
                List<UserQuestion> QuestionList = null;
                if (IsStudent)
                    QuestionList = DBC.UserQuestions.Where(i => i.IsStudent == true && i.IsActive == true).ToList();
                else
                    QuestionList = DBC.UserQuestions.Where(i => i.IsStudent == false && i.IsActive == true).ToList();

                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: " + ex.Message;
                return View();
            }
        }

        #endregion

    }
}
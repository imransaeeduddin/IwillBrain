using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace IWill_MvcApplication.Models
{
    public class Helper
    {
        public static string Client_ID = ConfigurationManager.AppSettings["google_clientId"].ToString();
        public static string Return_url = ConfigurationManager.AppSettings["google_RedirectUrl"].ToString();

        public enum ERoles : int { Admin = 1, Student = 2, Coach = 3 };

        public static List<UserFollower> GetUserFollowers(long UID)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    return DBC.UserFollowers.Where(i => i.FkUID == UID).ToList();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public static List<UserFollower> GetUserFollowing(long UID)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    return DBC.UserFollowers.Where(i => i.FkFollowerID == UID).ToList();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public static SelectList GetYear()
        {
            try
            {
                IwillDbEntities DBC = new IwillDbEntities();
                return new SelectList(DBC.Years.ToList(), "Year1", "Year1", 0);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public static SelectList GetEducationTypes()
        {
            try
            {
                IwillDbEntities DBC = new IwillDbEntities();
                return new SelectList(DBC.EducationTypes.ToList(), "ETID", "EName", 0);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public static ModelUserProfile AddUserSocialLinks(ModelUserProfile Userlinks, string IsAdd)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {

                    //var Links = new ModelUserSocialLinks() { };
                    if (Helper.User == null)
                        throw new Exception("User not found");

                    var User = DBC.Users.Where(i => i.UID == Helper.User.UID).FirstOrDefault();
                    User.FacebookUrl = Userlinks.FacebookUrl;
                    User.LinkedInUrl = Userlinks.LinkedInUrl;
                    User.PlusGoogleUrl = Userlinks.PlusGoogleUrl;
                    User.TwitterUrl = Userlinks.TwitterUrl;
                    var Isrecord = DBC.SaveChanges();

                    return Userlinks;

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public static List<UserGivenQuesAn> GetQuestionsOPtions(Int64? QID)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    return DBC.UserGivenQuesAns.Where(data => data.FkUQID == QID).ToList();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public static List<Role> GetRoles()
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    return DBC.Roles.ToList();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public static List<SelectListItem> GetActiveInActiveSelectList()
        {
            try
            {

                List<SelectListItem> lst = new List<SelectListItem>() {
                        new SelectListItem() { Value = "0", Text = "All", Selected = true },
                        new SelectListItem() { Value = "1", Text = "Active", Selected = true },
                        new SelectListItem() { Value = "2", Text = "InActive", Selected = false }
                                                                      };
                //lst.Add(new SelectListItem() { Value = "0", Text = "All", Selected = true });
                //lst.Add(new SelectListItem() { Value = "0", Text = "All", Selected = true });
                //lst.Add(new SelectListItem() { Value = "2", Text = "InActive", Selected = false });

                return lst;
                //    return DBC.Roles.ToList();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public static SelectList GetQuestionairTypes()
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    return new SelectList(DBC.QuestionTypes.ToList(), "QTID", "QTName", 0);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public static SelectList GetCourseCategories()
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    return new SelectList(DBC.CourseCategories.ToList(), "CCID", "CCName", 0);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public static SelectList GetRoles(long SelectedIndex)
        {
            try
            {
                IwillDbEntities DBC = new IwillDbEntities();
                return new SelectList(DBC.Roles.ToList(), "RID", "RName", SelectedIndex);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public static long GetPrimaryRoleID(int RoleID)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())

                {
                    var data = DBC.Roles.Where(i => i.RoleID == RoleID).FirstOrDefault();
                    if (data != null)
                        return data.RID;
                    return 0;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public static string GetGender(byte? GID)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    var data = DBC.Genders.Where(i => i.GID == GID).FirstOrDefault();
                    if (data != null)
                        return data.Name;
                    return "";
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static string GetCityName(long? CTID)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {

                    var data = DBC.Cities.Where(i => i.CTID == CTID).FirstOrDefault();
                    if (data != null)
                        return data.CName;
                    return "";
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static string GetCountryName(long? CTRYID)
        {
            using (IwillDbEntities DBC = new IwillDbEntities())
            {
                try
                {
                    var data = DBC.Countries.Where(i => i.CTRYID == CTRYID).FirstOrDefault();
                    if (data != null)
                        return data.CTName;
                    return "";

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
        }

        public static List<UserQuestionDetail> GetQuestionDetaills(Int64 UQID)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    var data = DBC.UserQuestionDetails.Where(i => i.FkQID == UQID).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

          
        }

        public static ModelViewRegQuestion GetViewRegQuestions(bool IsStudent)
        {
            using (IwillDbEntities DBC = new IwillDbEntities())
            {
                ModelViewRegQuestion mod = new ModelViewRegQuestion();

                // ModelDataViewRegQuestion mod = new ModelDataViewRegQuestion();
                List<ModelUserRegQuestion> MQuesList = DBC.UserQuestions.Where(i => i.IsStudent == IsStudent && i.IsActive == true).Select(
                                                                                                            s => new ModelUserRegQuestion()
                                                                                                            {
                                                                                                                FkUQType = s.FkUQType,
                                                                                                                IsActive = s.IsActive,
                                                                                                                UQID = s.UQID,
                                                                                                                IsStudent = s.IsStudent == null ? false : true,
                                                                                                                UQuestion = s.UQuestion

                                                                                                            }).ToList();

                foreach (var item in MQuesList)
                {
                    List<ModelUserQuestionDetail> modOptlist = DBC.UserQuestionDetails.Where(i => i.FkQID == item.UQID && i.IsActive == true).Select(
                                                                                                                s => new ModelUserQuestionDetail()
                                                                                                                {
                                                                                                                    FkQID = s.FkQID,
                                                                                                                    IsActive = s.IsActive,
                                                                                                                    IsRight = s.IsRight == null ? false : true,
                                                                                                                    QuesOptionName = s.QuesOptionName,
                                                                                                                    UQDID = s.UQDID
                                                                                                                }).ToList();


                    item.UserQuestionDetail = modOptlist;




                }
                mod.LstQuestion = MQuesList;

                return mod;
            }
        }
        public static List<UserQuestionDetail> GetRegQuestionDetails(Int64 UQID)
        {
            using (IwillDbEntities DBC = new IwillDbEntities())
            {
                var data = DBC.UserQuestionDetails.Where(i => i.FkQID == UQID && i.IsActive == true).ToList();
                return data;
            }
        }




        public static void SendMessage(string Subject, string From, string To, string Body)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                if (string.IsNullOrEmpty(From))
                    msg.From = new MailAddress("imransaeeduddin@gmail.com");
                else
                    msg.From = new MailAddress(From);
                msg.To.Add(new MailAddress(To));
                msg.Subject = Subject;
                msg.Body = Body;


                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("imransaeeduddin@gmail.com", "Imran123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Send(msg);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }
        public static string Encrypt(string clearText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
          
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
          
        }

        public static bool CreateCookie(UserLoginData user, HttpResponseBase response)
        {
            try
            {
                bool result = false;

                // Create the authentication ticket with custom user data.
                var serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(user);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        user.UName,
                        DateTime.Now,
                        DateTime.Now.AddDays(30),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);
                // Encrypt the ticket.

                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                result = true;

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            
        }

        public static UserLoginData User
        {
            get
            {
                try
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        // The user is authenticated. Return the user from the forms auth ticket.
                        return ((MyPrincipal)(HttpContext.Current.User)).User;
                    }
                    else if (HttpContext.Current.Items.Contains("User"))
                    {
                        // The user is not authenticated, but has successfully logged in.
                        return (UserLoginData)HttpContext.Current.Items["User"];
                    }

                    return null;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }

        }

        public static byte AuthenticateGender(string Gender)
        {
            if (Gender.IndexOf("Male") == 0 || Gender.IndexOf("male") == 0)
                return 1;
            if (Gender.IndexOf("Female") == 0 || Gender.IndexOf("female") == 0)
                return 2;
            if (Gender.IndexOf("Shemale") == 0 || Gender.IndexOf("shemale") == 0)
                return 3;
            return 0;
        }

        public static int GetCityID(string CityName)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    var cupper = CityName.ToUpper();
                    var data = DBC.Cities.Where(i => CityName.ToUpper().IndexOf(i.CName.ToUpper()) == 0).FirstOrDefault();
                    //foreach(var i in DBC.Cities)
                    //{
                    //    var dupper = i.CName.ToUpper();

                    //    if (cupper.IndexOf(dupper) == 0)
                    //    {

                    //    }

                    //}
                    if (data != null)
                        return Convert.ToInt32(data.CTID);
                    return 0;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        public static int GetCountryID(string CountryName)
        {
            try
            {
                using (IwillDbEntities DBC = new IwillDbEntities())
                {
                    var data = DBC.Countries.Where(i => CountryName.ToUpper().IndexOf(i.CTName.ToUpper()) == 0).FirstOrDefault();
                    if (data != null)
                        return Convert.ToInt32(data.CTRYID);
                    return 0;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        //public static void SendEmail(string body , string message)
        //{

        //}

    }


    public class UserLoginData
    {
        public long UID { get; set; }
        public string UName { get; set; }

        public string LName { get; set; }
        public string Email { get; set; }

        public long RoleID { get; set; }
        public string RoleName { get; set; }

        public string Image { get; set; }

        public int? BackImgTop { get; set; }

        public int? BackImgLeft { get; set; }

        public int? FrontImgTop { get; set; }
        public int? FrontImgLeft { get; set; }

        public bool IsFirstLoggedIn { get; set; }

        //public Nullable<int> CreatedBy { get; set; }
        //public Nullable<System.DateTime> CreatedOn { get; set; }

        //public Nullable<System.DateTime> JoiningDate { get; set; }
        //public Nullable<int> EmpID { get; set; }
        //public string Deptrment { get; set; }
        //public List<VMCreatePage> lstPags { get; set; }
    }
    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(IIdentity identity)
        {
            Identity = identity;
        }
        public IIdentity Identity
        {
            get;
            private set;
        }
        public UserLoginData User { get; set; }
        public bool IsInRole(string role)
        {
            return true;
        }

    }

    public class GmailLoginData
    {
        public string id { get; set; }
        public string email { get; set; }
        public string verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string link { get; set; }
        public string picture { get; set; }
        public string gender { get; set; }
        public string locale { get; set; }

    }

    public class FacebookLoginData
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Verified_email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Gender { get; set; }


    }

    public class LinkedInLoginData
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }

        public string ProfileUrl { get; set; }
        public string HeadLine { get; set; }
        public string Industry { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string Gender { get; set; }


    }

    public static class UseDefinedRoles
    {
        public static string Admin = "Admin";
        public static string Coach = "Coach";
        public static string Student = "Student";
    }

}
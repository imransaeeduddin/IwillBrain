using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static IWill_MvcApplication.Models.Helper;
using PagedList;
using PagedList.Mvc;

using IWill_MvcApplication.Models;
using System.Web.Script.Serialization;

namespace IWill_MvcApplication.Controllers
{
    public class UserRegQuestionController : Controller
    {
        private IwillDbEntities DBC = new IwillDbEntities();

        // GET: UserQuiz
        [AuthorizeUserAccess(UserRole = "Admin")]
        public ActionResult ViewCreateQuesForUserRegistration()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: to Create Regestration Question " + ex.Message;
                return View();
            }

        }

        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Admin")]
        public ActionResult InsertQuesForUserRegistration(ModelUserRegQuestion mod)
        {
            using (var transaction = DBC.Database.BeginTransaction())
            {
                try
                {
                    if (mod == null)
                        throw new Exception("There is no Questionairs to save ");


                    var QuestionObj = new UserQuestion()
                    {
                        UQuestion = mod.UQuestion,
                        FkUQType = mod.FkUQType,
                        IsStudent = mod.IsStudent,
                        IsActive = mod.IsActive == null ? true : mod.IsActive,
                        CreatedOn = DateTime.Now,
                        CreatedBy = Helper.User.UID
                    };

                    DBC.UserQuestions.Add(QuestionObj);
                    DBC.SaveChanges();

                    if (mod.FkUQType != 2)
                    {
                        var QuestionID = DBC.UserQuestions.Local[0].UQID;
                        foreach (var item in mod.UserQuestionDetail)
                        {
                            var ObjDetail = new UserQuestionDetail()
                            {
                                QuesOptionName = item.QuesOptionName,
                                IsRight = item.IsRight,
                                IsActive = item.IsActive == null ? true : item.IsActive,
                                FkQID = QuestionID,
                            };
                            DBC.UserQuestionDetails.Add(ObjDetail);
                            DBC.SaveChanges();
                        }
                    }

                    TempData["success"] = "Question has been created";
                    transaction.Commit();
                    return RedirectToAction("ViewCreateQuesForUserRegistration", "UserRegQuestion");
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewData["Error"] = "Error: to Save data " + ex.Message;
                    return RedirectToAction("ViewCreateQuesForUserRegistration", "UserRegQuestion");

                }

            }


        }

        [AuthorizeUserAccess(UserRole = "Admin")]
        public ActionResult ViewUserQuestionsList(string Question, string Criteria, string QuestionTyp, string IsActiv, int? PageNumber)
        {
            try
            {
                string UserQuestion = null;
                bool? IsStudent = null;
                int? QuestionType = null;
                bool? IsActive = null;

                if (string.IsNullOrEmpty(Question) || Question.Length == 0)
                    UserQuestion = null;
                else
                    UserQuestion = Question;

                if (!string.IsNullOrEmpty(Criteria))
                {
                    if (Criteria == "1") IsStudent = true;
                    if (Criteria == "2") IsStudent = false;
                }

                if (!string.IsNullOrEmpty(QuestionTyp))
                    QuestionType = Convert.ToInt32(QuestionTyp);

                //if (!string.IsNullOrEmpty(IsActiv))
                //    IsActive = true;
                //else IsActive = false;

                if (!string.IsNullOrEmpty(IsActiv))
                {
                    if (IsActiv == "1") IsActive = true;
                    if (IsActiv == "2") IsActive = false;
                }

                int PageSize = 5;

                IPagedList<spSearchUserRegestrationQuestions_Result> data = DBC.spSearchUserRegestrationQuestions(UserQuestion, IsStudent, QuestionType, IsActive).ToList().ToPagedList(PageNumber ?? 1, PageSize);
                ViewData["UserQuestions"] = data;
                return View();
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: to Save data " + ex.Message;
                return RedirectToAction("ViewUserQuestionsList", "UserRegQuestion");
            }
        }


        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult UpdateUserRegesterQuestion(ModelUserRegQuestion GetUserQues)
        {
            try
            {

                using (IwillDbEntities data = new IwillDbEntities())
                {
                    var UserQuestion = data.UserQuestions.Where(i => i.UQID == GetUserQues.UQID).FirstOrDefault();
                    if (UserQuestion == null)
                        return Json("No Record Found", JsonRequestBehavior.AllowGet);

                    UserQuestion.UQuestion = GetUserQues.UQuestion;
                    UserQuestion.IsStudent = GetUserQues.IsStudent;

                    int r = data.SaveChanges();

                    if (r >= 0)
                    {
                        TempData["success"] = "Question has Successfully updated";
                        return Json("Record Updated Successfully", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //ViewData["Error"]= "Error: on updating the question";
                        return Json("error on inserting", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: " + ex.Message;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }



        }

        [AuthorizeUserAccess(UserRole = "Admin")]
        [HttpGet]
        public JsonResult ActivateQuestion(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Question ID Requried");

                //if (string.IsNullOrEmpty(UQID))
                //    throw new Exception("Activation can not be empty");

                Int64 QID = Convert.ToInt64(id);
                //bool IsActivated = Convert.ToBoolean(IsActivate);

                using (IwillDbEntities data = new IwillDbEntities())
                {
                    var UserQuestion = data.UserQuestions.Where(i => i.UQID == QID).FirstOrDefault();
                    if (UserQuestion == null)
                        return Json("No Record Found", JsonRequestBehavior.AllowGet);

                    UserQuestion.IsActive = true;

                    int r = data.SaveChanges();

                    if (r >= 0)
                    {
                        TempData["success"] = "Question has Successfully Activated";
                        TempData["UQID"] = id;

                        return Json("Record Activated Successfully", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //ViewData["Error"]= "Error: on updating the question";
                        return Json("error on Activating", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: " + ex.Message;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }



        }

        [AuthorizeUserAccess(UserRole = "Admin")]
        [HttpGet]
        public JsonResult DeactivateQuestion(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Question ID Requried");

                //if (string.IsNullOrEmpty(UQID))
                //    throw new Exception("Activation can not be empty");

                Int64 QID = Convert.ToInt64(id);
                //bool IsActivated = Convert.ToBoolean(IsActivate);

                using (IwillDbEntities data = new IwillDbEntities())
                {
                    var UserQuestion = data.UserQuestions.Where(i => i.UQID == QID).FirstOrDefault();
                    if (UserQuestion == null)
                        return Json("No Record Found", JsonRequestBehavior.AllowGet);

                    UserQuestion.IsActive = false;

                    int r = data.SaveChanges();
                    TempData["UQID"] = id;

                    if (r >= 0)
                    {
                        TempData["success"] = "Question has Successfully Deactivated";
                        TempData["UQID"] = id;
                        return Json("Record Deactivated Successfully", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //ViewData["Error"]= "Error: on updating the question";
                        return Json("error on Deactivating", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: " + ex.Message;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }



        }

        [AuthorizeUserAccess(UserRole = "Admin")]
        [HttpGet]
        public JsonResult ActivateQuestionDetailOption(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Question ID Requried");

                Int64 UQDID = Convert.ToInt64(id);


                using (IwillDbEntities data = new IwillDbEntities())
                {
                    var UserQuestOption = data.UserQuestionDetails.Where(i => i.UQDID == UQDID).FirstOrDefault();
                    if (UserQuestOption == null)
                        return Json("No Record Found", JsonRequestBehavior.AllowGet);


                    Int64 QID = Convert.ToInt64(UserQuestOption.FkQID);
                    var Ques = DBC.UserQuestions.Where(i => i.UQID == QID).FirstOrDefault();

                    if (Ques == null)
                        return Json("No Question Detail Found", JsonRequestBehavior.AllowGet);

                    if (Ques.FkUQType == 1 && UserQuestOption.IsRight == true)
                    {
                        return Json("Cannot Activated MCQz option, because its value is true", JsonRequestBehavior.AllowGet);
                    }


                    UserQuestOption.IsActive = true;

                    int r = data.SaveChanges();

                    if (r >= 0)
                    {
                        TempData["success"] = "Question Option has Successfully Activated";
                        TempData["UQDID"] = id;
                        return Json("Question Option has Successfully Activated", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //ViewData["Error"]= "Error: on updating the question";
                        return Json("error on Activating option", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: " + ex.Message;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }



        }

        [AuthorizeUserAccess(UserRole = "Admin")]
        [HttpGet]
        public JsonResult DeactivateQuestionDetailOption(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Question ID Requried");

                Int64 UQDID = Convert.ToInt64(id);


                using (IwillDbEntities data = new IwillDbEntities())
                {
                    var UserQuestOption = data.UserQuestionDetails.Where(i => i.UQDID == UQDID).FirstOrDefault();
                    if (UserQuestOption == null)
                        return Json("No Record Found", JsonRequestBehavior.AllowGet);

                    Int64 QID = Convert.ToInt64(UserQuestOption.FkQID);


                    var Ques = DBC.UserQuestions.Where(i => i.UQID == QID).FirstOrDefault();

                    if (Ques == null)
                        return Json("No Question Detail Found", JsonRequestBehavior.AllowGet);

                    if (Ques.FkUQType == 1 && UserQuestOption.IsRight == true)
                        return Json("Cannot Deactivated MCQz option, because its value is true", JsonRequestBehavior.AllowGet);


                    else if (data.UserQuestionDetails.Where(i => i.FkQID == QID).ToList().Count == 2)

                        return Json("Cannot Deactivated option, because there are only two items ramining", JsonRequestBehavior.AllowGet);


                    UserQuestOption.IsActive = false;

                    int r = data.SaveChanges();

                    if (r >= 0)
                    {
                        TempData["success"] = "Question Option has Successfully Deactivated";
                        TempData["UQDID"] = id;
                        return Json("Question Option has Successfully Deactivated", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //ViewData["Error"]= "Error: on updating the question";
                        return Json("error on Activating option", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: " + ex.Message;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }



        }


        [AuthorizeUserAccess(UserRole = "Admin")]
        public JsonResult UpdateUserRegesterQuestionOption(ModelUserQuestionDetail GetUserQuesOption)
        {
            try
            {

                using (IwillDbEntities data = new IwillDbEntities())
                {
                    var UserQuestionOption = data.UserQuestionDetails.Where(i => i.UQDID == GetUserQuesOption.UQDID).FirstOrDefault();
                    if (UserQuestionOption == null)
                        return Json("No Record Found", JsonRequestBehavior.AllowGet);

                    UserQuestionOption.QuesOptionName = GetUserQuesOption.QuesOptionName;

                    int r = data.SaveChanges();

                    if (r >= 0)
                    {
                        TempData["success"] = "Question option has Successfully updated";
                        TempData["UQDID"] = GetUserQuesOption.UQDID;
                        return Json("Record Updated Successfully", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //ViewData["Error"]= "Error: on updating the question";
                        return Json("error on inserting", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {

                ViewData["Error"] = "Error: " + ex.Message;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }



        }



        [HttpPost]
        [AuthorizeUserAccess(UserRole = "Admin")]
        public ActionResult AddQuestionOptionMcqz(ModelUserRegQuestion mod)
        {
            using (var transaction = DBC.Database.BeginTransaction())
            {
                try
                {
                    long? effectedID = null;
                    if (mod == null)
                        throw new Exception("There is no Questionairs to save ");

                    foreach (var item in mod.UserQuestionDetail)
                    {
                        var ObjDetail = new UserQuestionDetail();


                        ObjDetail.QuesOptionName = item.QuesOptionName;
                        if (item.IsRight)
                        {
                            var DBoptions = DBC.UserQuestionDetails.Where(i => i.FkQID == mod.UQID && i.IsRight == true).FirstOrDefault();
                            DBoptions.IsRight = false;
                            DBC.SaveChanges();
                        }
                        ObjDetail.IsRight = item.IsRight;
                        ObjDetail.IsActive = item.IsActive == null ? true : item.IsActive;
                        ObjDetail.FkQID = mod.UQID;

                        DBC.UserQuestionDetails.Add(ObjDetail);
                        DBC.SaveChanges();
                        effectedID = DBC.UserQuestionDetails.Local[0].UQDID;
                    }


                    TempData["success"] = "New Question MCQz Options has been created";
                    TempData["UQID"] = mod.UQID;
                    TempData["option"] = effectedID;
                    transaction.Commit();
                    return RedirectToAction("ViewUserQuestionsList", "UserRegQuestion");
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewData["Error"] = "Error: to Save data " + ex.Message;
                    return RedirectToAction("ViewUserQuestionsList", "UserRegQuestion");

                }

            }


        }

        [AuthorizeUserAccess(UserRole = "Admin")]
        public ActionResult AddQuestionOptionChoices(ModelUserRegQuestion mod)
        {
            using (var transaction = DBC.Database.BeginTransaction())
            {
                try
                {
                    long? effectedID = null;
                    if (mod == null)
                        throw new Exception("There is no choices to save ");

                    foreach (var item in mod.UserQuestionDetail)
                    {
                        var ObjDetail = new UserQuestionDetail();
                        ObjDetail.QuesOptionName = item.QuesOptionName;
                        ObjDetail.IsRight = item.IsRight;
                        ObjDetail.IsActive = item.IsActive == null ? true : item.IsActive;
                        ObjDetail.FkQID = mod.UQID;

                        DBC.UserQuestionDetails.Add(ObjDetail);
                        DBC.SaveChanges();
                        effectedID = DBC.UserQuestionDetails.Local[0].UQDID;
                    }


                    TempData["success"] = "New Question Choice Options has been created";
                    TempData["UQID"] = mod.UQID;
                    TempData["option"] = effectedID;
                    transaction.Commit();
                    return RedirectToAction("ViewUserQuestionsList", "UserRegQuestion");
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewData["Error"] = "Error: to Save data " + ex.Message;
                    return RedirectToAction("ViewUserQuestionsList", "UserRegQuestion");

                }

            }


        }

        //[AuthorizeUserAccess(UserRole = "Coach,Student")]
        //public ActionResult ViewGetRegestrationQuestion()
        //{
        //    try
        //    {
        //        //if (TempData["IsStudent"] == null)
        //        //    throw new Exception("Invalid Access");

        //        //if (TempData["UserID"] == null)
        //        //    throw new Exception("Invalid Access");

        //        //string ReqIsStudent = TempData["IsStudent"].ToString();
        //        //string ReqUserID = TempData["UserID"].ToString();



        //        string ReqIsStudent = Request.QueryString["IsStudent"].ToString();
        //        string ReqUserID = Request.QueryString["UserID"].ToString();

        //        bool IsStudent; Int64 UserID;
        //        if (!Boolean.TryParse(ReqIsStudent, out IsStudent))
        //            throw new Exception("Invalid Student Value ");

        //        if (!Int64.TryParse(ReqUserID, out UserID))
        //            throw new Exception("Invalid Student ID");

        //        using (IwillDbEntities dbc = new IwillDbEntities())
        //        {
        //            var Udata = dbc.UserGivenQuesAns.Where(i => i.FkUserID == UserID).FirstOrDefault();
        //            if (Udata != null)
        //                throw new Exception("You have already registered for the questions");

        //        }

        //        ViewBag.IsStudent = IsStudent;
        //        ViewBag.UserID = UserID;
        //        ModelViewRegQuestion data = Helper.GetViewRegQuestions(IsStudent);

        //        // List< ModelUserRegQuestion> lstQuestions = DBC.UserQuestions
        //        return View(data);
        //    }
        //    catch (Exception ex)
        //    {

        //        ViewData["Error"]= "Error: to View Regestration Question " + ex.Message;
        //        //return RedirectToAction("ViewGetRegestrationQuestion", "UserRegQuestion");
        //        return View();
        //    }
        //}


        //[AuthorizeUserAccess(UserRole = "Coach,Student")]
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ViewGetRegestrationQuestion( string IsStudnt)
        {
            try
            {

                bool IsStudent; //Int64 UserID;
                if (!Boolean.TryParse(IsStudnt, out IsStudent))
                    throw new Exception("Invalid Student Value ");

                ViewBag.IsStudent = IsStudent;

                ModelViewRegQuestion data = Helper.GetViewRegQuestions(IsStudent);
                // var serializer = new JavaScriptSerializer();
                //serializer.Serialize(data.LstQuestion)
                return Json(data.LstQuestion, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error:" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        //[HttpPost]
        //[AuthorizeUserAccess(UserRole = "Coach,Student")]
        //public ActionResult AttempUserRegestrationQuestion(ModelViewRegQuestion mod)
        //{
        //    try
        //    {

        //        if (mod.UserID == null || mod.UserID == 0)
        //            throw new Exception("Invalid user");
        //        Int64 UID = Convert.ToInt64(mod.UserID);

        //        int QuesWeight = mod.LstQuestion.Where(i => i.FkUQType != 2).ToList().Count;
        //        float CountRight = 0;
        //        using (IwillDbEntities Dbc = new IwillDbEntities())
        //        {
        //            using (var transaction = Dbc.Database.BeginTransaction())
        //            {
        //                foreach (var ques in mod.LstQuestion)
        //                {
        //                    float CountTotOption = 0F;
        //                    float CountRightOptions = 0F;
        //                    float PerOptionWeight = 0.00F;

        //                    UserGivenQuesAn data = new UserGivenQuesAn();
        //                    data.FkUserID = UID;
        //                    data.FkUQID = ques.UQID;
        //                    data.CreatedOn = DateTime.Now;
        //                    data.FkQTID = ques.FkUQType;
        //                    data.Question = ques.UQuestion;
        //                    if (ques.FkUQType == 2)
        //                    {

        //                        //UserGivenQuesAn data = new UserGivenQuesAn();
        //                        //data.FkUserID = UID;
        //                        //data.FkUQID = ques.UQID;
        //                        //data.CreatedOn = DateTime.Now;
        //                        //data.FkQTID = ques.FkUQType;

        //                        data.Answer = ques.Answer;
        //                        Dbc.UserGivenQuesAns.Add(data);
        //                        Dbc.SaveChanges();
        //                    }

        //                    else// if (ques.FkUQType == 1 || ques.FkUQType == 3)
        //                    {
        //                        CountTotOption = ques.UserQuestionDetail.Count;
        //                        //For multiple options
        //                        CountRightOptions = Dbc.UserQuestionDetails.Where(i => i.FkQID == ques.UQID && i.IsRight == true && i.IsActive == true).ToList().Count;
        //                        PerOptionWeight = CountRightOptions / (CountRightOptions * CountRightOptions);

        //                        foreach (var option in ques.UserQuestionDetail)
        //                        {
        //                            //UserGivenQuesAn data = new UserGivenQuesAn();
        //                            //data.FkUserID = UID;
        //                            //data.FkUQID = ques.UQID;
        //                            //data.CreatedOn = DateTime.Now;
        //                            //data.FkQTID = ques.FkUQType;

        //                            data.OptionName = option.QuesOptionName;
        //                            data.FkUQDID = option.UQDID;
        //                            var RightAnsDetl = Dbc.UserQuestionDetails.Where(i => i.UQDID == option.UQDID && i.IsRight == true && i.IsActive == true).FirstOrDefault();
        //                            if (RightAnsDetl != null)
        //                                data.IsOptionRight = RightAnsDetl.IsRight;
        //                            else data.IsOptionRight = false;

        //                            if (ques.FkUQType == 1) // checking for MCQz
        //                            {

        //                                if (RightAnsDetl != null && option.IsRight)
        //                                {
        //                                    data.IsUserRight = option.IsRight;
        //                                    CountRight = CountRight + 1;
        //                                }
        //                                else data.IsUserRight = option.IsRight;

        //                            }

        //                            if (ques.FkUQType == 3)
        //                            {

        //                                if (RightAnsDetl != null && option.IsRight)
        //                                {
        //                                    data.IsUserRight = option.IsRight;
        //                                    CountRight += PerOptionWeight;
        //                                }
        //                                //else if (RightAnsDetl == null && option.IsRight)
        //                                //{
        //                                //    data.IsUserRight = option.IsRight;
        //                                //    CountRight -= PerOptionWeight;
        //                                //}
        //                                else data.IsUserRight = option.IsRight;

        //                            }

        //                            Dbc.UserGivenQuesAns.Add(data);
        //                            Dbc.SaveChanges();

        //                        }

        //                    }


        //                }

        //                float Perce = (CountRight / QuesWeight) * 100;
        //                var userdata = Dbc.Users.Where(i => i.UID == UID).FirstOrDefault();
        //                userdata.RegQuesPercentage = Convert.ToDecimal(Perce);
        //                Dbc.SaveChanges();
        //                transaction.Commit();
        //                TempData["Reg"] = "Regestered";
        //                RedirectToAction("Users", "UserProfile");
        //            }
        //        }
        //        return Json("done", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("Error: " + ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [AuthorizeUserAccess(UserRole = "Admin")]
        public ActionResult ViewUsersGivenRegQuestions(string UserID)

        {
            try
            {
                if (Request["UserID"] == null)
                    throw new Exception();
                Int64 UID;
                Int64.TryParse(Request["UserID"], out UID);

                using (IwillDbEntities Dbc = new IwillDbEntities())
                {
                    //var data = Dbc.UserGivenQuesAns.Where(i => i.FkUserID == UID).GroupBy(g=>g.FkUQID).GroupBy(g=>g.Question).Select(s => s.FirstOrDefault().FkUQID).ToList();
                    var data = Dbc.spGetRegUserQuestion(UID).ToList();
                    ViewBag.UserRegQues = data;
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error: to View User Given Regestration Question " + ex.Message;
                return View();
            }


        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IWill_MvcApplication.Models
{

    //public class ModelDataViewRegQuestion
    //{
    //    public List<UserQuestion> LstDataQuestion { get; set; }
    //}

    public class ModelViewRegQuestion
    {
        public List<ModelUserRegQuestion> LstQuestion { get; set; }
        public Int64? UserID { get; set; }

    }


    public class ModelUserRegQuestion
    {
        public long UQID { get; set; }

        [Display(Name = "Question")]
        /*[Required(ErrorMessage = "Question field cannot be empty")]*/
        public string UQuestion { get; set; }
        public string Answer { get; set; }

        [Display(Name = "Question type")]
        //[Required(ErrorMessage = "please select Question type")]
        public Nullable<long> FkUQType { get; set; }

        [Display(Name = "For Student")]
        public bool IsStudent { get; set; }
        public Nullable<bool> IsActive { get; set; }


        public List<ModelUserQuestionDetail> UserQuestionDetail { get; set; }

    }

    public class ModelUserQuestionDetail
    {
        public long UQDID { get; set; }
        public string hdnUQDID { get; set; }

        public Nullable<long> FkQID { get; set; }

        //[Required (ErrorMessage = "Option Name Required")]
        //[Display(Name = "Option Name")]
        public string QuesOptionName { get; set; }

        [Display(Name = "Is Right")]
        public bool IsRight { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }




}
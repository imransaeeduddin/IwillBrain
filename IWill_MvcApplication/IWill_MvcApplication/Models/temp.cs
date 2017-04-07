namespace IWill_MvcApplication.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class temp : DbContext
    {
        public temp()
            : base("name=temp")
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<CoachTaughtBy> CoachTaughtBies { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseCategory> CourseCategories { get; set; }
        public virtual DbSet<CourseCoach> CourseCoaches { get; set; }
        public virtual DbSet<CourseDetail> CourseDetails { get; set; }
        public virtual DbSet<CourseMaterialType> CourseMaterialTypes { get; set; }
        public virtual DbSet<CourseOutline> CourseOutlines { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<MartialStatu> MartialStatus { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionsDetail> QuestionsDetails { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Quiz> Quizs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserEnrollCourse> UserEnrollCourses { get; set; }
        public virtual DbSet<UserGivenQuesAn> UserGivenQuesAns { get; set; }
        public virtual DbSet<UserQuestionDetail> UserQuestionDetails { get; set; }
        public virtual DbSet<UserQuestion> UserQuestions { get; set; }
        public virtual DbSet<UserQuizAnswer> UserQuizAnswers { get; set; }
        public virtual DbSet<UserQuizAttemp> UserQuizAttemps { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .Property(e => e.CName)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CIPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<CoachTaughtBy>()
                .Property(e => e.CTName)
                .IsUnicode(false);

            modelBuilder.Entity<CoachTaughtBy>()
                .Property(e => e.CTUniversityName)
                .IsUnicode(false);

            modelBuilder.Entity<CoachTaughtBy>()
                .Property(e => e.PicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.CTName)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.CntyIPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Cities)
                .WithOptional(e => e.Country)
                .HasForeignKey(e => e.FkCTRYID);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseName)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseSessionID)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseCode)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Commitment)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseLanguage)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.LogoUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.HowToPass)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.LearningLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CoachUniversityName)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.BackgroundPicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CerticationDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseImportance)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.CoachTaughtBies)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.FKCID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.CourseDetails)
                .WithOptional(e => e.Course)
                .HasForeignKey(e => e.FKCID);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Quizs)
                .WithOptional(e => e.Course)
                .HasForeignKey(e => e.FKCID);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.UserEnrollCourses)
                .WithOptional(e => e.Course)
                .HasForeignKey(e => e.FKCID);

            modelBuilder.Entity<CourseCategory>()
                .Property(e => e.CCName)
                .IsUnicode(false);

            modelBuilder.Entity<CourseCategory>()
                .Property(e => e.CCDescription)
                .IsUnicode(false);

            modelBuilder.Entity<CourseCategory>()
                .HasMany(e => e.Courses)
                .WithOptional(e => e.CourseCategory)
                .HasForeignKey(e => e.FkCCID);

            modelBuilder.Entity<CourseCoach>()
                .Property(e => e.CTUniversityName)
                .IsUnicode(false);

            modelBuilder.Entity<CourseCoach>()
                .Property(e => e.CTName)
                .IsUnicode(false);

            modelBuilder.Entity<CourseCoach>()
                .Property(e => e.PicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<CourseDetail>()
                .Property(e => e.SesssionName)
                .IsUnicode(false);

            modelBuilder.Entity<CourseDetail>()
                .Property(e => e.SesssionCode)
                .IsUnicode(false);

            modelBuilder.Entity<CourseDetail>()
                .Property(e => e.SessionDescription)
                .IsUnicode(false);

            modelBuilder.Entity<CourseDetail>()
                .Property(e => e.SessionTotalSummary)
                .IsUnicode(false);

            modelBuilder.Entity<CourseDetail>()
                .HasMany(e => e.CourseOutlines)
                .WithOptional(e => e.CourseDetail)
                .HasForeignKey(e => e.FKCDID);

            modelBuilder.Entity<CourseMaterialType>()
                .Property(e => e.MaterialName)
                .IsUnicode(false);

            modelBuilder.Entity<CourseMaterialType>()
                .HasMany(e => e.CourseOutlines)
                .WithOptional(e => e.CourseMaterialType)
                .HasForeignKey(e => e.FKCMTID);

            modelBuilder.Entity<CourseOutline>()
                .Property(e => e.OName)
                .IsUnicode(false);

            modelBuilder.Entity<CourseOutline>()
                .Property(e => e.Olink)
                .IsUnicode(false);

            modelBuilder.Entity<CourseOutline>()
                .Property(e => e.ODescription)
                .IsUnicode(false);

            modelBuilder.Entity<CourseOutline>()
                .Property(e => e.LecCode)
                .IsUnicode(false);

            modelBuilder.Entity<Gender>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<MartialStatu>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<Question>()
                .Property(e => e.QuesDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.QuestionsDetails)
                .WithOptional(e => e.Question)
                .HasForeignKey(e => e.FKQSID);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.UserQuizAnswers)
                .WithOptional(e => e.Question)
                .HasForeignKey(e => e.FkQSID);

            modelBuilder.Entity<QuestionsDetail>()
                .Property(e => e.QuesOptionName)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionsDetail>()
                .HasMany(e => e.UserQuizAnswers)
                .WithOptional(e => e.QuestionsDetail)
                .HasForeignKey(e => e.FkQDID);

            modelBuilder.Entity<QuestionType>()
                .Property(e => e.QTName)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionType>()
                .HasMany(e => e.Questions)
                .WithOptional(e => e.QuestionType)
                .HasForeignKey(e => e.FKQTID);

            modelBuilder.Entity<QuestionType>()
                .HasMany(e => e.UserQuestions)
                .WithOptional(e => e.QuestionType)
                .HasForeignKey(e => e.FkUQType);

            modelBuilder.Entity<Quiz>()
                .Property(e => e.QName)
                .IsUnicode(false);

            modelBuilder.Entity<Quiz>()
                .Property(e => e.QuizDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Quiz>()
                .Property(e => e.QuizCode)
                .IsUnicode(false);

            modelBuilder.Entity<Quiz>()
                .HasMany(e => e.Questions)
                .WithOptional(e => e.Quiz)
                .HasForeignKey(e => e.FKQZID);

            modelBuilder.Entity<Quiz>()
                .HasMany(e => e.UserQuizAttemps)
                .WithOptional(e => e.Quiz)
                .HasForeignKey(e => e.FkQZID);

            modelBuilder.Entity<Role>()
                .Property(e => e.RName)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.UserRoles)
                .WithRequired(e => e.Role)
                .HasForeignKey(e => e.FKRID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.AboutMe)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.WebUrl)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FacebookUrl)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PlusGoogleUrl)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.GitHubUrl)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.EmergenyNumber)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Nationality)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Religion)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.MotherLanguage)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstLoginUrl)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LinkedInUrl)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserEnrollCourses)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.FKUID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserGivenQuesAns)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.FkUserID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserQuizAttemps)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.FkUID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoles)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.FKUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserEnrollCourse>()
                .Property(e => e.CardName)
                .IsUnicode(false);

            modelBuilder.Entity<UserEnrollCourse>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<UserGivenQuesAn>()
                .Property(e => e.Answer)
                .IsUnicode(false);

            modelBuilder.Entity<UserGivenQuesAn>()
                .Property(e => e.OptionName)
                .IsUnicode(false);

            modelBuilder.Entity<UserGivenQuesAn>()
                .Property(e => e.Question)
                .IsUnicode(false);

            modelBuilder.Entity<UserQuestionDetail>()
                .Property(e => e.QuesOptionName)
                .IsUnicode(false);

            modelBuilder.Entity<UserQuestionDetail>()
                .HasMany(e => e.UserGivenQuesAns)
                .WithOptional(e => e.UserQuestionDetail)
                .HasForeignKey(e => e.FkUQDID);

            modelBuilder.Entity<UserQuestion>()
                .Property(e => e.UQuestion)
                .IsUnicode(false);

            modelBuilder.Entity<UserQuestion>()
                .HasMany(e => e.UserGivenQuesAns)
                .WithOptional(e => e.UserQuestion)
                .HasForeignKey(e => e.FkUQID);

            modelBuilder.Entity<UserQuestion>()
                .HasMany(e => e.UserQuestionDetails)
                .WithOptional(e => e.UserQuestion)
                .HasForeignKey(e => e.FkQID);

            modelBuilder.Entity<UserQuizAnswer>()
                .Property(e => e.QAns)
                .IsUnicode(false);

            modelBuilder.Entity<UserQuizAttemp>()
                .Property(e => e.Grade)
                .IsUnicode(false);

            modelBuilder.Entity<UserQuizAttemp>()
                .HasMany(e => e.UserQuizAnswers)
                .WithOptional(e => e.UserQuizAttemp)
                .HasForeignKey(e => e.FkUQID);
        }
    }
}

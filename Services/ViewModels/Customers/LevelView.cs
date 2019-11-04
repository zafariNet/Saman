using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Model.Customers;

namespace Services.ViewModels.Customers
{
    public class LevelView : BaseView
    {
        public Guid LevelTypeID { get; set; }

        [Display(Name = "نوع چرخه")]
        public string LevelTypeTitle { get; set; }

        [Display(Name = "عنوان")]
        public string LevelTitle { get; set; }

        [Display(Name = "اسم مستعار")]
        public string LevelNikname { get; set; }

        [Display(Name = "پیامک هنگام ورود")]
        public bool OnEnterSendSMS { get; set; }

        [Display(Name = "ایمیل هنگام ورود")]
        public bool OnEnterSendEmail { get; set; }

        [Display(Name = "رویداد ورود")]
        public string OnEnter { get; set; }

        [Display(Name = "رویداد خروج")]
        public string OnExit { get; set; }

        [Display(Name = "متن ایمیل")]
        public string EmailText { get; set; }

        [Display(Name = "متن پیامک")]
        public string SMSText { get; set; }

        [Display(Name = "مراحل اولیه")]
        public bool IsFirstLevel { get; set; }

        [Display(Name = "آپشن ها")]
        public string Options { get; set; }

        [Display(Name = "غیر فعال")]
        public bool Discontinued { get; set; }

        [Display(Name = "مسئول مرحله")]
        public string LevelStaffID { get; set; }

        [Display(Name = "مسئول مرحله")]
        public string LevelStaffName { get; set; }

        public bool HasRequireNetwork { get; set; }
        public int SortOrder { get; set; }

        internal GraphicalPropertiesView GraphicalPropertiesV { get; set; }

        public GraphicalPropertiesView GraphicalPropertiesView
        {
            get
            {
                if (GraphicalPropertiesV == null)
                    GraphicalPropertiesV = new GraphicalPropertiesView();
                return GraphicalPropertiesV;
            }
            set
            {
                GraphicalPropertiesV = value;
            }
        }

        internal LevelOptionsView LevelOptionsV { get; set; }
        public LevelOptionsView LevelOptionsView
        {
            get
            {
                if (LevelOptionsV == null)
                    LevelOptionsV = new LevelOptionsView();
                return LevelOptionsV;
            }
            set
            {
                LevelOptionsV = value;
            }
        }

        //IEnumerables

        [Display(Name = "مشتریان و مرحله")]
        public IEnumerable<CustomerLevelView> CustomerLevels { get; protected set; }

        [Display(Name = "")]
        public IEnumerable<LevelConditionView> LevelConditions { get; set; }

        [Display(Name = "نت ها")]
        public IEnumerable<NoteView> Notes { get; protected set; }

        /// <summary>
        /// ایجاد یک پشتیبانی به محض ورود به مرحله
        /// </summary>
        public bool CreateSupportOnEnter { get; set; }

        //[Display(Name = "مراحل بعد")]
        //public IEnumerable<LevelSummaryView> NextLevels { get; set; }

    }

    //public class LevelSummaryView
    //{
    //    public Guid LevelID { get; set; }

    //    public string LevelTitle { get; set; }
    //}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class LevelConditionView : BaseView
    {
        public Guid LevelID { get; set; }

        [Display(Name = "مرحله")]
        public string LevelTitle { get; set; }

        //public Guid ConditionID { get; set; }

        //[Display(Name = "شرط")]
        //public string ConditionTitle { get; set; }

        [Display(Name = "شرط")]
        [UIHint("ClientCondition")]
        public ConditionView Condition { get; set; }
    }

}

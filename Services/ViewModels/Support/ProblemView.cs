using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Support
{
    public class ProblemView : BaseView
    {
        public Guid CustomerID { get; set; }

        [Display(Name = "مشتری")]
        public string CustomerName { get; set; }

        [Display(Name = "عنوان")]
        public string ProblemTitle { get; set; }

        [Display(Name = "شرح مشکل")]
        public string ProblemDescription { get; set; }

        [Display(Name = "اهمیت")]
        public short Priority { get; set; }

        [Display(Name = "وضعیت")]
        public short State { get; set; }
    }
}

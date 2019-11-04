using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class CustomerLevelView : BaseView
    {
        public Guid LevelID { get; set; }

        public string LevelTitle { get; set; }

        [Display(Name = "مشتری")]
        public string CustomerName { get; set; }
        
        public Guid CustomerID { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        public int WaitingDays { get; set; }
    }
}

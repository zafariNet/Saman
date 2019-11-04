using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class LevelLevelView : BaseView
    {
        public Guid LevelID { get; set; }

        [Display(Name = "عنوان مرحله")]
        public string LevelTitle { get; set; }

        public Guid RelatedLevelID { get; set; }

        [Display(Name = "عنوان مرحله بعد")]
        public string RelatedLevelTitle { get; set; }
    }

    public class LevelLevelView2
    {
        public Guid ID { get; set; }
        public Guid MainLevelID { get; set; }

        public Guid NextLevelID { get; set; }
    }

    public class LevelLevelView3
    {
        public Guid source { get; set; }

        public Guid target { get; set; }
    }
}

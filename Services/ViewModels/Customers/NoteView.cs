using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class NoteView : BaseView
    {
        public Guid CustomerID { get; set; }

        [Display(Name = "مشتری")]
        public string CustomerName { get; set; }

        public Guid LevelID { get; set; }

        [Display(Name = "مرحله")]
        public string LevelTitle { get; set; }

        [Display(Name = "یادداشت")]
        public string NoteDescription { get; set; }
    }
}

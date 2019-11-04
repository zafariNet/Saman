#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Services.ViewModels.Customers
{
    public class DocumentView : BaseView
    {
        /// <summary>
        /// آیدی مشتری
        /// </summary>
        public Guid CustomerID { get; set; }

        [Display(Name = "مشتری")]
        public string CustomerName { get; set; }

        [Display(Name = "نام مدرک")]
        public string DocumentName { get; set; }

        [Display(Name = "تصویر")]
        public string Photo { get; set; }

        [Display(Name = "نوع عکس")]
        public string ImageType { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
    }
}

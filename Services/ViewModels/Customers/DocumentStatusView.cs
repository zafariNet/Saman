#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Services.ViewModels.Customers
{
    public class DocumentStatusView : BaseView
    {
        [Display(Name = "وضعیت")]
        public string DocumentStatusName { get; set; }

        [Display(Name = "پیش فرض")]
        public bool DefaultStatus { get; set; }

        [Display(Name = "کامل بودن")]
        public bool CompleteStatus { get; set; }

        [Display(Name = "ترتیب")]
        public int SortOrder { get; set; }


        //IEnumerables

        //[Display(Name = "مشتریان")]
        //public IEnumerable<CustomerView> Customers { get; protected set; }
    }
}

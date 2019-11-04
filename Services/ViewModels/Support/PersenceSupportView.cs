using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Support
{
    public class PersenceSupportView : BaseView
    {
        public Guid CustomerID { get; set; }

        [Display(Name = "مشتری")]
        public string CustomerName { get; set; }

        public Guid InstallerID { get; set; }

        [Display(Name = "نصاب")]
        public string InstallerName { get; set; }

        [Display(Name = "نوع خدمات")]
        public short SupportType { get; set; }

        [Display(Name = "شرح مشکل")]
        public string Problem { get; set; }

        [Display(Name = "تاریخ هماهنگی")]
        public string PlanDate { get; set; }

        [Display(Name = "ساعت هماهنگی از")]
        public string PlanTimeFrom { get; set; }

        [Display(Name = "ساعت هماهنگی تا")]
        public string PlanTimeTo { get; set; }

        [Display(Name = "توضیحات")]
        public string PlanNote { get; set; }

        [Display(Name = "تحویل شد")]
        public bool Delivered { get; set; }

        [Display(Name = "مبلغ")]
        public long ReceivedCost { get; set; }

        [Display(Name = "خدمات اضافه")]
        public long ReceivedCostForExtraServices { get; set; }

        [Display(Name = "اینترنت وصل شد")]
        public bool ConnectedToInternet { get; set; }

        [Display(Name = "تاریخ تحویل")]
        public string DeliverDate { get; set; }

        [Display(Name = "ساعت تحویل")]
        public string DeliverTime { get; set; }

        [Display(Name = "توضیحات تحویل")]
        public string DeliverNote { get; set; }
    }
}

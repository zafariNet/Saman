using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class StoreView : BaseView
    {
        public Guid OwnerEmployeeID { get; set; }

        [Display(Name = "متعلق به")]
        public string OwnerEmployeeName { get; set; }

        [Display(Name = "نام انبار")]
        public string StoreName { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        public Guid StoreID { get; set; }
        //IEnumerables

        [Display(Name = "انبار کالاها")]
        public IEnumerable<StoreProductView> StoreProducts { get; protected set; }
    }
}

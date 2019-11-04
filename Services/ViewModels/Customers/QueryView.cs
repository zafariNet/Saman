using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class QueryView : BaseView
    {
        [Display(Name = "عنوان نما")]
        public string Title { get; set; }

        // xType
        public string xType { get; set; }

        [Display(Name = "متن کوئری")]
        public string QueryText { get; set; }

        [Display(Name = "پارامتر")]
        public string PrmDefinition { get; set; }

        [Display(Name = "مقدار پارامتر")]
        public string PrmValues { get; set; }
        // ستونهای قابل نمایش در این نما
        public string Columns { get; set; }

        public int CustomerCount { get; set; }

        /// <summary>
        /// بازیابی اولیه
        /// </summary>
        public bool PreLoad { get; set; }
        //IEnumerables

        //[Display(Name = "پرسمان مشتری ها")]
        //public IEnumerable<QueryEmployeeView> QueryEmployees { get; protected set; }
    }
}

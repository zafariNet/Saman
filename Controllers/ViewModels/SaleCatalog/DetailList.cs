using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Controllers.ViewModels.SaleCatalog
{
    public class DetailList
    {
        public Guid SaleDetailID { get; set; }

        [DisplayName("شرح")]
        public string SaleDetailName { get; set; }
    }
}

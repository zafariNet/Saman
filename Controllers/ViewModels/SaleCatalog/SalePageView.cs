using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;
using Services.ViewModels.Store;
using Controllers.Controllers;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.SaleCatalog
{
    public class SalePageView : BasePageView
    {
        public IEnumerable<ClientSaleDetailViewModel> ClientSaleDetailViewModels { get; set; }

        public CustomerView CustomerView { get; set; }

        //public IEnumerable<ClientSaleViewModel> ClientSaleViewModels { get; set; }

        public long SaleTotal { get; set; }



        //public Guid SaleDetailID { get; set; }

        //public string SaleDetailName { get; set; }

        //public long UnitPrice { get; set; }

        //public long MaxDiscount { get; set; }

        //public long Imposition { get; set; }

    }
}

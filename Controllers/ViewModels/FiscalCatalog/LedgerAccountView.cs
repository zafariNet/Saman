using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Model.Fiscals;
using Services.ViewModels.Customers;
using Services.ViewModels.Fiscals;

namespace Controllers.ViewModels.FiscalCatalog
{
    class LedgerAccountView
    {
       public IEnumerable<FiscalView> FiscalViews { get; set; }
       
        public int Count { get; set; }

       public CustomerView CustomerView { get; set; }

    }
}

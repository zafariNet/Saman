using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using Services.Messaging;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class CustomerCreatesuccess : BasePageView
    {
        public LevelView LevelView { get; set; }

        public CustomerView CustomerView { get; set; }

        public GeneralResponse AddLevelResponse { get; set; }

        public bool TempDataIsNull { get; set; }
    }
}

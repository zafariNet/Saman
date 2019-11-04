using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddLevelConditionRequest
    {
        public Guid LevelID { get; set; }

        public Guid ConditionID { get; set; }

        public Guid CreateEmployeeID { get; set; }
    }

}

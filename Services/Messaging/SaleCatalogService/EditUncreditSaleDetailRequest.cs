using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class EditUncreditSaleDetailRequest : AddSaleDetailBaseRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }

        public Guid UncreditServiceID { get; set; }
    }
}

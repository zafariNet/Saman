using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class DeleteSaleDetailRequest
    {
        public IEnumerable<DeleteRequest> deleteProductSaleDetailRequests { get; set; }

        public IEnumerable<DeleteRequest> deleteCreditSaleDetailRequests { get; set; }

        public IEnumerable<DeleteRequest> deleteUncreditSaleDetailRequests { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class AddNetworkCreditRequestOld
    {
        public Guid NetworkID { get; set; }
        public long Amount { get; set; }
        public string InvestDate { get; set; }
        public Guid FromAccountID { get; set; }
        public string ToAccount { get; set; }
        public string TransactionNo { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddNetworkCreditRequest
    {
        public bool Type { get; set; }
        public Guid NetworkID { get; set; }
        public long Amount { get; set; }
        public string InvestDate { get; set; }
        public Guid FromAccountID { get; set; }
        public string ToAccount { get; set; }
        public string TransactionNo { get; set; }
        public string Note { get; set; }
    }
}

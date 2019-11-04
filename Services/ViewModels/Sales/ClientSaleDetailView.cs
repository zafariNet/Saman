using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Sales
{
    public class ClientSaleDetailView : ProductionView
    {
        public Guid RowID { get; set; }

        public int Units { get; set; }

        public long Discount { get; set; }

        public int RowVersion { get; set; }

        public bool CanDeliver { get; set; }

        public string DeliverEmployeeName { get; set; }

        public string DeliverDate { get; set; }

        public long RollbackPrice { get; set; }

        public string RollbackEmployeeName { get; set; }

        public string RollbackDate { get; set; }
    }
}

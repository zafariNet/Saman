using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Controllers.ViewModels.SaleCatalog
{
    public class ClientSaleDetailViewModel: Production
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

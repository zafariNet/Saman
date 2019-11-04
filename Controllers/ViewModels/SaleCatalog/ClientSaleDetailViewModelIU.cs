using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Controllers.ViewModels.SaleCatalog
{
    public class ClientSaleDetailViewModelIU : Production
    {

        public Guid RowID { get; set; }

        [DisplayName("تعداد")]
        [UIHint("Integer")]
        public int Units { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Discount { get; set; }


        public int RowVersion { get; set; }

    }
}

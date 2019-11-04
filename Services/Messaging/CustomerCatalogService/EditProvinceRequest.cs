using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    /// <summary>
    /// ایجاد شده توسط محمد ظفری
    /// </summary>
    public class EditProvinceRequest:AddProvinceRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
        public Guid ModifiedEmployeeID { get; set; }
    }
}

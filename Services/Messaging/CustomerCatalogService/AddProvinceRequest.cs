using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    /// <summary>
    /// ایجاد شده توسط محمد ظفری
    /// </summary>
    public class AddProvinceRequest
    {
        public string ProvinceName { get; set; }
        public Guid EmployeeId { get; set; }
    }
}

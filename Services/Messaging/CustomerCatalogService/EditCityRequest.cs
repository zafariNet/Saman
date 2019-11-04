using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    /// <summary>
    /// اضافه شده توسط محمد ظفری
    /// </summary>
    public class EditCityRequest:AddCityRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
        public Guid MdifiedEmployeeID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    /// <summary>
    /// ایجاد شده توسط محمد ظفری
    /// </summary>
    public class AddCityRequest
    {
        public string CityName { get; set; }
        public Guid ProvinceID { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}

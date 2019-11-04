using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
namespace Services.Messaging.CustomerCatalogService
{
    /// <summary>
    /// اضافه شده توسط محمد ظفری
    /// </summary>
    class GetCitiesResponse
    {
        public IEnumerable<CityView> CityViews { get; set; }
    }
}

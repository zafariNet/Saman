using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class ProductLogMapper
    {
        public static IEnumerable<ProductLogView> ConvertToProductLogViews(
            this IEnumerable<ProductLog> productLogs)
        {
            return Mapper.Map<IEnumerable<ProductLog>,
                IEnumerable<ProductLogView>>(productLogs);
        }

        public static ProductLogView ConvertToProductLogView(this ProductLog productLog)
        {
            return Mapper.Map<ProductLog, ProductLogView>(productLog);
        }
    }
}

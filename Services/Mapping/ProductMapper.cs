using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class ProductMapper
    {
        public static IEnumerable<ProductView> ConvertToProductViews(
            this IEnumerable<Product> products)
        {
            return Mapper.Map<IEnumerable<Product>,
                IEnumerable<ProductView>>(products);
        }

        public static ProductView ConvertToProductView(this Product product)
        {
            return Mapper.Map<Product, ProductView>(product);
        }
    }
}

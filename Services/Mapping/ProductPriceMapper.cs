using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class ProductPriceMapper
    {
        public static IEnumerable<ProductPriceView> ConvertToProductPriceViews(
            this IEnumerable<ProductPrice> productPrices)
        {
            return Mapper.Map<IEnumerable<ProductPrice>,
                IEnumerable<ProductPriceView>>(productPrices);
        }

        public static ProductPriceView ConvertToProductPriceView(this ProductPrice productPrice)
        {
            return Mapper.Map<ProductPrice, ProductPriceView>(productPrice);
        }
    }
}

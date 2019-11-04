using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class ProductCategoryMapper
    {
        public static IEnumerable<ProductCategoryView> ConvertToProductCategoryViews(
            this IEnumerable<ProductCategory> productCategorys)
        {
            return Mapper.Map<IEnumerable<ProductCategory>,
                IEnumerable<ProductCategoryView>>(productCategorys);
        }

        public static ProductCategoryView ConvertToProductCategoryView(this ProductCategory productCategory)
        {
            return Mapper.Map<ProductCategory, ProductCategoryView>(productCategory);
        }
    }
}

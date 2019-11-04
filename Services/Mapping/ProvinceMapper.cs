using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class ProvinceMapper
    {
        public static IEnumerable<ProvinceView> ConvertToProvinceViews(this IEnumerable<Province> provinces)
        {
            return Mapper.Map<IEnumerable<Province>, IEnumerable<ProvinceView>>(provinces);
        }
        public static ProvinceView ConvertToProvinceView(this Province province)
        {
            return Mapper.Map<Province, ProvinceView>(province);
        }
    }
}

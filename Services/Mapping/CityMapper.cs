using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class CityMapper
    {
        public static IEnumerable<CityView> ConvertToCityViews(this IEnumerable<City> cities)
        {
            return Mapper.Map<IEnumerable<City>, IEnumerable<CityView>>(cities);
        }
        public static CityView ConvertToCityView(this City city)
        {
            return Mapper.Map<City, CityView>(city);
        }
    }
}

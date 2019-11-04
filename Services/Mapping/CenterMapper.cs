using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{   
    public static class CenterMapper
    {
        public static IEnumerable<CenterView> ConvertToCenterViews(
            this IEnumerable<Center> centers)
        {
            List<CenterView> returnCenters = new List<CenterView>();

            foreach (Center employee in centers)
            {
                returnCenters.Add(employee.ConvertToCenterView());
            }

            return returnCenters;
        }

        public static CenterView ConvertToCenterView(this Center center)
        {
            return Mapper.Map<Center, CenterView>(center);
        }
    }
}

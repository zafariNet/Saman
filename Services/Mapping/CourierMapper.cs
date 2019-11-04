using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Sales;
using Services.ViewModels.Sales;

namespace Services.Mapping
{
    public static class CourierMapper
    {
        public static IEnumerable<CourierView> ConvertToCourierViews(
            this IEnumerable<Courier> courier)
        {
            return Mapper.Map<IEnumerable<Courier>,
                IEnumerable<CourierView>>(courier);
        }

        public static CourierView ConvertToCourierView(this Courier courier)
        {
            return Mapper.Map<Courier, CourierView>(courier);
        }
    }
}

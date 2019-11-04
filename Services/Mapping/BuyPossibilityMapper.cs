using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class BuyPossibilityMapper
    {
        public static IEnumerable<BuyPossibilityView> ConvertToBuyPossibilityViews(
            this IEnumerable<BuyPossibility> buyPossibilitys)
        {
            return Mapper.Map<IEnumerable<BuyPossibility>,
                IEnumerable<BuyPossibilityView>>(buyPossibilitys);
        }

        public static BuyPossibilityView ConvertToBuyPossibilityView(this BuyPossibility buyPossibility)
        {
            return Mapper.Map<BuyPossibility, BuyPossibilityView>(buyPossibility);
        }
    }
}

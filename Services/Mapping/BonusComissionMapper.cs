using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;

namespace Services.Mapping
{
    public static class BonusComissionMapper
    {
        public static IEnumerable<BonusComissionView> ConvertToBonusComissionViews(
    this IEnumerable<BonusComission> bonusComissions)
        {
            return Mapper.Map<IEnumerable<BonusComission>,
                IEnumerable<BonusComissionView>>(bonusComissions);
        }

        public static BonusComissionView ConvertToBonusComissionView(this BonusComission bonusComission)
        {
            return Mapper.Map<BonusComission, BonusComissionView>(bonusComission);
        }
    }
}

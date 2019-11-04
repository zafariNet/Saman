using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class ConditionMapper
    {
        public static IEnumerable<ConditionView> ConvertToConditionViews(
            this IEnumerable<Condition> conditions)
        {
            return Mapper.Map<IEnumerable<Condition>,
                IEnumerable<ConditionView>>(conditions);
        }

        public static ConditionView ConvertToConditionView(this Condition condition)
        {
            return Mapper.Map<Condition, ConditionView>(condition);
        }
    }
}

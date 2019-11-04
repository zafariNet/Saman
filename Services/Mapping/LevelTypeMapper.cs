using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class LevelTypeMapper
    {
        public static IEnumerable<LevelTypeView> ConvertToLevelTypeViews(
            this IEnumerable<LevelType> levelTypes)
        {
            return Mapper.Map<IEnumerable<LevelType>,
                IEnumerable<LevelTypeView>>(levelTypes);
        }

        public static LevelTypeView ConvertToLevelTypeView(this LevelType levelType)
        {
            return Mapper.Map<LevelType, LevelTypeView>(levelType);
        }
    }
}

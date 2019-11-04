using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class LevelMapper
    {
        public static IEnumerable<LevelView> ConvertToLevelViews(
            this IEnumerable<Level> levels)
        {
            List<LevelView> returnLevelViews = new List<LevelView>();

            foreach (Level level in levels)
            {
                returnLevelViews.Add(level.ConvertToLevelView());
            }

            return returnLevelViews;
        }

        public static LevelView ConvertToLevelView(this Level level)
        {
            return Mapper.Map<Level, LevelView>(level);
        }
    }
}

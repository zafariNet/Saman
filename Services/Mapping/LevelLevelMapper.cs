using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class LevelLevelMapper
    {
        public static IEnumerable<LevelLevelView> ConvertToLevelLevelViews(
            this IEnumerable<LevelLevel> levelLevels)
        {
            List<LevelLevelView> returnLevelLevels = new List<LevelLevelView>();

            foreach (LevelLevel levelLevel in levelLevels)
            {
                returnLevelLevels.Add(levelLevel.ConvertToLevelLevelView());
            }

            return returnLevelLevels;
        }

        public static LevelLevelView ConvertToLevelLevelView(this LevelLevel levelLevel)
        {
            return Mapper.Map<LevelLevel, LevelLevelView>(levelLevel);
        }
    }
}

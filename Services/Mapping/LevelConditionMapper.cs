using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class LevelConditionMapper
    {
        public static IEnumerable<LevelConditionView> ConvertToLevelConditionViews(
            this IEnumerable<LevelCondition> levelConditions)
        {
            List<LevelConditionView> returnLevelConditions = new List<LevelConditionView>();

            foreach (LevelCondition levelCondition in levelConditions)
            {
                returnLevelConditions.Add(levelCondition.ConvertToLevelConditionView());
            }

            return returnLevelConditions;
        }

        public static LevelConditionView ConvertToLevelConditionView(this LevelCondition levelCondition)
        {
            return Mapper.Map<LevelCondition, LevelConditionView>(levelCondition);
        }
    }
}

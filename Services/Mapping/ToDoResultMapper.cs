using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class ToDoResultMapper
    {
        public static IEnumerable<ToDoResultView> ConvertToToDoResultViews(
    this IEnumerable<ToDoResult> ToDoResults)
        {
            List<ToDoResultView> returnToDoResults = new List<ToDoResultView>();

            foreach (ToDoResult toDoResult in ToDoResults)
            {
                returnToDoResults.Add(toDoResult.ConvertToToDoResultView());
            }

            return returnToDoResults;
        }

        public static ToDoResultView ConvertToToDoResultView(this ToDoResult toDoResult)
        {
            return Mapper.Map<ToDoResult, ToDoResultView>(toDoResult);
        }
    }
}

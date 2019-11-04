using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;

namespace Services.Mapping
{
    public static class TaskMapper
    {
        public static IEnumerable<TaskOwnView> ConvertToTaskOwnViews(
            this IEnumerable<Task> tasks)
        {
            return Mapper.Map<IEnumerable<Task>,
                IEnumerable<TaskOwnView>>(tasks);
        }

        public static TaskOwnView ConvertToTaskOwnView(this Task task)
        {
            return Mapper.Map<Task, TaskOwnView>(task);
        }
    }
}

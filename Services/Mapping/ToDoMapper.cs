using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class ToDoMapper
    {
        public static IEnumerable<ToDoView> ConvertToToDoViews(
            this IEnumerable<ToDo> ToDos)
        {
            List<ToDoView> returnToDos = new List<ToDoView>();

            foreach (ToDo toDo in ToDos)
            {
                returnToDos.Add(toDo.ConvertToToDoView());
            }

            return returnToDos;
        }

        public static ToDoView ConvertToToDoView(this ToDo toDo)
        {
            return Mapper.Map<ToDo, ToDoView>(toDo);
        }
    }
}

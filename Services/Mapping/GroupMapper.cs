using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Services.ViewModels.Employees;
using AutoMapper;

namespace Services.Mapping
{
    public static class GroupMapper
    {
        public static IEnumerable<GroupView> ConvertToGroupViews(
            this IEnumerable<Group> groups)
        {
            List<GroupView> returnGroups = new List<GroupView>();

            foreach (Group group in groups)
            {
                returnGroups.Add(group.ConvertToGroupView());
            }

            return returnGroups;

        }

        public static GroupView ConvertToGroupView(this Group group)
        {
            return Mapper.Map<Group, GroupView>(group);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Services.ViewModels.Employees;
using AutoMapper;

namespace Services.Mapping
{
    public static class PermissionMapper
    {
        public static IEnumerable<PermissionView> ConvertToPermissionViews(
            this IEnumerable<Permission> permissions)
        {
            return Mapper.Map<IEnumerable<Permission>,
                IEnumerable<PermissionView>>(permissions);
        }

        public static PermissionView ConvertToPermissionView(this Permission permission)
        {
            return Mapper.Map<Permission, PermissionView>(permission);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;

namespace Services.Messaging.EmployeeCatalogService
{
    public class GetPermissionsResponse
    {
        public IEnumerable<PermissionView> PermissionViews { get; set; }
    }
}

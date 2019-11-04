using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.EmployeeCatalogService;
using Services.Messaging;
using Services.ViewModels.Employees;

namespace Services.Interfaces
{
    public interface IPermissionService
    {
        GeneralResponse AddPermission(AddPermissionRequest request);
        GeneralResponse EditPermission(EditPermissionRequest request);
        GeneralResponse DeletePermission(DeleteRequest request);
        GetPermissionResponse GetPermission(GetRequest request);
        GetPermissionsResponse GetPermissions();
        GetPermissionsResponse GetPermissions(GetPermissionsRequest request);
        //Edited By Zafari
        //Orginal GetPermissionsResponse GetPermissions(GetPermissionsRequestG request);
        GetPermissionsResponse GetPermissions(Guid GroupID);
        // By Hojjat
        GetGeneralResponse<IEnumerable<PermissionView>> GetPermissions(Guid GroupID, int pageSize, int pageNumber);

        //GetGeneralResponse<IEnumerable<PermissionView>> GetPermissions(Guid GroupID, int pageSize, int pageNumber);
    }
}

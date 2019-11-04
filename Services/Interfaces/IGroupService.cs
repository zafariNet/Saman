using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.EmployeeCatalogService;
using Services.Messaging;
using Services.ViewModels.Employees;

namespace Services.Interfaces
{
    public interface IGroupService
    {
        GeneralResponse AddGroup(AddGroupRequest request);
        GeneralResponse EditGroup(EditGroupRequest request,Guid ModifiedEmployeeID);
        //GeneralResponse EditGroup(EditPermissionRequestG request);
        GeneralResponse EditGroup(IEnumerable<EditPermissionRequestG> requests, Guid GroupID, Guid ModifiedEmployeeID);
        GeneralResponse DeleteGroup(DeleteRequest request);
        GetGroupResponse GetGroup(GetRequest request);
        GetGeneralResponse<IEnumerable<GroupView>> GetGroups(int pageSize, int pageNumber);
        GeneralResponse EditGroupPermissions(EditGroupPermissionsRequest request, Guid ModifiedEmployeeID);
        GetGeneralResponse<IEnumerable<GroupView>> GetChildGroups(Guid GroupID);
        GeneralResponse ChangeGroupParent(Guid GroupID, Guid ParentID,Guid ModifiedEmployeeID);
        GeneralResponse AddNewPermissionsToGroupAndUsers(Guid PermissionID);
        GetGeneralResponse<IEnumerable<SimpleGroupView>> GetSimpleGroup();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.EmployeeCatalogService;
using Services.Messaging;
using Model.Employees;
using Services.ViewModels.Employees;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IEmployeeService
    {
        GetGeneralResponse<IEnumerable<EmployeeView>> GetAllChilOfAnEmployee(Guid EmployeeID);
        GeneralResponse AddEmployee(AddEmployeeRequest request , Guid CreateEmployeeID);
        GeneralResponse EditEmployee(EditEmployeeRequest request , Guid ModifiedEmployeeID);
        GeneralResponse EditEmployee(EditPermissionRequest request);
        //Added By Zafari
        GeneralResponse EditEmployees(IEnumerable<EditEmployeeRequest> request, Guid ModifiedEmployeeID);
        //Added By Zafari
        GeneralResponse DeleteEmployees(IEnumerable<DeleteRequest> requests);
        GeneralResponse DeleteEmployee(DeleteRequest request);
        GetEmployeeResponse GetEmployee(GetRequest request);
        GetEmployeesResponse GetEmployees();
        GetGeneralResponse<IEnumerable<SimpleEmployeeView>> _GetEmployees();

        GetGeneralResponse<IEnumerable<EmployeeView>> GetEmployeesInstaller();
        //Added By Zafari
        GetGeneralResponse<IEnumerable<EmployeeView>> GetEmployees(Guid? groupID, int pageSize, int pageNumber,List<FilterData> filter,List<Sort> sort);
        //Added By Zafari
        GetGeneralResponse<IEnumerable<EmployeeView>> GetEmployeesByGroup(Guid GroupID);
        GetGeneralResponse<IEnumerable<EmployeeView>> GetEmployeesByGroup(Guid GroupID,int pageSize,int pageNumber);
        GeneralResponse EditEmployeePermission(IEnumerable<EditPermissionRequestG> requests, Guid EmployeeID, Guid ModifiedEmployeeID);
        GetEmployeesResponse GetInstallExprets();
        string Authenticate(string userName, string password, bool persistentLogin);

        GeneralResponse RecreateSaPermissions();

        GeneralResponse ChangeGroup(IEnumerable<Guid> EmployeeIDs, Guid GroupID);
        ///Added By Zafari
        GeneralResponse EditSimpleEmployees(IEnumerable<EditSimpleEmployeeRequest> requests);

        GeneralResponse RecreateGroupPermissions(Guid GroupID);

        GetEmployeeResponse GetSimpleEmployee(GetRequest request);

        GeneralResponse ChangePassword(string CurrentPassword, string NewPassword, string ConfirmPassword, Guid EmployeeID);

        GeneralResponse InsertPicture(Guid EmployeeID, string Path);
    }
}

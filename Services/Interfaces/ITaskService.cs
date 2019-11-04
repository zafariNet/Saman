using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Services.Interfaces
{
    public interface ITaskService
    {
        GeneralResponse AddTask(AddTaskRequest request, Guid CreateEmployeeID);
        GetGeneralResponse<IEnumerable<TaskOwnView>> GetEmployeeTasks(Guid EmployeeID, int pageSize, int pageNumber, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<TaskOwnView>> GetTasks(Guid employeeId, string TaskType,
           string Status, string StartDate, string EndDate, Guid CurrentEmployeeId);
        GeneralResponse CloseSecondary(Guid EmployeeID, Guid TaskID, string SecondaryDescription);
        GeneralResponse ClosePrimary(Guid EmployeeID, Guid TaskID);

        bool CanAttachSecondary(Guid EmployeeID, Guid TaskID);
        bool CanAttachPrimary(Guid EmployeeID, Guid TaskID);
        GeneralResponse DeleteAttachment(Guid TaskID);
        GeneralResponse AttachFileToSecondary(Guid TaskID, Guid EmployeeID,string path, string fileName);
        GeneralResponse AttachFileToPrimary(Guid TaskID, Guid EmployeeID,string path ,string fileName);

        GetGeneralResponse<IEnumerable<TaskOwnView>> GetTasks(IEnumerable<Guid?> CreateEmployeeIDs, IEnumerable<Guid?> ReferedEmployeeIDs,
            int? Status, int? Type,  IList<Sort> sort, Guid EmployeeID, string StartDate, string EndDate,Guid CurrentEmployeeId);

        GetGeneralResponse<IEnumerable<TaskOwnView>> GetFirstPageTasks(string taskType, Guid employeeId, int pageSize, int pageNumber);
    }
}

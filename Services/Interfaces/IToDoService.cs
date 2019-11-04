using Infrastructure.Querying;
using Model.Employees;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Interfaces
{
    public interface IToDoService
    {
        GetGeneralResponse<IEnumerable<ToDoView>> GetAllToDos(int pageSize, int pageNumber);
        GetGeneralResponse<IEnumerable<ToDoView>> GetCreatorEmployeeToDos(Guid CreateEmployeeID, bool? PrimaryClosed, bool? SecondaryClosed, int pageSize, int pageNumber);
        GetGeneralResponse<IEnumerable<ToDoView>> GetReferedEmployeeToDos(Guid CreateEmployeeID, Guid ReferedEmployeeID, int? Close,int? TaskStatusID,
            string StartDateRange, string EndDateRange, Guid CustomerID, int pageSize, int pageNumber, IList<Sort> sort);
        GeneralResponse SecondaryClose(Guid ToDoResultID, string ToDoResultDescription, Guid ModifiedEmployeeID);
        GeneralResponse PrimaryClose(Guid ToDoID, Guid ModifiedEmployeeID);
        GeneralResponse AddTodo(AddToDoRequest request, Guid CreateEmployeeID);
        GeneralResponse EditToDoResult(EditToDoResultRequest request, Guid ModifiedEmployeeID);
        GeneralResponse AddToDoAttachment(AddToDoAttachmentRequest request, Guid CreateEmployeeID);
        GeneralResponse AddToDoResultAttachment(AddToDoResultAttachmentRequest request, Guid CreateEmployeeID);
        GeneralResponse DeleteToDoAttachment(Guid ToDoID,Guid ModifiedEmployeeID);
        GeneralResponse DeleteToDoResultAttachment(Guid ToDoID, Guid ModifiedEmployeeID);
        GetGeneralResponse<IEnumerable<ToDoResultView>> GetToDoResults(Guid EmployeeID,Guid ToDoID);
        ToDoResultView GetToDoResult(Guid ToDoResultID);
        GetGeneralResponse<ToDoView> GetToDo(Guid ToDoID);
        GetGeneralResponse<IEnumerable<SimpleEmployeeView>> GetAllChildren(Guid managerID);
        GetGeneralResponse<IEnumerable<ToDoResultView>> GetToDoResults(IEnumerable<Guid> ToDoResultsID);
        bool isFileExist(string path);
    }
}

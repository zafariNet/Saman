using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.FiscalCatalogService;
using Services.Messaging;
using Services.ViewModels.Fiscals;

namespace Services.Interfaces
{
    public interface IMoneyAccountEmployeeService
    {
        GeneralResponse AddMoneyAccountEmployee(AddMoneyAccountEmployeeRequestOld request);
        GeneralResponse EditMoneyAccountEmployee(EditMoneyAccountEmployeeRequest request);
        GeneralResponse DeleteMoneyAccountEmployee(DeleteRequest request);
        GetMoneyAccountEmployeeResponse GetMoneyAccountEmployee(GetRequest request);
        GetMoneyAccountEmployeesResponse GetMoneyAccountEmployees();
        GetMoneyAccountEmployeesResponse GetMoneyAccountEmployees(Guid moneyAccountID);

        GeneralResponse DeleteMoneyAccountEmployee(Guid moneyAccountID, Guid employeeID);


        GetGeneralResponse<IEnumerable<MoneyAccountEmployeeView>> GetMoneyAccountEmployees(Guid MoneyAccountID, int pageSize, int pageNumber);
        GeneralResponse AddMoneyAccountEmployee(AddMoneyAccountEmployeeRequest request, Guid CreateEmployeeID);
        GeneralResponse DeleteMoneyAccountEmployee(IEnumerable<DeleteMoneyAccountEmployeeRequest> requests, Guid MoneyAccountID);
    }
}

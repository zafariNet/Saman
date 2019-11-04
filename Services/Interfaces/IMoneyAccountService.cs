using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.FiscalCatalogService;
using Services.Messaging;
using Services.ViewModels.Fiscals;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IMoneyAccountService
    {
        GeneralResponse AddMoneyAccount(AddMoneyAccountRequestOld request);
        GeneralResponse EditMoneyAccount(EditMoneyAccountRequestOld request);
        GeneralResponse DeleteMoneyAccount(DeleteRequest request);
        GetMoneyAccountResponse GetMoneyAccount(GetRequest request);
        GetMoneyAccountsResponse GetMoneyAccounts();
        GetMoneyAccountsResponse GetBankAccounts();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GetGeneralResponse<IEnumerable<MoneyAccountView>> GetMoneyAccounts(bool isReceive, int PageSize, int PageNumber);

        GeneralResponse AddMoneyAccounts(IEnumerable<AddMoneyAccountRequest> requests, Guid CreateEmployeeID);
        GeneralResponse EditMoneyAccounts(IEnumerable<EditMoneyAccountRequest> requests, Guid ModifiedEmployeeID);
        GeneralResponse DeleteMoneyAccounts(IEnumerable<DeleteRequest> requests);
         GetGeneralResponse<IEnumerable<MoneyAccountView>> GetAllMoneyAccounts(int pageSize, int pageNumber,IList<Sort> sort);
         
    }
}

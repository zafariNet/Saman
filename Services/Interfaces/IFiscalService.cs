using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.FiscalCatalogService;
using Services.Messaging;
using Services.ViewModels.Fiscals;
using Model.Fiscals;
using Infrastructure.Querying;
using Services.ViewModels.Reports;

namespace Services.Interfaces
{
    public interface IFiscalService
    {
        GeneralResponse AddFiscal(AddFiscalRequest request);
        GeneralResponse EditFiscal(EditFiscalRequest request);
        GeneralResponse DeleteFiscal(DeleteRequest request);
        GetFiscalResponse GetFiscal(GetRequest request);
        GetFiscalsResponse GetFiscals(AjaxGetRequest getRequest, Guid employeeID);
        GetFiscalsResponse GetFiscalsCreatedOrConfirmedWithMe(AjaxGetRequest getRequest, string employeeID);
        GetFiscalsResponse GetFiscals(AjaxGetRequest getRequest);
        GetFiscalsResponse GetFiscalsCanConfirm(AjaxGetRequest getRequest, string employeeID);
        GetFiscalsResponse GetFiscalsOfCustomer(AjaxGetRequest getRequest, Guid customerID);
        GeneralResponse Confirm(ConfirmRequest request);

        GetGeneralResponse<IEnumerable<FiscalView>> GetFiscals(Guid customerID, int pageSize, int pageNumber,IList<Sort> sort,IList<FilterData> filter);
        GetGeneralResponse<IEnumerable<FiscalView>> GetAllFiscals(int pageSize, int pageNumber, IList<Sort> sort,IList<FilterData> filter);
        GetGeneralResponse<FiscalView> GetFiscalByID(Guid FiscallID);
        GeneralResponse ChangeCharedStatus(ChargeStatus chargStatus, Guid fiscalID,Guid ModifiedEmployeeID);
        GetGeneralResponse<FiscalView> GetFollowUpNumber(int FollowNumber);
        GetGeneralResponse<IEnumerable<GetBankCasheReportView>> GetBankCasheReport(int? TransactionType, IEnumerable<Guid> MoneyAccountID,
            string InvestStartDate, string InvestEndDate, string ConfirmStartDate, string ConfirmEndDate, IList<Sort> sort, bool NotConfirmed);
    }
}

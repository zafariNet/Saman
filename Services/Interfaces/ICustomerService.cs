using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.Messaging.ReportCatalogService;
using Services.ViewModels.Customers;
using Infrastructure.Querying;
using Services.ViewModels.Reports;

namespace Services.Interfaces
{
    public interface ICustomerService
    {
        GeneralResponse AddCustomer(AddCustomerRequest request);
        GeneralResponse EditCustomer(EditCustomerRequest request);
        GeneralResponse DeleteCustomer(DeleteRequest request);
        GetCustomerResponse GetCustomer(GetRequest request);
        GetGeneralResponse<IEnumerable<CustomerView>> GetCustomers();

        GetGeneralResponse<IEnumerable<CustomerView>> GetCustomers(QuickSearchRequest getRequest);

        CustomerView getCustomerbyPhone(string ADSLPhone);

        CustomerView GetCustomerByID(Guid CustomerID);

        GetGeneralResponse<IEnumerable<CustomerView>> FindCustomers(QuickSearchRequest request, Guid currentEmployeeID);
        GetGeneralResponse<IEnumerable<CustomerView>> FindCustomers(AdvancedSearchRequest request);

        GetGeneralResponse<IEnumerable<CustomerView>> GetCustomers(Guid queryID, Guid currentEmployeeID, int pageSize, int pageNumber,IList<Sort> sort,IList<FilterData> filter);

        GeneralResponse QuickAddCustomer(AddCustomerRequest request);

        GetGeneralResponse<IEnumerable<CustomerView>> test(IList<FilterData> filter);
        GetGeneralResponse<IEnumerable<CustomerView>> SendSMS(IEnumerable<Guid> IDs, string Message,Guid CreateEmployeeID);
        GetGeneralResponse<IEnumerable<CustomerView>> SendEmail(IEnumerable<Guid> IDs, string Message,string Subject, Guid CreateEmployeeID);
        GetGeneralResponse<LevelOptionsView> GetLevelOptions(Guid customerID);
        string LazyTest(string hqlQuery);
        GetGeneralResponse<IEnumerable<CustomerView>> GetAllCustomrs();

        GeneralResponse SendEmalAndSms(IEnumerable<Guid> IDs, string subject, string Content, string Message,
            Guid CreateEmployeeID);

        GetGeneralResponse<SimpleCustomerView> GetSimpleCustomer(string ADSLPhone);

        GetGeneralResponse<IEnumerable<CustomerView>> GetCustomerMustoGoToRanje(int pageSize, int pageNumber,IList<FilterData> filter,IList<Sort> sort);

        GetGeneralResponse<IEnumerable<SuctionModeReportView>> GetSuctionModeReport(GetSuctionModeRequest request);
        GetGeneralResponse<IEnumerable<GetNetworkReportView>> GetNetworkReport(GetSuctionModeRequest request);
        GetGeneralResponse<IEnumerable<CenterReportView>> GetCenterReport(GetSuctionModeRequest request);
        GetGeneralResponse<IEnumerable<GetCustomerCampaignView>> GetCustomerForCampaign(IList<FilterData> filter);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using Model.Sales;
using Services.Messaging;
using Services.Messaging.ReportCatalogService;
using Services.Messaging.SaleCatalogService;
using Services.ViewModels.Sales;
using Services.ViewModels.Reports;

namespace Services.Interfaces
{
    public interface ICampaignPaymentService
    {

        GetGeneralResponse<IEnumerable<CampaignPaymentView>> GetCampaignPayments(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<CampaignPaymentView>> GetCampaignPaymentsByAgent(int pageSize,int pageNumber,Guid CampaignAgentID);

        GeneralResponse AddCampaignPayment(IEnumerable<AddCampignPaymentRequest> requests, Guid CampaignAgentID, Guid CreateEmployeeID);
        GeneralResponse EditCampaignPayment(IEnumerable<EditCampignPaymentRequest> requests, Guid CampaignAgentID, Guid ModifiedEmployeeID);
        GeneralResponse DeleteCampaignPayment(IEnumerable<DeleteRequest> requests);

        GetGeneralResponse<IEnumerable<GetSuctionModeCost1View>> GetSuctionModeCostReport1(IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<GetSuctionModeCost2View>> GetSuctionModeCostReport2(IList<FilterData> filter,
            IList<Sort> sort);

        GetGeneralResponse<IEnumerable<GetcampaignAgents>> GetSuctionModeCostReport3(IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<SuctionModeCost>> GetSuctionModeReport3(
            GetsuctionModeRequestForReport3 request);
        GetGeneralResponse<IEnumerable<SuctionModeCost>> GetSuctionModeReport4(
            GetsuctionModeRequestForReport3 request);

    }
}

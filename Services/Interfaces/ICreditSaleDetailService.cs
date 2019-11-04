using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.SaleCatalogService;
using Services.Messaging;
using Model.Sales;
using Services.ViewModels.Sales;
using Services.ViewModels.Reports;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface ICreditSaleDetailService
    {
        GeneralResponse EditCreditSaleDetail(EditCreditSaleDetailRequest request);
        GeneralResponse DeleteCreditSaleDetail(DeleteRequest request);
        GetCreditSaleDetailResponse GetCreditSaleDetail(GetRequest request);
        GetCreditSaleDetailsResponse GetCreditSaleDetails();
        IEnumerable<CreditSaleDetail> PrepareCreditSaleDetails(IEnumerable<AddCreditSaleDetailRequest> requests);
        //GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(SaleReportRequest request);
        GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(IList<FilterData> flters);
        
    }
}

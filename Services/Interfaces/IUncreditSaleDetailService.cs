using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.SaleCatalogService;
using Services.Messaging;
using Services.ViewModels.Sales;
using Services.ViewModels.Reports;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IUncreditSaleDetailService
    {
        GeneralResponse AddUncreditSaleDetail(AddSaleDetailBaseRequest request);
        GeneralResponse EditUncreditSaleDetail(EditUncreditSaleDetailRequest request);
        GeneralResponse DeleteUncreditSaleDetail(DeleteRequest request);
        GetUncreditSaleDetailResponse GetUncreditSaleDetail(GetRequest request);
        GetUncreditSaleDetailsResponse GetUncreditSaleDetails();
        //GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(SaleReportRequest request);
        GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(IList<FilterData> request);
    }
}

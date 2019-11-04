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
    public interface IProductSaleDetailService
    {
        GeneralResponse AddProductSaleDetail(AddProductSaleDetailRequest request);
        GeneralResponse EditProductSaleDetail(EditProductSaleDetailRequest request);
        GeneralResponse DeleteProductSaleDetail(DeleteRequest request);
        GetProductSaleDetailResponse GetProductSaleDetail(GetRequest request);
        GetProductSaleDetailsResponse GetProductSaleDetails();
        GetGeneralResponse<IEnumerable<ProductSaleDetailView>> GetProductSaleDetails_ByProductID(Guid productID);
        GetGeneralResponse<IEnumerable<ProductSaleDetailView>> GetProductSaleDetails(IList<FilterData> filters);
        GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(SaleReportRequest request);
        GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(IList<FilterData> filters);
    }
}

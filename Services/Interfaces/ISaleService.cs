using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.SaleCatalogService;
using Services.Messaging;
using Services.ViewModels.Reports;
using Services.ViewModels.Sales;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface ISaleService
    {
        GeneralResponse AddSale(AddSaleRequest request);

        GeneralResponse EditSale(EditSaleRequest request);

        GeneralResponse CloseSale(CloseSaleRequest request);

        GeneralResponse DeleteSale(DeleteRequest request);

        GetSaleResponse GetSale(GetRequest request);

        GetSalesResponse GetSales();

        GetSalesResponse GetSales(AjaxGetRequest request, IList<Sort> sort, IList<FilterData> filter, bool ForReport);

        GeneralResponse DeleteProductSaleDetail(DeleteRequest request);

        GeneralResponse DeleteCreditSaleDetail(DeleteRequest request);

        GeneralResponse DeleteUncreditSaleDetail(DeleteRequest request);

        GeneralResponse DeleteSaleDetail(DeleteSaleDetailRequest request);

        GeneralResponse SaleDetail_Deliver(DeliverRequest request);

        GetGeneralResponse<SaleView> GetSale(Guid saleID);


            GetGeneralResponse<IEnumerable<SaleView>> GetUnClosedSales(int pageSize, int pageNumber);

        GetGeneralResponse<IEnumerable<ProductSaleDetailView>> GetUnDeliveredProducts(Guid  CustomerID);

        GetSalesResponse SimpleGetSales(AjaxGetRequest request, IList<Sort> sort, IList<FilterData> filter,
            bool ForReport);
        GeneralResponse Bonus();

        GetGeneralResponse<IEnumerable<CanDeliverCostView>> GetSaleCanDeliverCost(string StartDate, string EndDate);
        GetGeneralResponse<IEnumerable<CanDeliverCostView>> GetSaleBalance(string StartDate, string EndDate);

    }
}

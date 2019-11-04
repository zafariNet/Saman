using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Customers.Interfaces;

namespace Model.Sales.Interfaces
{
    public interface ICampaignPaymentRepository:IRepository<CampaignPayment>
    {
        IList<SuctionModeCost> Report(string RegisterStartdate, string RegisterEndDate, string PaymentStartDate, string PaymentEndDate, int WhichReport, bool IsRanje, string InputSupportStartDate, string InputSupportEndDate, bool HasFiscal, IEnumerable<Guid?> SuctionModeDetailsIDs, IEnumerable<Guid> SuctionModeIDs,IEnumerable<Guid?> CenterIDs);
    }
}

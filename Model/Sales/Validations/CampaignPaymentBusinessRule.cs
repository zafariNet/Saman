using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Sales.Validations
{
    public class CampaignPaymentBusinessRule
    {
        public static readonly BusinessRule SuctionModeDetailRequired = new BusinessRule("SuctionModeDetail","لطفا روش جذب را انتخاب نمایید");

        public static readonly BusinessRule CampaignAgentRequired = new BusinessRule("CampaignAgent",
            "لطفا عامل تبلیغاتی را انتخاب نمایید");
        public static readonly BusinessRule PaymentDateRequired = new BusinessRule("PaymentDate","تاریخ واریز باید به درستی وارد شود");

        public static readonly BusinessRule AmountRequired = new BusinessRule("Amount","مبلغ وارد شده معتبر نمیباشد");
    }
}

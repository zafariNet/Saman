using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Sales
{
    public class CampaignPaymentView:BaseView
    {

        /// <summary>
        /// نام شیوه جذب
        /// </summary>
        public string SuctionModeDetailName { get; set; }
        /// <summary>
        /// نام شیوه جذب
        /// </summary>
        public string SuctionModeName { get; set; }

        /// <summary>
        /// نام شیوه جذب
        /// </summary>
        public string SuctionModeDetailID { get; set; }
        /// <summary>
        /// نام شیوه جذب
        /// </summary>
        public string SuctionModeID { get; set; }

        /// <summary>
        /// نام عامل تبلیغاتی
        /// </summary>
        public string CampaignAgentName { get; set; }

        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// مبلغ پرداخت
        /// </summary>
        public string Amount { get; set; }
    }
}

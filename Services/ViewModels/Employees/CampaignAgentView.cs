using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;

namespace Services.ViewModels.Employees
{
    public class CampaignAgentView:BaseView
    {


        /// <summary>
        /// نام نماینده
        /// </summary>
        public  string CampaignAgentName { get; set; }

        /// <summary>
        /// مجموع پرداختی به عامل
        /// </summary>
        public  long TotalPayment { get; set; }



    }
}

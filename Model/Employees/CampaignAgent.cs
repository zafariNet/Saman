using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees.Validations;
using Model.Sales;

namespace Model.Employees
{
    public class CampaignAgent:EntityBase,IAggregateRoot
    {

        /// <summary>
        /// نام نماینده
        /// </summary>
        public virtual string CampaignAgentName { get; set; }

        /// <summary>
        /// مجموع پرداختی به عامل
        /// </summary>
        public virtual long TotalPayment { get; set; }

        public virtual IEnumerable<CampaignPayment> CampaignPayments { get; set; }

        protected override void Validate()
        {
            if(string.IsNullOrEmpty(this.CampaignAgentName))
                AddBrokenRule(CampaignAgentBusinessRule.CampaignAgentNameRequired);
        }
    }
}

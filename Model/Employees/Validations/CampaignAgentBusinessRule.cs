using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Employees.Validations
{
    public class CampaignAgentBusinessRule
    {
        public static readonly BusinessRule CampaignAgentNameRequired = new BusinessRule("CampaignAgent","نام عامل باید وارد شود");
    }
}

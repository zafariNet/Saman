using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
using Model.Base.Validations;

namespace Infrastructure.Domain
{
    /// <summary>
    /// base class for value objects.
    /// Value Object is a data without Id.
    /// </summary>
    public abstract class ValueObjectBase
    {
        List<BusinessRule> _brokenRules = new List<BusinessRule>();

        public ValueObjectBase()
        {

        }

        protected abstract void Validate();

        public void ThrowExeptionIfInvalid()
        {
            _brokenRules.Clear();
            Validate();
            if (_brokenRules.Count() > 0)
            {
                StringBuilder issues = new StringBuilder();
                foreach (BusinessRule br in _brokenRules)
                {
                    issues.AppendLine(br.Rule);
                }
                throw new ValueObjectIsInvalidException(issues.ToString());
            }
        }

        protected void AddBrokenRule(BusinessRule brokenRule)
        {
            _brokenRules.Add(brokenRule);
        }
    }
}

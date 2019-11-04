using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Infrastructure.Querying
{
    public class Criterion
    {
        string _propertyName;
        object _value;
        CriteriaOperator _criteriaOperator;

        public Criterion()
        {

        }
        public Criterion(string propertyName, object value, CriteriaOperator criteriaOperator)
        {
            _propertyName = propertyName;
            _value = value;
            _criteriaOperator = criteriaOperator;
        }

        public Criterion(string propertyName, CriteriaOperator criteriaOperator)
        {
            _propertyName = propertyName;
            _criteriaOperator = criteriaOperator;
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public object Value
        {
            get { return _value; }
        }

        public CriteriaOperator criteriaOperator
        {
            get { return _criteriaOperator; }
        }

        public static Criterion Create<T>(Expression<Func<T, Object>> expression,
            object value,
            CriteriaOperator criteriaOperator)
        {
            string propertyName = PropertyNameHelper.ResolvePropertyName<T>(expression);
            Criterion MyCriterion = new Criterion(propertyName, value, criteriaOperator);

            return MyCriterion;
        }

        public static Criterion Create<T>(Expression<Func<T, Object>> expression,
            CriteriaOperator criteriaOperator)
        {
            string propertyName = PropertyNameHelper.ResolvePropertyName<T>(expression);
            Criterion MyCriterion = new Criterion(propertyName, criteriaOperator);

            return MyCriterion;
        }
    }
}

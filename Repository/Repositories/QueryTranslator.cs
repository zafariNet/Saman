#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using NHibernate;
using NHibernate.Criterion;

#endregion

namespace Repository.Repositories
{
    public static class QueryTranslator
    {
        public static ICriteria TranslateIntoNHQuery<T>(this Query query, ICriteria criteria)
        {
            BuildQueryFrom(query, criteria);

            if (query.OrderByProperty != null)
                criteria.AddOrder(new Order(query.OrderByProperty.PropertyName, !query.OrderByProperty.Desc));

            return criteria;
        }

        private static void BuildQueryFrom(Query query, ICriteria criteria)
        {
            IList<ICriterion> criterions = new List<ICriterion>();

            if (query.Criteria != null)
            {
                string wildcard = @"%";

                foreach (Criterion c in query.Criteria)
                {
                    ICriterion criterion;

                    switch (c.criteriaOperator)
                    {
                        case CriteriaOperator.Equal:
                            criterion = Expression.Eq(c.PropertyName, c.Value);
                            break;
                        case CriteriaOperator.NotEqual:
                            criterion = Expression.Not(Expression.Eq(c.PropertyName, c.Value));
                            break;
                        case CriteriaOperator.LesserThanOrEqual:
                            criterion = Expression.Le(c.PropertyName, c.Value);
                            break;
                        case CriteriaOperator.GreaterThanOrEqual:
                            criterion = Expression.Ge(c.PropertyName, c.Value);
                            break;
                        case CriteriaOperator.GreaterThan:
                            criterion = Expression.Gt(c.PropertyName, c.Value);
                            break;
                        case CriteriaOperator.LesserThan:
                            criterion = Expression.Lt(c.PropertyName, c.Value);
                            break;
                        case CriteriaOperator.IsNotNullOrEmpty:
                            criterion = Expression.And(Expression.IsNotNull(c.PropertyName), Expression.IsNotEmpty(c.PropertyName));
                            break;
                        case CriteriaOperator.IsNullOrEmpty:
                            criterion = Expression.Or(Expression.IsNull(c.PropertyName), Expression.IsEmpty(c.PropertyName));
                            break;
                        case CriteriaOperator.Contains:
                            criterion = Expression.Like(c.PropertyName, wildcard + c.Value + wildcard);
                            break;
                        case CriteriaOperator.StartsWith:
                            criterion = Expression.Like(c.PropertyName, wildcard + c.Value);
                            break;
                        case CriteriaOperator.EndsWith:
                            criterion = Expression.Like(c.PropertyName, c.Value + wildcard);
                            break;
                        default:
                            throw new ApplicationException("No Operator Defined");
                    }

                    criterions.Add(criterion);
                }

                if (query.QueryOperator == QueryOperator.And)
                {
                    Conjunction andSubQuery = Expression.Conjunction();
                    foreach (ICriterion _criterion in criterions)
                    {
                        andSubQuery.Add(_criterion);
                    }
                    criteria.Add(andSubQuery);
                }
                else
                {
                    Disjunction orSubQuery = Expression.Disjunction();
                    foreach (ICriterion _criterion in criterions)
                    {
                        orSubQuery.Add(_criterion);
                    }
                    criteria.Add(orSubQuery);
                }

                foreach (Query sub in query.SubQueries)
                {
                    BuildQueryFrom(sub, criteria);
                }

            }
        }
    }
}
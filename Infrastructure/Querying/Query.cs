using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Querying
{
    public class Query
    {
        IList<Query> _subQueries = new List<Query>();
        IList<Criterion> _criteria = new List<Criterion>();

        public IEnumerable<Criterion> Criteria
        {
            get { return _criteria; }
        }

        public IEnumerable<Query> SubQueries
        {
            get { return _subQueries; }
        }

        public void AddSubQuery(Query subQuery)
        {
            _subQueries.Add(subQuery);
        }

        public void Add(Criterion criteria)
        {
            _criteria.Add(criteria);
        }

        public void Add(IEnumerable<Criterion> Criterions)
        {
            foreach (var criteria in Criterions)
                _criteria.Add(criteria);
        }

        public QueryOperator QueryOperator { get; set; }

        public OrderByClause OrderByProperty { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Querying
{
    public enum CriteriaOperator
    {
        Equal,
        LesserThanOrEqual,
        LesserThan,
        GreaterThanOrEqual,
        GreaterThan,
        NotEqual,
        IsNotNullOrEmpty,
        IsNullOrEmpty,
        Contains,
        StartsWith,
        EndsWith
    }
}

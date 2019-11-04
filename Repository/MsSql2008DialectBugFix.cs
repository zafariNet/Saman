using System;
using NHibernate.Dialect;
using NHibernate.SqlCommand;


public class MsSql2008DialectBugFix : MsSql2008Dialect
{
    public override SqlString GetLimitString(SqlString queryString, SqlString offset, SqlString limit)
    {
        var result = new SqlStringBuilder();

        if (offset == null)
        {
            var insertPoint = GetAfterSelectInsertPoint(queryString);

            result
                .Add(queryString.Substring(0, insertPoint))
                .Add(" TOP (")
                .Add(limit)
                .Add(") ")
                .Add(queryString.Substring(insertPoint));

            return result.ToSqlString();
        }

        return base.GetLimitString(queryString, offset, limit);
    }

    private static int GetAfterSelectInsertPoint(SqlString sql)
    {
        Int32 selectPosition;

        if ((selectPosition = sql.IndexOfCaseInsensitive("select distinct")) >= 0)
        {
            return selectPosition + 15; // "select distinct".Length;

        }
        if ((selectPosition = sql.IndexOfCaseInsensitive("select")) >= 0)
        {
            return selectPosition + 6; // "select".Length;
        }

        throw new NotSupportedException("The query should start with 'SELECT' or 'SELECT DISTINCT'");
    }
}
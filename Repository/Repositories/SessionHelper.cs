using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Repository.Repositories
{
    public static class SessionHelper
    {
        public static void Delete<T>(this ISession session, Guid id)
        {
            var queryString = string.Format("delete {0} where id = :id",
                                       typeof(T));
            session.CreateQuery(queryString)
                   .SetParameter("id", id)
                   .ExecuteUpdate();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Web;

namespace Repository.SessionStorage
{
    public class HttpSessionContainer : ISessionStorageContainer
    {
        string _sessionKey = "NHSession";


        public ISession GetCurrentSession()
        {
            ISession nhSession = null;

            if (HttpContext.Current.Items.Contains(_sessionKey))
                nhSession = (ISession)HttpContext.Current.Items[_sessionKey];

            return nhSession;
        }

        public void Store(ISession session)
        {
            if (HttpContext.Current.Items.Contains(_sessionKey))
                HttpContext.Current.Items[_sessionKey] = session;
            else
                HttpContext.Current.Items.Add(_sessionKey, session);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using global::NHibernate;
using System.Threading;

namespace Repository.SessionStorage
{
    public class ThreadSessionStorageContainer : ISessionStorageContainer
    {
        private static readonly Hashtable _nhSessions = new Hashtable();

        public ISession GetCurrentSession()
        {
            ISession nhSession = null;

            if (_nhSessions.Contains(GetThreadName()))
                nhSession = (ISession)_nhSessions[GetThreadName()];

            return nhSession;
        }

        public void Store(ISession session)
        {
            if (_nhSessions.Contains(GetThreadName()))
                _nhSessions[GetThreadName()] = session;
            else
                _nhSessions.Add(GetThreadName(), session);
        }

        private static string GetThreadName()
        {
            return Thread.CurrentThread.Name;
        }
    }
}

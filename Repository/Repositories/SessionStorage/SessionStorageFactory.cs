using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Repository.SessionStorage
{
    public static class SessionStorageFactory
    {
        static ISessionStorageContainer _nhsessionStorageContainer;

        public static ISessionStorageContainer GetStorageContainer()
        {
            if (_nhsessionStorageContainer == null)
            {
                if (HttpContext.Current == null)
                    _nhsessionStorageContainer = new ThreadSessionStorageContainer();
                else
                    _nhsessionStorageContainer = new HttpSessionContainer();
            }

            return _nhsessionStorageContainer;
        }
    }
}

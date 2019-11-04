using System;
using StructureMap;
using System.Web.Routing;
using System.Web.Mvc;

namespace Controllers
{
    public class IoCControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }
}
 
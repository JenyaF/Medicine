using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;

using System.Web.Mvc;

using System.Web.Routing;

using Medicine.BLL.Util;


namespace Medicine.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new DataErrorInfoModelValidatorProvider());

            NinjectModule registrations = new NinjectRegistrations();
             var kernel = new StandardKernel(registrations);
             DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            
        }
    }
}

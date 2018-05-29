using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Medicine.BLL.Services;
using Microsoft.AspNet.Identity;
using Medicine.BLL.Interfaces;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using Medicine.BLL.Util;

[assembly: OwinStartup(typeof(Medicine.WEB.App_Start.Startup))]
namespace Medicine.WEB.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login")
            });
            
        }

         private IUserService CreateUserService()
         {

            return serviceCreator.CreateUserService("DefaultContext");
        }
    }
}
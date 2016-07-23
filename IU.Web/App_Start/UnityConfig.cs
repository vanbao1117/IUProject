using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Microsoft.Owin.Security;
using System.Web;
using System.Diagnostics.CodeAnalysis;

namespace IU.Web
{
    [ExcludeFromCodeCoverage]
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
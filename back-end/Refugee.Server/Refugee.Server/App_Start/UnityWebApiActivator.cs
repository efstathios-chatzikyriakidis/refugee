using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;
using Refugee.Server;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(UnityWebApiActivator), nameof(UnityWebApiActivator.Start))]
[assembly: ApplicationShutdownMethod(typeof(UnityWebApiActivator), nameof(UnityWebApiActivator.Shutdown))]

namespace Refugee.Server
{
    public static class UnityWebApiActivator
    {
        public static void Start()
        {
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetContainer());
        }

        public static void Shutdown()
        {
            UnityConfig.GetContainer().Dispose();
        }
    }
}
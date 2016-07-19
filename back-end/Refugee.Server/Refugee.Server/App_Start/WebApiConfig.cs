using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Refugee.BusinessLogic.Infrastructure.FilterProviders;

namespace Refugee.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "GET, POST, PUT, DELETE");

            configuration.EnableCors(cors);

            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            configuration.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;

            UnityActionFilterProvider.Register(UnityConfig.GetContainer(), configuration);
        }
    }
}
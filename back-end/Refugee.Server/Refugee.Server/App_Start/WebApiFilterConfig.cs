using System.Web.Http.Filters;
using Refugee.Server.Filters;

namespace Refugee.Server
{
    public static class WebApiFilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new CustomSessionApiControllerFilter());

            filters.Add(new ExceptionFilter());
        }
    }
}
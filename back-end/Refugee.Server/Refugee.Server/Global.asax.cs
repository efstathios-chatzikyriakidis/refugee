using System;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using FluentValidation;
using Refugee.DataAccess.NHibernate.Config;
using Refugee.DataAccess.Relational.Mapping;
using Refugee.Server.Mapping;
using Serilog;

namespace Refugee.Server
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            #region MVC

            GlobalConfiguration.Configure(WebApiConfig.Register);

            WebApiFilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration.Filters);

            #endregion

            #region Logging

            Environment.SetEnvironmentVariable("APPLICATION_PHYSICAL_PATH", HostingEnvironment.ApplicationPhysicalPath);

            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

            #endregion

            #region NHibernate

            NHibernateConfiguration.Initialize(typeof(UserMap).Assembly, "Refugee", "web");

            #endregion

            #region Unity

            UnityConfig.Initialize();

            #endregion

            #region Object-Object Mapping

            Mapper.Initialize();

            #endregion

            #region Validators

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            #endregion
        }

        public override void Dispose()
        {
            NHibernateConfiguration.SessionFactory.Dispose();

            base.Dispose();
        }
    }
}
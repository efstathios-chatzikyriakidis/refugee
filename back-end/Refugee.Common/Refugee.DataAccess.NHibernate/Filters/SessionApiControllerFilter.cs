using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NHibernate;
using NHibernate.Context;
using Refugee.DataAccess.NHibernate.Config;

namespace Refugee.DataAccess.NHibernate.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class SessionApiControllerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            CurrentSessionContext.Bind(NHibernateConfiguration.SessionFactory.OpenSession());
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            ISession session = CurrentSessionContext.Unbind(NHibernateConfiguration.SessionFactory);

            if (session == null)
            {
                return;
            }

            if (session.IsOpen)
            {
                session.Close();
            }

            session.Dispose();
        }
    }
}
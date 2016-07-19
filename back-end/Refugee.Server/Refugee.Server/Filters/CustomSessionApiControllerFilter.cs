using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Refugee.DataAccess.NHibernate.Filters;
using Refugee.DataAccess.NHibernate.Transaction;

namespace Refugee.Server.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class CustomSessionApiControllerFilter : SessionApiControllerFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<TransactionAttribute>().Any() || actionContext.ActionDescriptor.GetCustomAttributes<AuthenticationFilter>().Any())
            {
                base.OnActionExecuting(actionContext);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<TransactionAttribute>().Any() || actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<AuthenticationFilter>().Any())
            {
                base.OnActionExecuted(actionExecutedContext);
            }
        }
    }
}
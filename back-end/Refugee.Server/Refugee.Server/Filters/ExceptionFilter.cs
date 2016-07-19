using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Newtonsoft.Json.Linq;
using Refugee.BusinessLogic.Infrastructure.Exceptions;
using Serilog;

namespace Refugee.Server.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            string errorMessage = "An error occurred. Try again later.";

            string messageFormat = @"{{'message' : ""{0}""}}";

            string message = errorMessage;

            if (actionExecutedContext.Exception is RestException)
            {
                RestException restException = (RestException)actionExecutedContext.Exception;

                httpStatusCode = restException.HttpStatusCode;

                message = restException.Message;
            }
            else
            {
                Log.Error("An unhandled exception [{@UnhandledException}] occurred!", actionExecutedContext.Exception);
            }

            JObject result;

            try
            {
                result = JObject.Parse(string.Format(messageFormat, message));
            }
            catch
            {
                result = JObject.Parse(string.Format(messageFormat, errorMessage));
            }

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(httpStatusCode, result);
        }
    }
}
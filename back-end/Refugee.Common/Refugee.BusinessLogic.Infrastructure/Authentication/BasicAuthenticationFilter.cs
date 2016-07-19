using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using EnsureThat;

namespace Refugee.BusinessLogic.Infrastructure.Authentication
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public abstract class BasicAuthenticationFilter : ActionFilterAttribute
    {
        #region Private Constant Fields

        private const string Scheme = "Basic";

        #endregion

        #region Protected Abstract Fields

        protected abstract IPrincipal GetPrincipal(string userName, string password);

        protected abstract string GetRealm();

        protected abstract bool RequiresHttps();

        #endregion

        #region Public Overridden Methods

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (RequiresHttps() && actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                HttpsIsRequired(actionContext);

                return;
            }

            var credentials = ExtractCredentials(actionContext);

            if (credentials == null)
            {
                Challenge(actionContext);

                return;
            }

            IPrincipal principal = GetPrincipal(credentials.Item1, credentials.Item2);

            if (principal == null)
            {
                Challenge(actionContext);

                return;
            }

            Thread.CurrentPrincipal = principal;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        #endregion

        #region Private Methods

        private Tuple<string, string> ExtractCredentials(HttpActionContext actionContext)
        {
            Ensure.That(nameof(actionContext)).IsNotNull();

            AuthenticationHeaderValue authorization = actionContext.Request.Headers.Authorization;

            if (authorization == null)
            {
                return null;
            }

            if (!authorization.Scheme.Equals(Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(authorization.Parameter))
            {
                return null;
            }

            string credentials;

            try
            {
                credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authorization.Parameter));
            }
            catch
            {
                return null;
            }

            int colonIndex = credentials.IndexOf(':');

            if (colonIndex < 0)
            {
                return null;
            }

            string userName = credentials.Substring(0, colonIndex);

            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            string password = credentials.Substring(colonIndex + 1);

            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return new Tuple<string, string>(userName, password);
        }

        private void Challenge(HttpActionContext actionContext)
        {
            Ensure.That(nameof(actionContext)).IsNotNull();

            string realm = GetRealm();

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Client is unauthorized!");

            actionContext.Response.Headers.Add("WWW-Authenticate", $@"{Scheme} realm=""{realm}""");
        }

        private void HttpsIsRequired(HttpActionContext actionContext)
        {
            Ensure.That(nameof(actionContext)).IsNotNull();

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "HTTPS is required!");
        }

        #endregion
    }
}
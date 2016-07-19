using System;
using System.Net;

namespace Refugee.BusinessLogic.Infrastructure.Exceptions
{
    public class RestException : Exception
    {
        #region Constructors

        public RestException()
        {
            HttpStatusCode = HttpStatusCode.InternalServerError;
        }

        public RestException(string message) : base(message)
        {
            HttpStatusCode = HttpStatusCode.InternalServerError;
        }

        public RestException(string message, Exception innerException) : base(message, innerException)
        {
            HttpStatusCode = HttpStatusCode.InternalServerError;
        }

        public RestException(HttpStatusCode httpStatusCode, string message, Exception innerException) : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }

        public RestException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        #endregion

        #region Public Properties

        public HttpStatusCode HttpStatusCode { get; protected set; }

        #endregion
    }
}
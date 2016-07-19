using System;
using System.Web;
using Refugee.DataAccess.Relational.Models;
using Refugee.Server.Principal;

namespace Refugee.Server.Utilities
{
    public class CurrentHttpRequest : ICurrentHttpRequest
    {
        #region Interface Implementation

        public User GetCurrentUser()
        {
            if (HttpContext.Current == null)
            {
                throw new ApplicationException("The HTTP context current is null!");
            }

            ICustomPrincipal principal = HttpContext.Current.User as ICustomPrincipal;

            if (principal == null)
            {
                throw new ApplicationException("The HTTP current user principal cannot be used!");
            }

            User user = principal.User;

            if (user == null)
            {
                throw new ApplicationException("The user does not exist in the principal!");
            }

            return user;
        }

        #endregion
    }
}
using System;
using System.Security.Principal;
using Microsoft.Practices.Unity;
using Refugee.BusinessLogic.Infrastructure.Authentication;
using Refugee.DataAccess.Relational.Dao;
using Refugee.DataAccess.Relational.Models;
using Refugee.Server.Principal;
using Refugee.Server.Properties;

namespace Refugee.Server.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AuthenticationFilter : BasicAuthenticationFilter
    {
        #region BasicAuthenticationFilter Implementation

        protected override IPrincipal GetPrincipal(string userName, string password)
        {
            User user = UserDao.GetByUserNameAndPassword(userName, password);

            if (user != null)
            {
                return new CustomPrincipal(user);
            }

            return null;
        }

        protected override string GetRealm()
        {
            return Settings.Default.AuthenticationRealm;
        }

        protected override bool RequiresHttps()
        {
            return Settings.Default.AuthenticationRequiresHttps;
        }

        #endregion

        #region Public Properties

        #region DAOs

        [Dependency]
        public IUserDao UserDao { get; protected set; }

        #endregion

        #endregion
    }
}
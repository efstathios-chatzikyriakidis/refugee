using System;
using System.Security.Principal;
using EnsureThat;
using Refugee.DataAccess.Relational.Models;

namespace Refugee.Server.Principal
{
    public class CustomPrincipal : ICustomPrincipal
    {
        #region Constructors

        public CustomPrincipal(User user)
        {
            Ensure.That(nameof(user)).IsNotNull();

            Identity = new GenericIdentity(user.UserName, "Basic");

            User = user;
        }

        #endregion

        #region IPrincipal Implementation

        #region Public Methods

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Properties

        public IIdentity Identity { get; set; }

        #endregion

        #endregion

        #region ICustomPrincipal Implementation

        #region Public Properties

        public User User { get; set; }

        #endregion

        #endregion
    }
}
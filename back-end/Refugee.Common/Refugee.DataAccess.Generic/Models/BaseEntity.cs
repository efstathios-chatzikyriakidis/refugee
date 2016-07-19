using System;

namespace Refugee.DataAccess.Generic.Models
{
    public abstract class BaseEntity : IEntity
    {
        #region Constructors

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        #endregion

        #region Public Properties

        public Guid Id { get; protected set; }

        #endregion
    }
}
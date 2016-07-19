using System;

namespace Refugee.DataAccess.Generic.Models
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
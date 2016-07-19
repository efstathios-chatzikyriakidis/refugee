using Refugee.DataAccess.Relational.Models;

namespace Refugee.Server.Utilities
{
    public interface ICurrentHttpRequest
    {
        User GetCurrentUser();
    }
}
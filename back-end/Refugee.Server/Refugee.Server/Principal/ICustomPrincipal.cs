using System.Security.Principal;
using Refugee.DataAccess.Relational.Models;

namespace Refugee.Server.Principal
{
    public interface ICustomPrincipal : IPrincipal
    {
        User User { get; set; }
    }
}
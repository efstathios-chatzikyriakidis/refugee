using System.Web.Http;
using Microsoft.Practices.Unity;
using Refugee.BusinessLogic.Infrastructure.Logging;
using Refugee.DataAccess.NHibernate.Transaction;
using Refugee.DataAccess.Relational.Models;
using Refugee.Rest.Dto.Output;
using Refugee.Server.Filters;
using Refugee.Server.Mapping;
using Refugee.Server.Utilities;
using Refugee.ServerApi;

namespace Refugee.Server.Controllers
{
    [RoutePrefix("api/users")]
    [Logging]
    public class UserController : ApiController, IUserService
    {
        #region Interface Implementation

        [Route("")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public virtual UserOutputDto GetMe()
        {
            User user = CurrentHttpRequest.GetCurrentUser();

            return Mapper.Map<User, UserOutputDto>(user);
        }

        #endregion

        #region Public Properties

        #region Utilities

        [Dependency]
        public ICurrentHttpRequest CurrentHttpRequest { get; protected set; }

        #endregion

        #endregion
    }
}
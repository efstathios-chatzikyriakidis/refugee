using System;
using System.Net;
using System.Web.Http;
using FluentValidation;
using Microsoft.Practices.Unity;
using Refugee.BusinessLogic.Infrastructure.Exceptions;
using Refugee.BusinessLogic.Infrastructure.Logging;
using Refugee.DataAccess.NHibernate.Transaction;
using Refugee.DataAccess.Relational.Dao;
using Refugee.Rest.Dto.Input;
using Refugee.Rest.Dto.Output;
using Refugee.Server.Validators;
using Refugee.ServerApi;

namespace Refugee.Server.Controllers
{
    [RoutePrefix("api/authentication")]
    [Logging]
    public class AuthenticationController : ApiController, IAuthenticationService
    {
        #region Interface Implementation

        [Route("")]
        [HttpPost]
        [Transaction]
        public virtual AuthenticationOutputDto Authenticate(AuthenticationInputDto authenticationInputDto)
        {
            if (authenticationInputDto == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.MissingInputDto, new ArgumentNullException(nameof(authenticationInputDto)));
            }

            try
            {
                AuthenticationInputDtoValidator.ValidateAndThrow(authenticationInputDto);
            }
            catch (ValidationException exception)
            {
                throw new RestException(HttpStatusCode.BadRequest, exception.Message, exception);
            }

            bool exists = UserDao.ExistsByUserNameAndPassword(authenticationInputDto.UserName, authenticationInputDto.Password);

            return new AuthenticationOutputDto { Success = exists };
        }

        #endregion

        #region Public Properties

        #region DAOs

        [Dependency]
        public IUserDao UserDao { get; protected set; }

        #endregion

        #region Validators

        [Dependency]
        public AuthenticationInputDtoValidator AuthenticationInputDtoValidator { get; protected set; }

        #endregion

        #endregion
    }
}
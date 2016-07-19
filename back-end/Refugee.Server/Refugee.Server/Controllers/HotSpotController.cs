using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using FluentValidation;
using Microsoft.Practices.Unity;
using Refugee.BusinessLogic.Infrastructure.Exceptions;
using Refugee.BusinessLogic.Infrastructure.Logging;
using Refugee.DataAccess.Graph.Models.Nodes;
using Refugee.DataAccess.Graph.UnitOfWork;
using Refugee.DataAccess.NHibernate.Transaction;
using Refugee.Rest.Dto.Input;
using Refugee.Rest.Dto.Output;
using Refugee.Server.Filters;
using Refugee.Server.Mapping;
using Refugee.Server.Validators;
using Refugee.ServerApi;

namespace Refugee.Server.Controllers
{
    [RoutePrefix("api/hotspots")]
    [Logging]
    public class HotSpotController : ApiController, IHotSpotService
    {
        #region Interface Implementation

        [Route("")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public virtual IList<HotSpotOutputDto> GetHotSpots()
        {
            IList<HotSpot> hotSpots;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                hotSpots = unitOfWork.HotSpotRepository.GetAll();
            }

            return Mapper.Map<IList<HotSpot>, IList<HotSpotOutputDto>>(hotSpots);
        }

        [Route("")]
        [HttpPost]
        [AuthenticationFilter]
        [Transaction]
        public virtual HotSpotOutputDto CreateHotSpot(CreateHotSpotInputDto createHotSpotInputDto)
        {
            if (createHotSpotInputDto == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.MissingInputDto, new ArgumentNullException(nameof(createHotSpotInputDto)));
            }

            try
            {
                CreateHotSpotInputDtoValidator.ValidateAndThrow(createHotSpotInputDto);
            }
            catch (ValidationException exception)
            {
                throw new RestException(HttpStatusCode.BadRequest, exception.Message, exception);
            }

            HotSpot hotSpot = Mapper.Map<CreateHotSpotInputDto, HotSpot>(createHotSpotInputDto);

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                unitOfWork.HotSpotRepository.Save(hotSpot);
            }

            return Mapper.Map<HotSpot, HotSpotOutputDto>(hotSpot);
        }

        [Route("{id:Guid}")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public virtual HotSpotOutputDto GetHotSpot(Guid id)
        {
            if (id == default(Guid))
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.InvalidIdentifier, new ArgumentException(Constants.Messages.InvalidIdentifier, nameof(id)));
            }

            HotSpot hotSpot;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                hotSpot = unitOfWork.HotSpotRepository.GetById(id);
            }

            if (hotSpot == null)
            {
                throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
            }

            return Mapper.Map<HotSpot, HotSpotOutputDto>(hotSpot);
        }

        [Route("{id:Guid}")]
        [HttpPut]
        [AuthenticationFilter]
        [Transaction]
        public virtual HotSpotOutputDto UpdateHotSpot(Guid id, UpdateHotSpotInputDto updateHotSpotInputDto)
        {
            if (id == default(Guid))
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.InvalidIdentifier, new ArgumentException(Constants.Messages.InvalidIdentifier, nameof(id)));
            }

            if (updateHotSpotInputDto == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.MissingInputDto, new ArgumentNullException(nameof(updateHotSpotInputDto)));
            }

            try
            {
                UpdateHotSpotInputDtoValidator.ValidateAndThrow(updateHotSpotInputDto);
            }
            catch (ValidationException exception)
            {
                throw new RestException(HttpStatusCode.BadRequest, exception.Message, exception);
            }

            HotSpot hotSpot;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                hotSpot = unitOfWork.HotSpotRepository.GetById(id);

                if (hotSpot == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                if (updateHotSpotInputDto.Name != null)
                {
                    hotSpot.Name = updateHotSpotInputDto.Name;
                }

                if (updateHotSpotInputDto.Latitude.HasValue)
                {
                    hotSpot.Latitude = updateHotSpotInputDto.Latitude.Value;
                }

                if (updateHotSpotInputDto.Longitude.HasValue)
                {
                    hotSpot.Longitude = updateHotSpotInputDto.Longitude.Value;
                }

                unitOfWork.HotSpotRepository.Update(hotSpot);
            }

            return Mapper.Map<HotSpot, HotSpotOutputDto>(hotSpot);
        }

        [Route("{id:Guid}")]
        [HttpDelete]
        [AuthenticationFilter]
        [Transaction]
        public virtual void DeleteHotSpot(Guid id)
        {
            if (id == default(Guid))
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.InvalidIdentifier, new ArgumentException(Constants.Messages.InvalidIdentifier, nameof(id)));
            }

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                HotSpot hotSpot = unitOfWork.HotSpotRepository.GetById(id);

                if (hotSpot == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                unitOfWork.HotSpotRepository.Delete(hotSpot);
            }
        }

        #endregion

        #region Public Properties

        #region Factories

        [Dependency]
        public IUnitOfWorkFactory UnitOfWorkFactory { get; protected set; }

        #endregion

        #region Validators

        [Dependency]
        public CreateHotSpotInputDtoValidator CreateHotSpotInputDtoValidator { get; protected set; }

        [Dependency]
        public UpdateHotSpotInputDtoValidator UpdateHotSpotInputDtoValidator { get; protected set; }

        #endregion

        #endregion
    }
}
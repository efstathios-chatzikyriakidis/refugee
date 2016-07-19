using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using FluentValidation;
using Microsoft.Practices.Unity;
using MoreLinq;
using Refugee.BusinessLogic.Infrastructure.Exceptions;
using Refugee.BusinessLogic.Infrastructure.Logging;
using Refugee.DataAccess.Graph.Models.Helpers;
using Refugee.DataAccess.Graph.Models.Nodes;
using Refugee.DataAccess.Graph.Models.Relationships;
using Refugee.DataAccess.Graph.UnitOfWork;
using Refugee.DataAccess.NHibernate.Transaction;
using Refugee.Rest.Dto.Input;
using Refugee.Rest.Dto.Output;
using Refugee.Server.Filters;
using Refugee.Server.Mapping;
using Refugee.Server.Validators;
using Refugee.ServerApi;

using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.Server.Controllers
{
    [RoutePrefix("api/refugees")]
    [Logging]
    public class RefugeeController : ApiController, IRefugeeService
    {
        #region Interface Implementation

        [Route("")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public virtual IList<RefugeeOutputDto> GetRefugees()
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                IList<RefugeeWithHotSpot> refugeeWithHotSpots = unitOfWork.RefugeeRepository.GetRefugeesWithHotSpots();

                IList<RefugeeOutputDto> refugeeOutputDtos = new List<RefugeeOutputDto>();

                foreach (RefugeeWithHotSpot refugeeWithHotSpotPair in refugeeWithHotSpots)
                {
                    RefugeeOutputDto refugeeOutputDto = Mapper.Map<RefugeeModel, RefugeeOutputDto>(refugeeWithHotSpotPair.Refugee);

                    refugeeOutputDto.HotSpot = Mapper.Map<HotSpot, HotSpotOutputDto>(refugeeWithHotSpotPair.HotSpot);

                    refugeeOutputDtos.Add(refugeeOutputDto);
                }

                return refugeeOutputDtos;
            }
        }

        [Route("")]
        [HttpPost]
        [AuthenticationFilter]
        [Transaction]
        public virtual RefugeeOutputDto CreateRefugee(CreateRefugeeInputDto createRefugeeInputDto)
        {
            if (createRefugeeInputDto == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.MissingInputDto, new ArgumentNullException(nameof(createRefugeeInputDto)));
            }

            try
            {
                CreateRefugeeInputDtoValidator.ValidateAndThrow(createRefugeeInputDto);
            }
            catch (ValidationException exception)
            {
                throw new RestException(HttpStatusCode.BadRequest, exception.Message, exception);
            }

            RefugeeModel refugee;

            HotSpot hotSpot;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                unitOfWork.BeginTransaction();

                hotSpot = unitOfWork.HotSpotRepository.GetById(createRefugeeInputDto.HotSpotId);

                if (hotSpot == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                refugee = Mapper.Map<CreateRefugeeInputDto, RefugeeModel>(createRefugeeInputDto);

                unitOfWork.RefugeeRepository.Save(refugee);

                unitOfWork.HotSpotRelationshipManager.Relate(refugee, hotSpot);

                unitOfWork.CommitTransaction();
            }

            RefugeeOutputDto refugeeOutputDto = Mapper.Map<RefugeeModel, RefugeeOutputDto>(refugee);

            refugeeOutputDto.HotSpot = Mapper.Map<HotSpot, HotSpotOutputDto>(hotSpot);

            return refugeeOutputDto;
        }

        [Route("{id:Guid}")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public virtual RefugeeOutputDto GetRefugee(Guid id)
        {
            if (id == default(Guid))
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.InvalidIdentifier, new ArgumentException(Constants.Messages.InvalidIdentifier, nameof(id)));
            }

            RefugeeModel refugee;

            HotSpot hotSpot;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                refugee = unitOfWork.RefugeeRepository.GetById(id);

                if (refugee == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                hotSpot = unitOfWork.HotSpotRepository.GetByRefugee(refugee);
            }

            RefugeeOutputDto refugeeOutputDto = Mapper.Map<RefugeeModel, RefugeeOutputDto>(refugee);

            refugeeOutputDto.HotSpot = Mapper.Map<HotSpot, HotSpotOutputDto>(hotSpot);

            return refugeeOutputDto;
        }

        [Route("{id:Guid}/relationships/graph")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public GraphOutputDto GetRelationshipsGraph(Guid id)
        {
            if (id == default(Guid))
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.InvalidIdentifier, new ArgumentException(Constants.Messages.InvalidIdentifier, nameof(id)));
            }

            IList<FamilyRelationshipsWithHotSpots> familyRelationshipsWithHotSpots;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                RefugeeModel refugee = unitOfWork.RefugeeRepository.GetById(id);

                if (refugee == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                familyRelationshipsWithHotSpots = unitOfWork.RefugeeRepository.GetFamilyRelationshipsWithHotSpotsByRefugee(refugee);
            }

            IList<NodeOutputDto> nodes = new List<NodeOutputDto>();

            IList<LinkOutputDto> links = new List<LinkOutputDto>();

            #region Mapping Operations

            byte refugeeNodeType = (byte)NodeType.Refugee;

            byte hotSpotNodeType = (byte)NodeType.HotSpot;

            string livesInTypeKey = LivesInRelationship.TypeKey.ToLower();

            foreach (FamilyRelationshipsWithHotSpots familyRelationshipsWithHotSpotsData in familyRelationshipsWithHotSpots)
            {
                #region Nodes

                #region Refugees

                nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.Refugee1.Id, Name = familyRelationshipsWithHotSpotsData.Refugee1.Name, Type = refugeeNodeType });

                nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.Refugee2.Id, Name = familyRelationshipsWithHotSpotsData.Refugee2.Name, Type = refugeeNodeType });

                if (familyRelationshipsWithHotSpotsData.Refugee3 != null)
                {
                    nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.Refugee3.Id, Name = familyRelationshipsWithHotSpotsData.Refugee3.Name, Type = refugeeNodeType });
                }

                #endregion

                #region HotSpots

                nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.HotSpot1.Id, Name = familyRelationshipsWithHotSpotsData.HotSpot1.Name, Type = hotSpotNodeType });

                nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.HotSpot2.Id, Name = familyRelationshipsWithHotSpotsData.HotSpot2.Name, Type = hotSpotNodeType });

                if (familyRelationshipsWithHotSpotsData.HotSpot3 != null)
                {
                    nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.HotSpot3.Id, Name = familyRelationshipsWithHotSpotsData.HotSpot3.Name, Type = hotSpotNodeType });
                }

                #endregion

                #endregion

                #region Links

                links.Add(new LinkOutputDto { SourceId = familyRelationshipsWithHotSpotsData.Refugee1.Id, TargetId = familyRelationshipsWithHotSpotsData.HotSpot1.Id, Name = livesInTypeKey });

                links.Add(new LinkOutputDto { SourceId = familyRelationshipsWithHotSpotsData.Refugee1.Id, TargetId = familyRelationshipsWithHotSpotsData.Refugee2.Id, Name = familyRelationshipsWithHotSpotsData.IsFamilyRelationshipData1.Degree.ToString().ToLower() });

                links.Add(new LinkOutputDto { SourceId = familyRelationshipsWithHotSpotsData.Refugee2.Id, TargetId = familyRelationshipsWithHotSpotsData.HotSpot2.Id, Name = livesInTypeKey });

                if (familyRelationshipsWithHotSpotsData.Refugee3 != null)
                {
                    links.Add(new LinkOutputDto { SourceId = familyRelationshipsWithHotSpotsData.Refugee3.Id, TargetId = familyRelationshipsWithHotSpotsData.HotSpot3.Id, Name = livesInTypeKey });

                    links.Add(new LinkOutputDto { SourceId = familyRelationshipsWithHotSpotsData.Refugee2.Id, TargetId = familyRelationshipsWithHotSpotsData.Refugee3.Id, Name = familyRelationshipsWithHotSpotsData.IsFamilyRelationshipData2.Degree.ToString().ToLower() });
                }

                #endregion
            }

            nodes = nodes.DistinctBy(o => o.Id).ToList();

            #endregion

            return new GraphOutputDto { Nodes = nodes, Links = links };
        }

        [Route("{id:Guid}")]
        [HttpPut]
        [AuthenticationFilter]
        [Transaction]
        public virtual RefugeeOutputDto UpdateRefugee(Guid id, [FromBody] UpdateRefugeeInputDto updateRefugeeInputDto)
        {
            if (id == default(Guid))
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.InvalidIdentifier, new ArgumentException(Constants.Messages.InvalidIdentifier, nameof(id)));
            }

            if (updateRefugeeInputDto == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.MissingInputDto, new ArgumentNullException(nameof(updateRefugeeInputDto)));
            }

            if (updateRefugeeInputDto.GenderType.HasValue && !Enum.IsDefined(typeof(GenderType), updateRefugeeInputDto.GenderType))
            {
                throw new RestException(HttpStatusCode.BadRequest, "The provided gender type is not supported.", new ArgumentException("The value of the gender type is not defined in the enumeration."));
            }

            try
            {
                UpdateRefugeeInputDtoValidator.ValidateAndThrow(updateRefugeeInputDto);
            }
            catch (ValidationException exception)
            {
                throw new RestException(HttpStatusCode.BadRequest, exception.Message, exception);
            }

            RefugeeModel refugee;

            HotSpot hotSpot;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                unitOfWork.BeginTransaction();

                refugee = unitOfWork.RefugeeRepository.GetById(id);

                if (refugee == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                hotSpot = unitOfWork.HotSpotRepository.GetByRefugee(refugee);

                if (updateRefugeeInputDto.HotSpotId.HasValue && hotSpot.Id != updateRefugeeInputDto.HotSpotId.Value)
                {
                    hotSpot = unitOfWork.HotSpotRepository.GetById(updateRefugeeInputDto.HotSpotId.Value);

                    if (hotSpot == null)
                    {
                        throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                    }

                    unitOfWork.HotSpotRelationshipManager.UnRelate(refugee);

                    unitOfWork.HotSpotRelationshipManager.Relate(refugee, hotSpot);
                }

                if (updateRefugeeInputDto.Name != null)
                {
                    refugee.Name = updateRefugeeInputDto.Name;
                }

                if (updateRefugeeInputDto.Nationality != null)
                {
                    refugee.Nationality = updateRefugeeInputDto.Nationality;
                }

                if (updateRefugeeInputDto.Passport != null)
                {
                    refugee.Passport = updateRefugeeInputDto.Passport;
                }

                if (updateRefugeeInputDto.BirthYear.HasValue)
                {
                    refugee.BirthYear = updateRefugeeInputDto.BirthYear.Value;
                }

                if (updateRefugeeInputDto.GenderType.HasValue)
                {
                    refugee.GenderType = (GenderType)updateRefugeeInputDto.GenderType.Value;
                }

                unitOfWork.RefugeeRepository.Update(refugee);

                unitOfWork.CommitTransaction();
            }

            RefugeeOutputDto refugeeOutputDto = Mapper.Map<RefugeeModel, RefugeeOutputDto>(refugee);

            refugeeOutputDto.HotSpot = Mapper.Map<HotSpot, HotSpotOutputDto>(hotSpot);

            return refugeeOutputDto;
        }

        [Route("relationships/family")]
        [HttpPost]
        [AuthenticationFilter]
        [Transaction]
        public void RelateRefugees(CreateRefugeesFamilyRelationshipInputDto createRefugeesFamilyRelationshipInputDto)
        {
            if (createRefugeesFamilyRelationshipInputDto == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.MissingInputDto, new ArgumentNullException(nameof(createRefugeesFamilyRelationshipInputDto)));
            }

            if (!Enum.IsDefined(typeof(FamilyRelationshipDegree), createRefugeesFamilyRelationshipInputDto.RelationshipDegree))
            {
                throw new RestException(HttpStatusCode.BadRequest, "The provided family relationship degree is not supported.", new ArgumentException("The value of the family relationship degree is not defined in the enumeration."));
            }

            try
            {
                CreateRefugeesFamilyRelationshipInputDtoValidator.ValidateAndThrow(createRefugeesFamilyRelationshipInputDto);
            }
            catch (ValidationException exception)
            {
                throw new RestException(HttpStatusCode.BadRequest, exception.Message, exception);
            }

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                unitOfWork.BeginTransaction();

                RefugeeModel sourceRefugee = unitOfWork.RefugeeRepository.GetById(createRefugeesFamilyRelationshipInputDto.SourceId);

                if (sourceRefugee == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                RefugeeModel targetRefugee = unitOfWork.RefugeeRepository.GetById(createRefugeesFamilyRelationshipInputDto.TargetId);

                if (targetRefugee == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                FamilyRelationshipDegree relationshipDegree = (FamilyRelationshipDegree)createRefugeesFamilyRelationshipInputDto.RelationshipDegree;

                unitOfWork.RefugeeRelationshipManager.UnRelate(sourceRefugee, targetRefugee);

                unitOfWork.RefugeeRelationshipManager.Relate(sourceRefugee, targetRefugee, new IsFamilyRelationshipData(relationshipDegree));

                unitOfWork.CommitTransaction();
            }
        }

        [Route("{id:Guid}")]
        [HttpDelete]
        [AuthenticationFilter]
        [Transaction]
        public virtual void DeleteRefugee(Guid id)
        {
            if (id == default(Guid))
            {
                throw new RestException(HttpStatusCode.BadRequest, Constants.Messages.InvalidIdentifier, new ArgumentException(Constants.Messages.InvalidIdentifier, nameof(id)));
            }

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                RefugeeModel refugee = unitOfWork.RefugeeRepository.GetById(id);

                if (refugee == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, Constants.Messages.ResourceCannotBeFound, new ArgumentException(Constants.Messages.ResourceCannotBeFound));
                }

                unitOfWork.RefugeeRepository.Delete(refugee);
            }
        }

        [Route("underage/all")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public virtual IList<RefugeeOutputDto> GetUnderageRefugees()
        {
            int currentYear = DateTime.UtcNow.Year;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                IList<RefugeeWithHotSpot> refugeeWithHotSpots = unitOfWork.RefugeeRepository.GetRefugeesWithHotSpots();

                refugeeWithHotSpots = refugeeWithHotSpots.Where(o => (currentYear - o.Refugee.BirthYear) < Properties.Settings.Default.UnderageAgeThreshold).ToList();

                IList<RefugeeOutputDto> refugeeOutputDtos = new List<RefugeeOutputDto>();

                foreach (RefugeeWithHotSpot refugeeWithHotSpotPair in refugeeWithHotSpots)
                {
                    RefugeeOutputDto refugeeOutputDto = Mapper.Map<RefugeeModel, RefugeeOutputDto>(refugeeWithHotSpotPair.Refugee);

                    refugeeOutputDto.HotSpot = Mapper.Map<HotSpot, HotSpotOutputDto>(refugeeWithHotSpotPair.HotSpot);

                    refugeeOutputDtos.Add(refugeeOutputDto);
                }

                return refugeeOutputDtos;
            }
        }

        [Route("adultsWithNoFamily/all")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public virtual IList<RefugeeOutputDto> GetAdultRefugeesWithNoFamily()
        {
            int currentYear = DateTime.UtcNow.Year;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                IList<RefugeeWithHotSpot> refugeeWithHotSpots = unitOfWork.RefugeeRepository.GetRefugeesWithNoFamilyAndWithHotSpots();

                refugeeWithHotSpots = refugeeWithHotSpots.Where(o => (currentYear - o.Refugee.BirthYear) >= Properties.Settings.Default.UnderageAgeThreshold).ToList();

                IList<RefugeeOutputDto> refugeeOutputDtos = new List<RefugeeOutputDto>();

                foreach (RefugeeWithHotSpot refugeeWithHotSpotPair in refugeeWithHotSpots)
                {
                    RefugeeOutputDto refugeeOutputDto = Mapper.Map<RefugeeModel, RefugeeOutputDto>(refugeeWithHotSpotPair.Refugee);

                    refugeeOutputDto.HotSpot = Mapper.Map<HotSpot, HotSpotOutputDto>(refugeeWithHotSpotPair.HotSpot);

                    refugeeOutputDtos.Add(refugeeOutputDto);
                }

                return refugeeOutputDtos;
            }
        }

        [Route("familiesWithChildren/graph")]
        [HttpGet]
        [AuthenticationFilter]
        [Transaction]
        public virtual GraphOutputDto GetFamiliesWithChildrenGraph()
        {
            List<FamilyRelationshipsWithHotSpots> familyRelationshipsWithHotSpots;

            int currentYear = DateTime.UtcNow.Year;

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                familyRelationshipsWithHotSpots = unitOfWork.RefugeeRepository.GetFamilyRelationshipsWithHotSpots().ToList();

                familyRelationshipsWithHotSpots.RemoveAll(o =>
                   ((currentYear - o.Refugee1.BirthYear) >= Properties.Settings.Default.UnderageAgeThreshold) &&
                   ((currentYear - o.Refugee2.BirthYear) >= Properties.Settings.Default.UnderageAgeThreshold));
            }

            IList<NodeOutputDto> nodes = new List<NodeOutputDto>();

            IList<LinkOutputDto> links = new List<LinkOutputDto>();

            #region Mapping Operations

            byte refugeeNodeType = (byte)NodeType.Refugee;

            byte hotSpotNodeType = (byte)NodeType.HotSpot;

            string livesInTypeKey = LivesInRelationship.TypeKey.ToLower();

            foreach (FamilyRelationshipsWithHotSpots familyRelationshipsWithHotSpotsData in familyRelationshipsWithHotSpots)
            {
                #region Nodes

                #region Refugees

                nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.Refugee1.Id, Name = familyRelationshipsWithHotSpotsData.Refugee1.Name, Type = refugeeNodeType });

                nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.Refugee2.Id, Name = familyRelationshipsWithHotSpotsData.Refugee2.Name, Type = refugeeNodeType });

                #endregion

                #region HotSpots

                nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.HotSpot1.Id, Name = familyRelationshipsWithHotSpotsData.HotSpot1.Name, Type = hotSpotNodeType });

                nodes.Add(new NodeOutputDto { Id = familyRelationshipsWithHotSpotsData.HotSpot2.Id, Name = familyRelationshipsWithHotSpotsData.HotSpot2.Name, Type = hotSpotNodeType });

                #endregion

                #endregion

                #region Links

                links.Add(new LinkOutputDto { SourceId = familyRelationshipsWithHotSpotsData.Refugee1.Id, TargetId = familyRelationshipsWithHotSpotsData.HotSpot1.Id, Name = livesInTypeKey });

                links.Add(new LinkOutputDto { SourceId = familyRelationshipsWithHotSpotsData.Refugee1.Id, TargetId = familyRelationshipsWithHotSpotsData.Refugee2.Id, Name = familyRelationshipsWithHotSpotsData.IsFamilyRelationshipData1.Degree.ToString().ToLower() });

                links.Add(new LinkOutputDto { SourceId = familyRelationshipsWithHotSpotsData.Refugee2.Id, TargetId = familyRelationshipsWithHotSpotsData.HotSpot2.Id, Name = livesInTypeKey });

                #endregion
            }

            nodes = nodes.DistinctBy(o => o.Id).ToList();

            #endregion

            return new GraphOutputDto { Nodes = nodes, Links = links };
        }

        #endregion

        #region Public Properties

        #region Factories

        [Dependency]
        public IUnitOfWorkFactory UnitOfWorkFactory { get; protected set; }

        #endregion

        #region Validators

        [Dependency]
        public CreateRefugeeInputDtoValidator CreateRefugeeInputDtoValidator { get; protected set; }

        [Dependency]
        public CreateRefugeesFamilyRelationshipInputDtoValidator CreateRefugeesFamilyRelationshipInputDtoValidator { get; protected set; }

        [Dependency]
        public UpdateRefugeeInputDtoValidator UpdateRefugeeInputDtoValidator { get; protected set; }

        #endregion

        #endregion
    }
}
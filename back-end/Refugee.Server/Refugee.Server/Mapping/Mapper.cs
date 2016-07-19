using AutoMapper;
using EnsureThat;
using Refugee.DataAccess.Graph.Models.Helpers;
using Refugee.DataAccess.Graph.Models.Nodes;
using Refugee.DataAccess.Relational.Models;
using Refugee.Rest.Dto.Input;
using Refugee.Rest.Dto.Output;

using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.Server.Mapping
{
    public static class Mapper
    {
        #region Private Fields

        private static IMapper _mapper;

        #endregion

        #region Private Properties

        private static IMapper Instance
        {
            get
            {
                if (_mapper == null)
                {
                    Initialize();
                }

                return _mapper;
            }
        }

        #endregion

        #region Public Methods

        public static TDestination Map<TSource, TDestination>(TSource source) where TSource : class
        {
            Ensure.That(nameof(source)).IsNotNull();

            return Instance.Map<TSource, TDestination>(source);
        }

        public static void Initialize()
        {
            if (_mapper == null)
            {
                var mapperConfiguration = CreateConfiguration();

                mapperConfiguration.AssertConfigurationIsValid();

                _mapper = mapperConfiguration.CreateMapper();
            }
        }

        #endregion

        #region Private Methods

        private static MapperConfiguration CreateConfiguration()
        {
            return new MapperConfiguration(o =>
            {
                #region User -> UserOutputDto

                o.CreateMap<User, UserOutputDto>();

                #endregion

                #region HotSpot -> HotSpotOutputDto

                o.CreateMap<HotSpot, HotSpotOutputDto>();

                #endregion

                #region Refugee -> RefugeeOutputDto

                o.CreateMap<RefugeeModel, RefugeeOutputDto>()
                    .ForMember(dest => dest.GenderType, opt => opt.MapFrom(src => (byte)src.GenderType))
                    .ForMember(dest => dest.HotSpot, opt => opt.Ignore());

                #endregion

                #region CreateHotSpotInputDto -> HotSpot

                o.CreateMap<CreateHotSpotInputDto, HotSpot>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

                #endregion

                #region CreateRefugeeInputDto -> Refugee

                o.CreateMap<CreateRefugeeInputDto, RefugeeModel>()
                    .ForMember(dest => dest.GenderType, opt => opt.MapFrom(src => (GenderType)src.GenderType))
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

                #endregion
            });
        }

        #endregion
    }
}
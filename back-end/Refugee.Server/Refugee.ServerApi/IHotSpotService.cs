using System;
using System.Collections.Generic;
using Refugee.Rest.Dto.Input;
using Refugee.Rest.Dto.Output;

namespace Refugee.ServerApi
{
    public interface IHotSpotService
    {
        IList<HotSpotOutputDto> GetHotSpots();

        HotSpotOutputDto CreateHotSpot(CreateHotSpotInputDto createHotSpotInputDto);

        HotSpotOutputDto GetHotSpot(Guid id);

        HotSpotOutputDto UpdateHotSpot(Guid id, UpdateHotSpotInputDto updateHotSpotInputDto);

        void DeleteHotSpot(Guid id);
    }
}
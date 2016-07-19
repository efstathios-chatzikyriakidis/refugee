using System;
using System.Collections.Generic;
using Refugee.Rest.Dto.Input;
using Refugee.Rest.Dto.Output;

namespace Refugee.ServerApi
{
    public interface IRefugeeService
    {
        IList<RefugeeOutputDto> GetRefugees();

        RefugeeOutputDto CreateRefugee(CreateRefugeeInputDto createRefugeeInputDto);

        RefugeeOutputDto GetRefugee(Guid id);

        RefugeeOutputDto UpdateRefugee(Guid id, UpdateRefugeeInputDto updateRefugeeInputDto);

        void RelateRefugees(CreateRefugeesFamilyRelationshipInputDto createRefugeesFamilyRelationshipInputDto);

        GraphOutputDto GetRelationshipsGraph(Guid id);

        void DeleteRefugee(Guid id);

        IList<RefugeeOutputDto> GetUnderageRefugees();

        IList<RefugeeOutputDto> GetAdultRefugeesWithNoFamily();

        GraphOutputDto GetFamiliesWithChildrenGraph();
    }
}
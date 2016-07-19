using System.Collections.Generic;
using Refugee.DataAccess.Generic.Repository;
using Refugee.DataAccess.Graph.Models.Helpers;

using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Repositories
{
    public interface IRefugeeRepository : IGenericRepository<RefugeeModel>
    {
        IList<RefugeeWithHotSpot> GetRefugeesWithHotSpots();

        IList<FamilyRelationshipsWithHotSpots> GetFamilyRelationshipsWithHotSpotsByRefugee(RefugeeModel refugee);

        IList<RefugeeWithHotSpot> GetRefugeesWithNoFamilyAndWithHotSpots();

        IList<FamilyRelationshipsWithHotSpots> GetFamilyRelationshipsWithHotSpots();
    }
}
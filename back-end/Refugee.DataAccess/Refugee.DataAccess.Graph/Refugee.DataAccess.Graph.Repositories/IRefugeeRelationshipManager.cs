using Refugee.DataAccess.Graph.Models.Relationships;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Repositories
{
    public interface IRefugeeRelationshipManager
    {
        void Relate(RefugeeModel source, RefugeeModel target, IsFamilyRelationshipData isFamilyRelationshipData);

        void UnRelate(RefugeeModel refugeeSource, RefugeeModel refugeeTarget);
    }
}
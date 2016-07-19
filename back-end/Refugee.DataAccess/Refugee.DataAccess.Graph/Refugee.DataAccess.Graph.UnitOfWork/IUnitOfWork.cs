using Refugee.DataAccess.Generic.UnitOfWork;
using Refugee.DataAccess.Graph.Repositories;

namespace Refugee.DataAccess.Graph.UnitOfWork
{
    public interface IUnitOfWork : IGenericUnitOfWork
    {
        IRefugeeRepository RefugeeRepository { get; }

        IHotSpotRepository HotSpotRepository { get; }

        IHotSpotRelationshipManager HotSpotRelationshipManager { get; }

        IRefugeeRelationshipManager RefugeeRelationshipManager { get; }
    }
}
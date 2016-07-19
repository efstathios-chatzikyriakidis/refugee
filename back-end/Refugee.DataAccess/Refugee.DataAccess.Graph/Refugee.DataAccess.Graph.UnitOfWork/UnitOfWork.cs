using EnsureThat;
using Neo4jClient;
using Refugee.DataAccess.Graph.Repositories;
using Refugee.DataAccess.Graph.Repositories.Neo4j;
using Refugee.DataAccess.Neo4j.UnitOfWork;

namespace Refugee.DataAccess.Graph.UnitOfWork
{
    public class UnitOfWork : Neo4jUnitOfWork, IUnitOfWork
    {
        #region Constructors

        public UnitOfWork(GraphClient graphClient) : base(graphClient)
        {
            Ensure.That(nameof(graphClient)).IsNotNull();

            #region Repositories

            #region Refugee

            var refugeeRepository = new RefugeeRepository();

            refugeeRepository.SetGraphClient(graphClient);

            RefugeeRepository = refugeeRepository;

            #endregion

            #region HotSpot

            var hotSpotRepository = new HotSpotRepository();

            hotSpotRepository.SetGraphClient(graphClient);

            HotSpotRepository = hotSpotRepository;

            #endregion

            #endregion

            #region Relationship Managers

            #region HotSpot

            var hotSpotRelationshipManager = new HotSpotRelationshipManager();

            hotSpotRelationshipManager.SetGraphClient(graphClient);

            HotSpotRelationshipManager = hotSpotRelationshipManager;

            #endregion

            #region Refugee

            var refugeeRelationshipManager = new RefugeeRelationshipManager();

            refugeeRelationshipManager.SetGraphClient(graphClient);

            RefugeeRelationshipManager = refugeeRelationshipManager;

            #endregion

            #endregion
        }

        #endregion

        #region Public Properties

        #region Repositories

        public IRefugeeRepository RefugeeRepository { get; protected set; }

        public IHotSpotRepository HotSpotRepository { get; protected set; }

        #endregion

        #region Relationship Managers

        public IHotSpotRelationshipManager HotSpotRelationshipManager { get; protected set; }

        public IRefugeeRelationshipManager RefugeeRelationshipManager { get; protected set; }

        #endregion

        #endregion
    }
}
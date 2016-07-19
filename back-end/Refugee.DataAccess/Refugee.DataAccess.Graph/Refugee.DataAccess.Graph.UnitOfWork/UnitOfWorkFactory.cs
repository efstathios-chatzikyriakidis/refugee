using EnsureThat;
using Neo4jClient;

namespace Refugee.DataAccess.Graph.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        #region Constructors

        public UnitOfWorkFactory(IGraphClientFactory graphClientFactory)
        {
            Ensure.That(nameof(graphClientFactory)).IsNotNull();

            _graphClientFactory = graphClientFactory;
        }

        #endregion

        #region Interface Implementation

        public IUnitOfWork Create()
        {
            GraphClient graphClient = (GraphClient)_graphClientFactory.Create();

            return new UnitOfWork(graphClient);
        }

        #endregion

        #region Private Readonly Fields

        private readonly IGraphClientFactory _graphClientFactory;

        #endregion
    }
}
using System;
using EnsureThat;
using Neo4jClient;

namespace Refugee.DataAccess.Graph.Repositories.Neo4j
{
    public abstract class RelationshipManager : IRelationshipManager
    {
        #region Interface Implementation

        public void SetGraphClient(IGraphClient graphClient)
        {
            Ensure.That(nameof(graphClient)).IsNotNull();

            _graphClient = graphClient;
        }

        #endregion

        #region Protected Properties

        protected IGraphClient GraphClient
        {
            get
            {
                if (_graphClient == null)
                {
                    throw new ApplicationException("You should first initialize the graph client.");
                }

                return _graphClient;
            }
        }

        #endregion

        #region Private Fields

        private IGraphClient _graphClient;

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using Neo4jClient;
using Refugee.DataAccess.Generic.Models;

namespace Refugee.DataAccess.Neo4j.Repository
{
    public class Neo4jRepository<TModel> : INeo4jRepository<TModel> where TModel : IEntity
    {
        #region IGenericRepository<TModel> Implementation

        public IList<TModel> GetAll()
        {
            return GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Return(e => e.As<TModel>())
                                     .Results
                                     .ToList();
        }

        public TModel GetById(Guid id)
        {
            return GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Where<IEntity>(e => e.Id == id)
                                     .Return(e => e.As<TModel>())
                                     .Results
                                     .SingleOrDefault();
        }

        public TModel Save(TModel model)
        {
            return GraphClient.Cypher.Create($"(e:{typeof(TModel).Name} {{model}})")
                                     .WithParam("model", model)
                                     .Return(e => e.As<TModel>())
                                     .Results
                                     .Single();
        }

        public TModel Update(TModel model)
        {
            Guid id = model.Id;

            return GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Where<IEntity>(e => e.Id == id)
                                     .Set("e = {model}")
                                     .WithParam("model", model)
                                     .Return(e => e.As<TModel>())
                                     .Results
                                     .Single();
        }

        public void Delete(TModel model)
        {
            Guid id = model.Id;

            GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                              .Where<IEntity>(e => e.Id == id)
                              .DetachDelete("e")
                              .ExecuteWithoutResults();
        }

        #endregion

        #region INeo4jRepository<TModel> Implementation

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
using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using Refugee.DataAccess.Graph.Models.Helpers;
using Refugee.DataAccess.Graph.Models.Nodes;
using Refugee.DataAccess.Graph.Models.Relationships;
using Refugee.DataAccess.Neo4j.Repository;

using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Repositories.Neo4j
{
    public class RefugeeRepository : Neo4jRepository<RefugeeModel>, IRefugeeRepository
    {
        #region Interface Implementation

        public IList<RefugeeWithHotSpot> GetRefugeesWithHotSpots()
        {
            string refugeeLabel = typeof(RefugeeModel).Name;

            string hotSpotLabel = typeof(HotSpot).Name;

            var queryResult = GraphClient.Cypher.Match($"(r:{refugeeLabel})-[:{LivesInRelationship.TypeKey}]->(h:{hotSpotLabel})")
                                         .Return((r, h) => new
                                         {
                                             Refugee = r.As<RefugeeModel>(),
                                             HotSpot = h.As<HotSpot>()
                                         })
                                         .Results
                                         .ToList();

            return queryResult.Select(o => new RefugeeWithHotSpot(o.Refugee, o.HotSpot)).ToList();
        }

        public IList<FamilyRelationshipsWithHotSpots> GetFamilyRelationshipsWithHotSpotsByRefugee(RefugeeModel refugee)
        {
            Ensure.That(nameof(refugee)).IsNotNull();

            string refugeeLabel = typeof(RefugeeModel).Name;

            string hotSpotLabel = typeof(HotSpot).Name;

            Guid id = refugee.Id;

            var queryResult = GraphClient.Cypher.Match($"(h1:{hotSpotLabel})<-[l1:{LivesInRelationship.TypeKey}]-(r1:{refugeeLabel} {{ Id: {{id}} }})-[rr1:{IsFamilyRelationship.TypeKey}]-(r2:{refugeeLabel})-[l2:{LivesInRelationship.TypeKey}]->(h2:{hotSpotLabel})")
                                                .OptionalMatch($"(r2)-[rr2:{IsFamilyRelationship.TypeKey}]-(r3:{refugeeLabel})-[:{LivesInRelationship.TypeKey}]->(h3:{hotSpotLabel})")
                                                .Where((RefugeeModel r3) => r3.Id != id)
                                                .WithParam("id", id)
                                                .Return((h1, r1, rr1, r2, h2, rr2, r3, h3) => new
                                                {
                                                    HotSpot1 = h1.As<HotSpot>(),
                                                    Refugee1 = r1.As<RefugeeModel>(),
                                                    IsFamilyRelationshipData1 = rr1.As<IsFamilyRelationshipData>(),
                                                    Refugee2 = r2.As<RefugeeModel>(),
                                                    HotSpot2 = h2.As<HotSpot>(),
                                                    IsFamilyRelationshipData2 = rr2.As<IsFamilyRelationshipData>(),
                                                    Refugee3 = r3.As<RefugeeModel>(),
                                                    HotSpot3 = h3.As<HotSpot>()
                                                })
                                                .Results
                                                .ToList();

            return queryResult.Select(o => new FamilyRelationshipsWithHotSpots(o.HotSpot1, o.Refugee1, o.IsFamilyRelationshipData1, o.Refugee2, o.HotSpot2, o.IsFamilyRelationshipData2, o.Refugee3, o.HotSpot3)).ToList();
        }

        public IList<RefugeeWithHotSpot> GetRefugeesWithNoFamilyAndWithHotSpots()
        {
            string refugeeLabel = typeof(RefugeeModel).Name;

            string hotSpotLabel = typeof(HotSpot).Name;

            var queryResult = GraphClient.Cypher.Match($"(r1:{refugeeLabel})-[:{LivesInRelationship.TypeKey}]->(h:{hotSpotLabel})")
                                                .Where($"NOT (r1)-[:{IsFamilyRelationship.TypeKey}]-(:{refugeeLabel})")
                                                .Return((r1, h) => new
                                                {
                                                    Refugee = r1.As<RefugeeModel>(),
                                                    HotSpot = h.As<HotSpot>()
                                                })
                                                .Results
                                                .ToList();

            return queryResult.Select(o => new RefugeeWithHotSpot(o.Refugee, o.HotSpot)).ToList();
        }

        public IList<FamilyRelationshipsWithHotSpots> GetFamilyRelationshipsWithHotSpots()
        {
            string refugeeLabel = typeof(RefugeeModel).Name;

            string hotSpotLabel = typeof(HotSpot).Name;

            var queryResult = GraphClient.Cypher.Match($"(h1:{hotSpotLabel})<-[l1:{LivesInRelationship.TypeKey}]-(r1:{refugeeLabel})-[r:{IsFamilyRelationship.TypeKey}]-(r2:{refugeeLabel})-[l2:{LivesInRelationship.TypeKey}]->(h2:{hotSpotLabel})")
                                                .Return((h1, r1, r, r2, h2) => new
                                                {
                                                    HotSpot1 = h1.As<HotSpot>(),
                                                    Refugee1 = r1.As<RefugeeModel>(),
                                                    IsFamilyRelationshipData1 = r.As<IsFamilyRelationshipData>(),
                                                    Refugee2 = r2.As<RefugeeModel>(),
                                                    HotSpot2 = h2.As<HotSpot>()
                                                })
                                                .Results
                                                .ToList();

            return queryResult.Select(o => new FamilyRelationshipsWithHotSpots(o.HotSpot1, o.Refugee1, o.IsFamilyRelationshipData1, o.Refugee2, o.HotSpot2)).ToList();
        }


        #endregion
    }
}
using System;
using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Input
{
    public class CreateRefugeesFamilyRelationshipInputDto
    {
        [JsonProperty("sourceId")]
        public Guid SourceId { get; set; }

        [JsonProperty("targetId")]
        public Guid TargetId { get; set; }

        [JsonProperty("relationshipDegree")]
        public byte RelationshipDegree { get; set; }
    }
}
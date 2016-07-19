using System;
using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Output
{
    public class LinkOutputDto
    {
        [JsonProperty("sourceId")]
        public Guid SourceId { get; set; }

        [JsonProperty("targetId")]
        public Guid TargetId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
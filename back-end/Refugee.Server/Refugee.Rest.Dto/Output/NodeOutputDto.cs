using System;
using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Output
{
    public class NodeOutputDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public byte Type { get; set; }
    }
}
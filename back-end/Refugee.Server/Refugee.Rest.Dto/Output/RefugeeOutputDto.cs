using System;
using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Output
{
    public class RefugeeOutputDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("genderType")]
        public byte GenderType { get; set; }

        [JsonProperty("passport")]
        public string Passport { get; set; }

        [JsonProperty("birthYear")]
        public int BirthYear { get; set; }

        [JsonProperty("hotSpot")]
        public HotSpotOutputDto HotSpot { get; set; }
    }
}
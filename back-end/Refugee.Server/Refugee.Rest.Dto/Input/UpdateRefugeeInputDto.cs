using System;
using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Input
{
    public class UpdateRefugeeInputDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("genderType")]
        public byte? GenderType { get; set; }

        [JsonProperty("passport")]
        public string Passport { get; set; }

        [JsonProperty("birthYear")]
        public int? BirthYear { get; set; }

        [JsonProperty("hotSpotId")]
        public Guid? HotSpotId { get; set; }
    }
}
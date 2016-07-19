using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Input
{
    public class CreateHotSpotInputDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }
    }
}
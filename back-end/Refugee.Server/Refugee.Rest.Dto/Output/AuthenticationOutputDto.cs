using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Output
{
    public class AuthenticationOutputDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
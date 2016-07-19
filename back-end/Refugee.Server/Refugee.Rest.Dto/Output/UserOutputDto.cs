using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Output
{
    public class UserOutputDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("realName")]
        public string RealName { get; set; }
    }
}
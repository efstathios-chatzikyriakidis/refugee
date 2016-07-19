﻿using Newtonsoft.Json;

namespace Refugee.Rest.Dto.Input
{
    public class AuthenticationInputDto
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace ETicaretAPI.Application.DTOs.Facebook
{
    public class FaceBookAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}

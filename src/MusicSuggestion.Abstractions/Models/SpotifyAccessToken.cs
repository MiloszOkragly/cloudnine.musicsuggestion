using Newtonsoft.Json;

namespace MusicSuggestion.Abstractions.Models;

public class SpotifyAccessToken
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAt => this.ExpiresIn > 0 ?
        this.CreatedAt.AddSeconds(this.ExpiresIn) : DateTime.MinValue;
}

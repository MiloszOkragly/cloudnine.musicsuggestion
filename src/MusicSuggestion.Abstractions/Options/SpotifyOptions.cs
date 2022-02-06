namespace MusicSuggestion.Abstractions.Options;

public class SpotifyOptions
{
    public string ApiBaseUri { get; set; } = string.Empty;

    public string AuthenticationBaseUri { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string GetAccessTokenEndpoint { get; set; } = string.Empty;

    public string GetAvailableGenresEndpoint { get; set; } = string.Empty;

    public string GetSearchEndpoint { get; set; } = string.Empty;

    public string GetRecommendationsEndpoint { get; set; } = string.Empty;
}

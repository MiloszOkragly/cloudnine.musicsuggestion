using Microsoft.Extensions.Options;
using MusicSuggestion.Abstractions.Models;
using MusicSuggestion.Abstractions.Options;
using MusicSuggestion.SpotifyApi.Interfaces;
using System.Net.Http.Headers;

namespace MusicSuggestion.SpotifyApi.Services;

public class SpotifyClient : ISpotifyClient, IDisposable
{
    private readonly SpotifyOptions _options;
    private readonly HttpClient _httpClient;
    private SpotifyAccessToken? _accessToken = default;

    public SpotifyClient(IOptions<SpotifyOptions> options)
    {
        _httpClient = new HttpClient();
        _options = options.Value;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }

    private async Task<string> GetAccessToken(CancellationToken token)
    {
        if (_accessToken == default || _accessToken.ExpiresAt <= DateTime.UtcNow)
        {
            var form = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret)
            };

            var response = await _httpClient.PostAsync(
                $"{_options.AuthenticationBaseUri}{_options.GetAccessTokenEndpoint}",
                new FormUrlEncodedContent(form),
                token);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(token);

            _accessToken = CustomJsonConverter.DeserializeObjectOrThrow<SpotifyAccessToken>(responseJson);
        }

        return _accessToken.AccessToken;
    }

    public async Task<AvailableGenreSeed> GetAvailableGenresAsync(CancellationToken token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.ApiBaseUri}{_options.GetAvailableGenresEndpoint}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await this.GetAccessToken(token));

        var response = await _httpClient.SendAsync(request, token);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(token);
        return CustomJsonConverter.DeserializeObjectOrThrow<AvailableGenreSeed>(responseJson);
    }

    public async Task<Artists> GetArtistsAsync(string query, CancellationToken token)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("type", "artist"),
            new KeyValuePair<string, string>("q", query)
        };

        var responseJson = await SendApiRequestAsync(_options.GetSearchEndpoint, parameters, token);
        return CustomJsonConverter.DeserializeObjectOrThrow<SearchResult>(responseJson).Artists;
    }

    public async Task<Tracks> GetTracksAsync(string query, CancellationToken token)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("type", "track"),
            new KeyValuePair<string, string>("q", query)
        };

        var responseJson = await SendApiRequestAsync(_options.GetSearchEndpoint, parameters, token);
        return CustomJsonConverter.DeserializeObjectOrThrow<SearchResult>(responseJson).Tracks;
    }

    public async Task<Recommendations> GetRecommendationsAsync(IEnumerable<Seed> seeds, CancellationToken token)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("seed_genres", string.Join(",", seeds.Where(s => s.Type == Seed.GENRE_KEY).Select(s => s.Value))),
            new KeyValuePair<string, string>("seed_artists", string.Join(",", seeds.Where(s => s.Type == Seed.ARTIST_KEY).Select(s => s.Value))),
            new KeyValuePair<string, string>("seed_tracks", string.Join(",", seeds.Where(s => s.Type == Seed.TRACK_KEY).Select(s => s.Value))),
        };

        var responseJson = await SendApiRequestAsync(_options.GetRecommendationsEndpoint, parameters, token);
        return CustomJsonConverter.DeserializeObjectOrThrow<Recommendations>(responseJson);
    }

    private async Task<string> SendApiRequestAsync(string endpoint, IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken token)
    {
        var query = string.Empty;
        if (parameters.Any())
        {
            query = $"?{string.Join("&", parameters.Select(p => p.Key + "=" + p.Value))}";
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.ApiBaseUri}{endpoint}{query}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await this.GetAccessToken(token));

        var response = await _httpClient.SendAsync(request, token);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(token);
    }
}

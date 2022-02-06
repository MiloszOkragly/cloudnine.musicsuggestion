using MusicSuggestion.Abstractions.Models;

namespace MusicSuggestion.SpotifyApi.Interfaces;

public interface ISpotifyService
{
    public Task<AvailableGenreSeed> GetAvailableGenresAsync(CancellationToken token);

    public Task<Artists> GetArtistsAsync(string query, CancellationToken token);

    public Task<Tracks> GetTracksAsync(string query, CancellationToken token);

    public Task<Recommendations> GetRecommendationsAsync(IEnumerable<Seed> seeds, CancellationToken token);
}

using MusicSuggestion.Abstractions.Models;
using MusicSuggestion.SpotifyApi.Interfaces;

namespace MusicSuggestion.SpotifyApi.Services;

public class SpotifyService : ISpotifyService
{
    private const int MAX_SEED_COUNT = 5;
    private readonly ISpotifyClient _spotifyClient;
    private readonly ISpotifyCacheService _spotifyCacheService;

    public SpotifyService(ISpotifyClient spotifyClient, ISpotifyCacheService spotifyCacheService)
    {
        _spotifyClient = spotifyClient;
        _spotifyCacheService = spotifyCacheService;
    }

    public async Task<AvailableGenreSeed> GetAvailableGenresAsync(CancellationToken token)
    {
        var cachedGenres = await _spotifyCacheService.GetAvailableGenreSeedFromCacheAsync(token);

        if (cachedGenres == null)
        {
            cachedGenres = await _spotifyClient.GetAvailableGenresAsync(token);
            await _spotifyCacheService.SetAvailableGenreSeedAsync(cachedGenres, token);
        }

        return cachedGenres;
    }

    public async Task<Artists> GetArtistsAsync(string query, CancellationToken token)
    {
        return await _spotifyClient.GetArtistsAsync(query, token);
    }

    public async Task<Tracks> GetTracksAsync(string query, CancellationToken token)
    {
        return await _spotifyClient.GetTracksAsync(query, token);
    }

    public async Task<Recommendations> GetRecommendationsAsync(IEnumerable<Seed> seeds, CancellationToken token)
    {
        var adjustedSeeds = GetAdjustSeeds(seeds);

        return await _spotifyClient.GetRecommendationsAsync(adjustedSeeds, token);
    }

    private static IEnumerable<Seed> GetAdjustSeeds(IEnumerable<Seed> seeds)
    {
        if (seeds.Count() > MAX_SEED_COUNT)
        {
            var shuffledSeeds = seeds.OrderBy(a => Guid.NewGuid()).Take(MAX_SEED_COUNT).ToList();
            seeds = shuffledSeeds;
        }

        return seeds;
    }
}

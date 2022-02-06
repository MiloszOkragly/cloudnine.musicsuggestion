using Microsoft.Extensions.Caching.Distributed;
using MusicSuggestion.Abstractions.Models;
using MusicSuggestion.SpotifyApi.Extentions;
using MusicSuggestion.SpotifyApi.Interfaces;

namespace MusicSuggestion.SpotifyApi.Services;

public class SpotifyCacheService : ISpotifyCacheService
{
    public const string SPOTIFY_GENRES_CACHE_KEY = nameof(SPOTIFY_GENRES_CACHE_KEY);

    private readonly IDistributedCache _cache;

    public SpotifyCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetAvailableGenreSeedAsync(AvailableGenreSeed availableGenreSeed, CancellationToken token)
    {
        await _cache.SetAsync(
            SPOTIFY_GENRES_CACHE_KEY,
            availableGenreSeed.ToByteArray(),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            },
            token);
    }

    public async Task<AvailableGenreSeed?> GetAvailableGenreSeedFromCacheAsync(CancellationToken token)
    {
        var availableGenreSeedByteArray = await _cache.GetAsync(SPOTIFY_GENRES_CACHE_KEY, token);
        return availableGenreSeedByteArray.FromByteArray<AvailableGenreSeed>();
    }
}

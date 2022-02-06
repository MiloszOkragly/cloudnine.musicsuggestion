using MusicSuggestion.Abstractions.Models;

namespace MusicSuggestion.SpotifyApi.Interfaces;

public interface ISpotifyCacheService
{
    public Task SetAvailableGenreSeedAsync(AvailableGenreSeed availableGenreSeed, CancellationToken token);

    public Task<AvailableGenreSeed?> GetAvailableGenreSeedFromCacheAsync(CancellationToken token);
}

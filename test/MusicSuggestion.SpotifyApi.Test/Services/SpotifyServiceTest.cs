using AutoFixture;
using FakeItEasy;
using MusicSuggestion.Abstractions.Models;
using MusicSuggestion.SpotifyApi.Interfaces;
using MusicSuggestion.SpotifyApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MusicSuggestion.SpotifyApi.Test.Services;

public class SpotifyServiceTest
{
    private readonly Fixture _fixture;
    private readonly ISpotifyClient _spotifyClientFake;
    private readonly ISpotifyCacheService _spotifyCacheServiceFake;
    private readonly SpotifyService _spotifyService;

    public SpotifyServiceTest()
    {
        _fixture = new Fixture();
        _spotifyClientFake = A.Fake<ISpotifyClient>();
        _spotifyCacheServiceFake = A.Fake<ISpotifyCacheService>();

        _spotifyService = new SpotifyService(_spotifyClientFake, _spotifyCacheServiceFake);
    }

    [Fact]
    public async Task GetAvailableGenresAsync_WhenCachedGenresExist_DoNotCallSpotifyApi()
    {
        // Arrange
        A.CallTo(() => _spotifyCacheServiceFake.GetAvailableGenreSeedFromCacheAsync(A<CancellationToken>.Ignored))
            .Returns(A.Fake<AvailableGenreSeed>());

        // Act
        var result = await _spotifyService.GetAvailableGenresAsync(CancellationToken.None);

        // Assert
        A.CallTo(() => _spotifyCacheServiceFake.GetAvailableGenreSeedFromCacheAsync(A<CancellationToken>.Ignored))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _spotifyClientFake.GetAvailableGenresAsync(A<CancellationToken>.Ignored))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task GetRecommendationsAsync_WhenSeedCountIsGreaterThanMaximumNumberOfSeeds_ThenCallApiWithMaximumNumberOfSeeds()
    {
        // Arrange
        var maxSeedCount = 5;
        var seed = _fixture.CreateMany<Seed>(maxSeedCount * 10);

        // Act
        var result = await _spotifyService.GetRecommendationsAsync(seed, CancellationToken.None);

        // Assert
        A.CallTo(() => _spotifyClientFake.GetRecommendationsAsync(
            A<IEnumerable<Seed>>.That.Matches(s => s.Count() == maxSeedCount),
            A<CancellationToken>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicSuggestion.Abstractions.Models;
using MusicSuggestion.Models;
using MusicSuggestion.SpotifyApi.Interfaces;
using System.Diagnostics;

namespace MusicSuggestion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISpotifyService _spotifyService;

        public HomeController(ILogger<HomeController> logger, ISpotifyService spotifyService)
        {
            _logger = logger;
            _spotifyService = spotifyService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var response = await _spotifyService.GetAvailableGenresAsync(CancellationToken.None);

            ViewBag.GenresList = new MultiSelectList(response.Genres);

            return View();
        }

        [HttpPost]
        public JsonResult SearchArtists(SearchQuery searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery.Query))
            {
                return Json(null);
            }

            var artists = _spotifyService.GetArtistsAsync(searchQuery.Query, CancellationToken.None).Result;
            return Json(artists);
        }

        [HttpPost]
        public JsonResult SearchTracks(SearchQuery searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery.Query))
            {
                return Json(null);
            }

            var tracks = _spotifyService.GetTracksAsync(searchQuery.Query, CancellationToken.None).Result;
            return Json(tracks);
        }

        public async Task<IActionResult> RecommendationsAsync(IEnumerable<string> genres, IEnumerable<string> artists, IEnumerable<string> tracks)
        {
            var seed = new List<Seed>();
            seed.AddRange(genres.Select(g => new Seed { Type = Seed.GENRE_KEY, Value = g }));
            seed.AddRange(artists.Select(a => new Seed { Type = Seed.ARTIST_KEY, Value = a }));
            seed.AddRange(tracks.Select(t => new Seed { Type = Seed.TRACK_KEY, Value = t }));

            if (seed.Count > 0)
            {
                var recommendations = await _spotifyService.GetRecommendationsAsync(seed, CancellationToken.None);
                ViewBag.RecomendedTracks = recommendations.Tracks;

                return View();
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
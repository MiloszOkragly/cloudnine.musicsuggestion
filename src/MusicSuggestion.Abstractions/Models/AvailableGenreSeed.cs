using Newtonsoft.Json;

namespace MusicSuggestion.Abstractions.Models;

public class AvailableGenreSeed
{
    [JsonProperty("genres")]
    public IEnumerable<string> Genres { get; set; } = new List<string>();
}

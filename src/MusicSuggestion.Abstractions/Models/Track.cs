using MusicSuggestion.Abstractions.Models.Shared;

namespace MusicSuggestion.Abstractions.Models;

public class Track : BaseIdentifiableModel
{
    public Album? Album { get; set; }

    public List<Artist> Artists { get; set; } = new List<Artist>();
}

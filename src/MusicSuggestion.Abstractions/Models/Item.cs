using MusicSuggestion.Abstractions.Models.Shared;

namespace MusicSuggestion.Abstractions.Models;

public class Item : BaseIdentifiableModel
{
    public Followers? Followers { get; set; }

    public List<string> Genres { get; set; } = new List<string>();

    public Album? Album { get; set; }

    public List<Artist> Artists { get; set; } = new List<Artist>();

    public List<Image> Images { get; set; } = new List<Image>();
}

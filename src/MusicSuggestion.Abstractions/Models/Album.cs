using MusicSuggestion.Abstractions.Models.Shared;

namespace MusicSuggestion.Abstractions.Models;

public class Album : BaseIdentifiableModel
{
    public List<Image> Images { get; set; } = new List<Image>();
}

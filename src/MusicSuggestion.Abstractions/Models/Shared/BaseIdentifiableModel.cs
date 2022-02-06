namespace MusicSuggestion.Abstractions.Models.Shared;

public class BaseIdentifiableModel
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}

namespace MusicSuggestion.Abstractions.Models;

public class SearchResult
{
    public Artists Artists { get; set; } = new Artists();

    public Tracks Tracks { get; set; } = new Tracks();
}


namespace MusicSuggestion.Abstractions.Models;

public class Seed
{
    public const string GENRE_KEY = "genre";
    public const string ARTIST_KEY = "artist";
    public const string TRACK_KEY = "track";

    public string Type { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}

using System.Text.Json;

namespace MusicSuggestion.SpotifyApi.Extentions;

public static class ObjectExtensions
{
    public static byte[]? ToByteArray(this object obj)
    {
        if (obj == null)
        {
            return null;
        }

        using var memoryStream = new MemoryStream();
        JsonSerializer.Serialize(memoryStream, obj);
        return memoryStream.ToArray();
    }
    public static T? FromByteArray<T>(this byte[] byteArray) where T : class
    {
        if (byteArray == null)
        {
            return default;
        }

        using var memoryStream = new MemoryStream(byteArray);
        return JsonSerializer.Deserialize<T>(memoryStream);
    }
}

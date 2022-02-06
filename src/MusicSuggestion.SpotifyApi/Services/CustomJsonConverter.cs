using Newtonsoft.Json;

namespace MusicSuggestion.SpotifyApi.Services;

public class CustomJsonConverter
{
    public static T DeserializeObjectOrThrow<T>(string json)
    {
        var deserializedJson = JsonConvert.DeserializeObject<T>(json);

        if (deserializedJson == null)
        {
            throw new ArgumentNullException(json);
        }

        return deserializedJson;
    }
}

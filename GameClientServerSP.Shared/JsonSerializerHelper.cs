using Newtonsoft.Json;
using System.Text;

namespace GameClientServerSP.Shared;

public static class JsonSerializerHelper
{
    public static byte[] Serialize<T>(T entity)
    {
        var message = JsonConvert.SerializeObject(entity, GetSettings());
        var bytes = Encoding.UTF8.GetBytes(message);

        return bytes;
    }

    public static T? Deserialize<T>(byte[] message)
    {
        var strMessage = Encoding.UTF8.GetString(message);
        return JsonConvert.DeserializeObject<T>(strMessage, GetSettings());
    }

    private static JsonSerializerSettings GetSettings()
    {
        return new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All // This includes type information in the JSON string
        };
    }
}

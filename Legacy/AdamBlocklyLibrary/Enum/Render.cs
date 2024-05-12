using System.Text.Json.Serialization;

namespace AdamBlocklyLibrary.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Render
    {
        geras,
        thrasos,
        zelos,
        minimalist,
        measurables
    }
}

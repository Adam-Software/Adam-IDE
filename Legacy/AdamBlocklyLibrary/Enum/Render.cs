using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdamBlocklyLibrary.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Render
    {
        geras,
        thrasos,
        zelos,
        minimalist,
        measurables
    }
}

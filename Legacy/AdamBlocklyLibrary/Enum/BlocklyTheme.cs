using System.Text.Json.Serialization;

namespace AdamBlocklyLibrary.Enum
{
    //[JsonConverter(typeof(StringEnumConverter))]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BlocklyTheme
    {
        Classic,
        Dark,
        Deuteranopia,
        Highcontrast, 
        Modern,
        Tritanopia,
        Zelos
    }
}

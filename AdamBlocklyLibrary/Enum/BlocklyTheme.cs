using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdamBlocklyLibrary.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdamBlocklyLibrary.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CategoryStyle
    {
        colour_category,
        list_category,
        logic_category,
        loop_category,
        math_category,
        procedure_category,
        text_category,
        variable_category,
        variable_dynamic_category
    }
}

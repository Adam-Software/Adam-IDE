using System.Text.Json.Serialization;

namespace AdamBlocklyLibrary.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
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

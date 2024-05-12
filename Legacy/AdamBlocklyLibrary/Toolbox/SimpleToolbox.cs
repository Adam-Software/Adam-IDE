using AdamBlocklyLibrary.Enum;
using System.Text.Json.Serialization;

namespace AdamBlocklyLibrary.Toolbox
{
    public class SimpleToolbox
    {
        [JsonPropertyName("kind")]
        public string Kind { get; } = "block";

        [JsonIgnore]
        public BlockType Type { get; set; }

        [JsonPropertyName("type")]
        public string TypeString => Type.ToString();
    }
}

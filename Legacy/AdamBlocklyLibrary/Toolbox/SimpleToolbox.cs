using AdamBlocklyLibrary.Enum;
using Newtonsoft.Json;

namespace AdamBlocklyLibrary.Toolbox
{
    public class SimpleToolbox
    {
        [JsonProperty("kind")]
        public string Kind { get; } = "block";

        [JsonIgnore]
        public BlockType Type { get; set; }

        [JsonProperty("type")]
        public string TypeString => Type.ToString();
    }
}

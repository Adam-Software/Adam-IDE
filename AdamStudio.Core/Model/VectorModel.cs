using System.Text.Json.Serialization;

namespace AdamStudio.Core.Model
{

    public class VectorModel
    {
        [JsonPropertyName("move")]
        public VectorItem Move { get; set; }
    }

    public class VectorItem
    {
        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        [JsonPropertyName("z")]
        public float Z { get; set; }

    }
}

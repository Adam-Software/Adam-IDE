using Newtonsoft.Json;

namespace AdamController.Model
{

    public class VectorModel
    {
        [JsonProperty("move")]
        public VectorItem Move { get; set; }
    }

    public class VectorItem
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }

        [JsonProperty("z")]
        public float Z { get; set; }

    }
}

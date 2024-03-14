using Newtonsoft.Json;

namespace AdamController.Core.Model
{
    public class WebMessageJsonReceived
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}

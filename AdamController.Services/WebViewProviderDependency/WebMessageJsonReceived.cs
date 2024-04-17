using Newtonsoft.Json;
using System;

namespace AdamController.Services.WebViewProviderDependency
{
    public class WebMessageJsonReceived : EventArgs
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}

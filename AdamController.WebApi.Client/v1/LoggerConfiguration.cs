namespace AdamController.WebApi.Client.v1
{
    internal static class LoggerConfiguration
    {
        private const string cApiPath = "LoggerConfiguration";

        internal static async Task<bool> DisableUdpSyslog()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/DisableUdpSyslog");

            bool responseValue = await responseMessage.Content.ReadAsAsync<bool>();
            return responseValue;

        }

        internal static async Task<bool> EnableUdpSyslog(string ip)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/EnableUdpSyslog/{ip}");

            bool responseValue = await responseMessage.Content.ReadAsAsync<bool>();
            return responseValue;
        }
    }
}

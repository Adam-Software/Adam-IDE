using AdamController.WebApi.Client.v1.RequestModel;

namespace AdamController.WebApi.Client.v1
{
    internal class ComunicationManagment
    {
        private const string cApiPath = "ComunicationManagment";

        internal static async Task<ComunicationStatus> GetServersStatus()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetServersStatus");

            ComunicationStatus responseValue = await responseMessage.Content.ReadAsAsync<ComunicationStatus>();
            return responseValue;
        }

        internal static async Task<string> GetServerStatus(string serverName)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetServerStatus/{serverName}");

            string responseValue = await responseMessage.Content.ReadAsAsync<string>();
            return responseValue;
        }

        internal static async Task<bool> SendCommand(ServerCommand command, string serverName)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/SendCommand/{serverName}/{command}");

            bool responseValue = await responseMessage.Content.ReadAsAsync<bool>();
            return responseValue;
        }

    }
}

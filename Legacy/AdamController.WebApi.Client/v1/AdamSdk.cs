using AdamController.WebApi.Client.Common;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.WebApi.Client.v1
{
    internal class AdamSdk
    {
        private const string cApiPath = "AdamSdk";

        internal static async Task<ExtendedCommandExecuteResult> MoveToZeroPosition()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/MoveToZeroPosition");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }
    }
}

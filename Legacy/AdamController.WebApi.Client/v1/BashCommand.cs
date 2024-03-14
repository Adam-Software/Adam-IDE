using AdamController.WebApi.Client.Common;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.WebApi.Client.v1
{
    internal class BashCommand
    {
        private const string cApiPath = "BashCommand";

        /// <summary>
        /// Execute with waiting and return json result to http api.
        /// Command canceled after 30 second automatic.
        /// </summary>
        /// <param name="command">Bash command or programm</param>
        /// <returns>JSON object ExtendedShellCommandExecuteResult</returns>
        internal static async Task<ExtendedCommandExecuteResult> Execute(string command)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/Execute/{command.FromStringToUrlEncodeString()}/cancelAfter?cancelAfterSeconds=-1");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Execute with waiting and return json result to http api
        /// </summary>
        /// <param name="command">Bash command or programm</param>
        /// <param name="cancelAfterSeconds">Command canceled after 30 second automatic if null</param>
        /// <returns>JSON object ExtendedShellCommandExecuteResult as http response</returns>
        internal static async Task<ExtendedCommandExecuteResult> Execute(string command, int cancelAfterSeconds)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/Execute/{command.FromStringToUrlEncodeString()}/cancelAfter?cancelAfterSeconds={cancelAfterSeconds}");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Execute without waiting. Return execute result in udp stream
        /// </summary>
        /// <param name="command">Bash command or programm</param>
        /// <returns>ExtendedShellCommandExecuteResult as http-response with report about running process
        /// and UDP stream by message client with result running process</returns>
        internal static async Task<ExtendedCommandExecuteResult> ExecuteAsync(string command)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/ExecuteAsync/{command.FromStringToUrlEncodeString()}");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Execute without waiting. With cancelation timer. Return execute result in udp stream.
        /// </summary>
        /// <param name="command">Bash command or programm</param>
        /// <param name="cancelAfterSeconds">Task canceled after this time in seconds</param>
        /// <returns>ExtendedShellCommandExecuteResult as http-response with report about running process
        /// and UDP stream by message client with result running process</returns>
        internal static async Task<ExtendedCommandExecuteResult> ExecuteAsyncWithCancelTimer(string command, int cancelAfterSeconds)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/ExecuteAsyncWithCancelTimer/{command.FromStringToUrlEncodeString()}/{cancelAfterSeconds}");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Stopped running process
        /// </summary>
        internal static async Task<ExtendedCommandExecuteResult> StopExecuteAsync()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/StopExecuteAsync");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Returned bash version
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with bash version in standart output</returns>
        internal static async Task<ExtendedCommandExecuteResult> GetVersion()
        {
            ExtendedCommandExecuteResult? responseValue = await Execute("bash --version");
            return responseValue;
        }
    }
}
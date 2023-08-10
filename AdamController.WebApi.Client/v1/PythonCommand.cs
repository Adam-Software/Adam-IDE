using AdamController.WebApi.Client.Common;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.WebApi.Client.v1
{
    internal class PythonCommand
    {
        private const string cApiPath = "PythonCommand";

        /// <summary>
        /// Execute with waiting and return json result to http api
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static async Task<ExtendedCommandExecuteResult> Execute(RequestModel.PythonCommand command)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Post($"{cApiPath}/Execute/", command);
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        ///  Execute without waiting. Return execute result in udp stream
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static async Task<ExtendedCommandExecuteResult> ExecuteAsync(RequestModel.PythonCommand command)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Post($"{cApiPath}/ExecuteAsync/", command);
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Execute without waiting. Return execute result in udp stream
        /// </summary>
        /// <param name="command">Python command or programm</param>
        /// <returns>ExtendedShellCommandExecuteResult as http-response with report about running process
        /// and UDP stream by message client with result running process</returns>
        internal static async Task<ExtendedCommandExecuteResult> ExecuteAsync(string command)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Put($"{cApiPath}/ExecuteAsync/{command.FromStringToUrlEncodeString()}");
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
        /// Returned python version
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        internal static async Task<ExtendedCommandExecuteResult> GetVersion()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetVersion");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Returned python work dir
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        internal static async Task<ExtendedCommandExecuteResult> GetPythonWorkDir()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetPythonWorkDir");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Returned python bin dir
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        internal static async Task<ExtendedCommandExecuteResult> GetPythonBinDir()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetPythonBinDir");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }
    }
}

using AdamController.WebApi.Client.Common;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.WebApi.Client.v1
{
    #region Interface

    public interface IPythonCommand
    {
        public Task<ExtendedCommandExecuteResult> ExecuteAsync(RequestModel.PythonCommandModel command);
        public Task<ExtendedCommandExecuteResult> StopExecuteAsync();
        public Task<ExtendedCommandExecuteResult> GetVersion();
        public Task<ExtendedCommandExecuteResult> GetPythonWorkDir();
        public Task<ExtendedCommandExecuteResult> GetPythonBinDir();
    }

    #endregion

    internal class PythonCommand : IPythonCommand
    {
        #region Const

        private const string cApiPath = "PythonCommand";

        #endregion

        #region Var

        private readonly ApiClient mApiClient;

        #endregion

        #region ~

        internal PythonCommand(ApiClient apiClient) 
        {
            mApiClient = apiClient;
        }

        #endregion

        #region Public methods

        /// <summary>
        ///  Execute without waiting. Return execute result in udp stream
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task<ExtendedCommandExecuteResult> ExecuteAsync(RequestModel.PythonCommandModel command)
        {
            var responseMessage =  mApiClient.Post($"{cApiPath}/ExecuteAsync/", command);
            var result =  responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        /// <summary>
        /// Stopped running process
        /// </summary>
        public Task<ExtendedCommandExecuteResult> StopExecuteAsync()
        {
            var responseMessage =  mApiClient.Put($"{cApiPath}/StopExecuteAsync");
            var result =  responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        /// <summary>
        /// Returned python version
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        public Task<ExtendedCommandExecuteResult> GetVersion()
        {
            var responseMessage =  mApiClient.Get($"{cApiPath}/GetVersion");
            var result =  responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        /// <summary>
        /// Returned python work dir
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        public Task<ExtendedCommandExecuteResult> GetPythonWorkDir()
        {
            var responseMessage =  mApiClient.Get($"{cApiPath}/GetPythonWorkDir");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        /// <summary>
        /// Returned python bin dir
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        public Task<ExtendedCommandExecuteResult> GetPythonBinDir()
        {
            var responseMessage =  mApiClient.Get($"{cApiPath}/GetPythonBinDir");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        #endregion
    }
}

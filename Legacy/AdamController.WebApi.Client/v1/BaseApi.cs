using AdamController.WebApi.Client.v1.RequestModel;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.WebApi.Client.v1
{
    public static class BaseApi
    {
        #region Api Client Settings (is not api call)

        public static void SetApiClientUri(Uri uri) => ApiClient.SetUri(uri);
        public static void SetApiClientUri(string uri) => ApiClient.SetUri(uri);
        public static void SetApiClientUri(string ip, int port) => ApiClient.SetUri(ip, port);
        public static void SetAuthenticationHeader(string login, string password) => ApiClient.SetAuthenticationHeaderValue(login, password);

        #endregion


        #region AdamSdk

        public static async Task<ExtendedCommandExecuteResult> MoveToZeroPosition() => await AdamSdk.MoveToZeroPosition();

        #endregion

        #region BashCommand

        /// <summary>
        /// Execute with waiting and return json result to http api.
        /// Command canceled after 30 second automatic.
        /// </summary>
        /// <param name="command">Bash command or programm</param>
        /// <returns>JSON object ExtendedShellCommandExecuteResult</returns>
        public static async Task<ExtendedCommandExecuteResult> BashExecute(string command) => await BashCommand.Execute(command);

        /// <summary>
        /// Execute with waiting and return json result to http api
        /// </summary>
        /// <param name="command">Bash command or programm</param>
        /// <param name="cancelAfterSeconds">Command canceled after 30 second automatic if null</param>
        /// <returns>JSON object ExtendedShellCommandExecuteResult as http response</returns>
        public static async Task<ExtendedCommandExecuteResult> BashExecute(string command, int cancelAfterSeconds) => await BashCommand.Execute(command, cancelAfterSeconds);

        /// <summary>
        /// Execute without waiting. Return execute result in udp stream
        /// </summary>
        /// <param name="command">Python command or programm</param>
        /// <returns>ExtendedShellCommandExecuteResult as http-response with report about running process
        /// and UDP stream by message client with result running process</returns>
        public static async Task<ExtendedCommandExecuteResult> BashExecuteAsync(string command) => await BashCommand.ExecuteAsync(command);

        /// <summary>
        /// Execute without waiting. With cancelation timer. Return execute result in udp stream.
        /// </summary>
        /// <param name="command">Bash command or programm</param>
        /// <param name="cancelAfterSeconds">Task canceled after this time in seconds</param>
        /// <returns>ExtendedShellCommandExecuteResult as http-response with report about running process
        /// and UDP stream by message client with result running process</returns>
        public static async Task<ExtendedCommandExecuteResult> BashExecuteAsyncWithCancelTimer(string command, int cancelAfterSeconds) => await BashCommand.ExecuteAsyncWithCancelTimer(command, cancelAfterSeconds);

        /// <summary>
        /// Stopped running process
        /// </summary>
        public static async Task<ExtendedCommandExecuteResult> BashPythonExecute() => await BashCommand.StopExecuteAsync();

        /// <summary>
        /// Returned bash version
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with bash version in standart output</returns>
        public static async Task<ExtendedCommandExecuteResult> GetBashVersion() => await BashCommand.GetVersion();

        #endregion

        #region ComunicationManagment

        public static Task<ComunicationStatus> GetServersStatus() => ComunicationManagment.GetServersStatus();
        public static Task<string> GetServerStatus(string serverName) => ComunicationManagment.GetServerStatus(serverName);
        public static Task<bool> SendCommand(ServerCommand command, string serverName) => ComunicationManagment.SendCommand(command, serverName);

        #endregion

        #region FileManager

        /// <summary>
        /// Listing dirrectory
        /// </summary>
        public static Task<ExtendedCommandExecuteResult> GetFileList() => FileManager.GetFileList();

        /// <summary>
        /// Listing dirrectory
        /// </summary>
        /// <param name="path">Exiting dirrectory path</param>
        public static Task<ExtendedCommandExecuteResult> GetFileList(string path) => FileManager.GetFileList(path);

        /// <summary>
        /// Extended listing dirrectory
        /// </summary>
        public static Task<ExtendedCommandExecuteResult> GetExtendedFileList() => FileManager.GetExtendedFileList();

        /// <summary>
        /// Extended listing dirrectory
        /// </summary>
        /// <param name="path">Exiting dirrectory path</param>
        public static Task<ExtendedCommandExecuteResult> GetExtendedFileList(string path) => FileManager.GetExtendedFileList(path);

        /// <summary>
        /// Cat file
        /// </summary>
        /// <param name="path">Exiting file path</param>
        public static Task<ExtendedCommandExecuteResult> GetFileContent(string path) => FileManager.GetFileContent(path);

        /// <summary>
        /// Calculate the checksum for file
        /// </summary>
        /// <param name="path">Exiting file path</param>
        public static Task<ExtendedCommandExecuteResult> GetCheckSum(string path) => FileManager.GetCheckSum(path);

        /// <summary>
        /// Calculate the SHA-1 checksum for file
        /// </summary>
        /// <param name="path">Exiting file path</param>
        public static Task<ExtendedCommandExecuteResult> GetSha1Sum(string path) => FileManager.GetSha1Sum(path);

        /// <summary>
        /// Calculate the SHA-256 checksum for file
        /// </summary>
        /// <param name="path">Exiting file path</param>
        public static Task<ExtendedCommandExecuteResult> GetSha256Sum(string path) => FileManager.GetSha256Sum(path);

        /// <summary>
        /// Simple file uploaded
        /// </summary>
        /// <returns>sha1sum in standart output field is success upload</returns>
        public static Task<ExtendedCommandExecuteResult> UploadFile(string filePath, string name, string fileName) => FileManager.UploadFile(filePath,  name, fileName);

        /// <summary>
        /// Simple file uploaded
        /// </summary>
        /// <returns>sha1sum in standart output field is success upload</returns>
        //public static Task<ExtendedCommandExecuteResult> UploadFile(string file, string name, string fileName) => FileManager.UploadFile(file, name, fileName);


        #endregion

        #region LoggerConfiguration

        public static Task<bool> DisableUdpSyslog() => LoggerConfiguration.DisableUdpSyslog();
        public static Task<bool> EnableUdpSyslog(string ip) => LoggerConfiguration.EnableUdpSyslog(ip);

        #endregion

        #region PythonCommand

        /// <summary>
        /// Execute with waiting and return json result to http api.
        /// Command canceled after 30 second automatic.
        /// </summary>
        /// <param name="command">Python command or programm</param>
        /// <returns>JSON object ExtendedShellCommandExecuteResult</returns>
        public static async Task<ExtendedCommandExecuteResult> PythonExecute(RequestModel.PythonCommand command) => await PythonCommand.Execute(command);

        /// <summary>
        /// Execute with waiting and return json result to http api
        /// </summary>
        /// <param name="command">Python command or programm</param>
        /// <param name="cancelAfterSeconds">Command canceled after 30 second automatic if null</param>
        /// <returns>JSON object ExtendedShellCommandExecuteResult as http response</returns>
        //public static async Task<ExtendedCommandExecuteResult> PythonExecute(string command, int cancelAfterSeconds) => await PythonCommand.Execute(command, cancelAfterSeconds);

        /// <summary>
        /// Execute without waiting. Return execute result in udp stream
        /// </summary>
        /// <param name="command">Python command or programm in JSON structure</param>
        /// <returns>ExtendedShellCommandExecuteResult as http-response with report about running process
        /// and UDP stream by message client with result running process</returns>
        public static async Task<ExtendedCommandExecuteResult> PythonExecuteAsync(RequestModel.PythonCommand command) => await PythonCommand.ExecuteAsync(command);

        /// <summary>
        /// Execute without waiting. Return execute result in udp stream
        /// </summary>
        /// <param name="command">Python command or programm</param>
        /// <returns>ExtendedShellCommandExecuteResult as http-response with report about running process
        /// and UDP stream by message client with result running process</returns>
        public static async Task<ExtendedCommandExecuteResult> PythonExecuteAsync(string command) => await PythonCommand.ExecuteAsync(command);

        /// <summary>
        /// Stopped running process
        /// </summary>
        public static async Task<ExtendedCommandExecuteResult> StopPythonExecute() => await PythonCommand.StopExecuteAsync();

        /// <summary>
        /// Returned python version
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        public static async Task<ExtendedCommandExecuteResult> GetPythonVersion() => await PythonCommand.GetVersion();

        /// <summary>
        /// Returned python bin dirrectory
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        public static async Task<ExtendedCommandExecuteResult> GetPythonBinDir() => await PythonCommand.GetPythonBinDir();

        /// <summary>
        /// Returned python work dirrectory
        /// </summary>
        /// <returns>ExtendedShellCommandExecuteResult with python version in standart output</returns>
        public static async Task<ExtendedCommandExecuteResult> GetPythonWorkDir() => await PythonCommand.GetPythonWorkDir();

        #endregion
    }
}

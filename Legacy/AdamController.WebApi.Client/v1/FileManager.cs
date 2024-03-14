using AdamController.WebApi.Client.Common;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.WebApi.Client.v1
{
    internal class FileManager
    {
        private const string cApiPath = "FileManager";

        #region Listing dirrectory

        /// <summary>
        /// Listing dirrectory
        /// </summary>
        internal static async Task<ExtendedCommandExecuteResult> GetFileList()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetFileList");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Listing dirrectory
        /// </summary>
        /// <param name="path">Exiting dirrectory path</param>
        /// <returns></returns>
        internal static async Task<ExtendedCommandExecuteResult> GetFileList(string path)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetFileList/{path.FromStringToBase64String()}");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result; 
        }

        #endregion

        #region Extended listing dirrectory

        /// <summary>
        /// Extended listing dirrectory
        /// </summary>
        internal static async Task<ExtendedCommandExecuteResult> GetExtendedFileList()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetExtendedFileList");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Extended listing dirrectory
        /// </summary>
        /// <param name="path">Exiting dirrectory path</param>
        /// <returns></returns>
        internal static async Task<ExtendedCommandExecuteResult> GetExtendedFileList(string path)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetExtendedFileList/{path.FromStringToBase64String()}");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        #endregion

        #region GetFileContent

        /// <summary>
        /// Cat file
        /// </summary>
        /// <param name="path">Exiting file path</param>
        internal static async Task<ExtendedCommandExecuteResult> GetFileContent(string path)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetFileContent/{path.FromStringToBase64String()}");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        #endregion

        #region CheckSum

        /// <summary>
        /// Calculate the checksum for file
        /// </summary>
        internal static async Task<ExtendedCommandExecuteResult> GetCheckSum(string path)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetCheckSum/{path.FromStringToBase64String()}");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Calculate the SHA-1 checksum for file
        /// </summary>
        internal static async Task<ExtendedCommandExecuteResult> GetSha1Sum(string path)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetSha1Sum/{path.FromStringToBase64String()}");

            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Calculate the SHA-256 checksum for file
        /// </summary>
        internal static async Task<ExtendedCommandExecuteResult> GetSha256Sum(string path)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetSha256Sum/{path.FromStringToBase64String()}");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        #endregion

        #region Upload File

        /// <summary>
        /// Simple file uploaded
        /// </summary>
        /// <returns>sha1sum in standart output field is success upload</returns>
        internal static async Task<ExtendedCommandExecuteResult> UploadFile(string filePath, string name, string fileName)
        {
            HttpResponseMessage? responseMessage = await ApiClient.Post($"{cApiPath}/UploadFile", filePath, name, fileName);
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        /// <summary>
        /// Simple file uploaded
        /// </summary>
        /// <returns>sha1sum in standart output field is success upload</returns>
        /*internal static async Task<ExtendedCommandExecuteResult> UploadFile(string filePath, string name, string fileName)
        {
            byte[] file;
            DateTime startTime = DateTime.Now;
            
            try
            {
                file = File.ReadAllBytes(filePath);
            }
            catch(Exception ex)
            {
                DateTime endTime = DateTime.Now;

                return new ExtendedCommandExecuteResult()
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    RunTime = endTime - startTime,

                    StandardOutput = "",
                    StandardError = ex.Message,

                    ExitCode = -1,
                    Succeesed = false
                };
            }
            
            HttpResponseMessage? responseMessage = await ApiClient.Post($"{cApiPath}/UploadFile", file, name, fileName);
            ExtendedCommandExecuteResult responseValue = await responseMessage?.Content.ReadAsAsync<ExtendedCommandExecuteResult>();

            return responseValue;
        }*/

        #endregion
    }
}
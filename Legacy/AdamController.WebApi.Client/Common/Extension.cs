using AdamController.WebApi.Client.v1.ResponseModel;
using System.Text;
using System.Text.Json;
using System.Web;

namespace AdamController.WebApi.Client.Common
{
    public static class Extension
    {
        #region public extensions

        [Obsolete]
        /// <summary>
        /// Deserealize jsonString to CommandExecuteResult
        /// </summary>
        /// <param name="jsonString">JSON string with CommandExecuteResult object</param>
        /// <returns>Returns the CommandExecuteResult object with the result, if deserialization is successful, or a new CommandExecuteResult object otherwise</returns>
        public static CommandExecuteResult ToCommandResult(this string jsonString)
        {
            if (jsonString == null)
                return new CommandExecuteResult();

            CommandExecuteResult executeResult = new();

            try
            {
                executeResult = JsonSerializer.Deserialize<CommandExecuteResult>(jsonString);
            }
            catch
            {

            }

            return executeResult;
        }

        /// <summary>
        /// Deserealize jsonString to ExtendedCommandExecuteResult
        /// </summary>
        /// <param name="jsonString">JSON string with ExtendedCommandExecuteResult object</param>
        /// <returns>Returns the ExtendedCommandExecuteResult object with the result, if deserialization is successful, or a new ExtendedCommandExecuteResult object otherwise</returns>
        public static ExtendedCommandExecuteResult ToExtendedCommandResult(this string jsonString)
        {
            if (jsonString == null)
                return new ExtendedCommandExecuteResult();

            ExtendedCommandExecuteResult executeResult = new();

            try
            {
                
                 executeResult = JsonSerializer.Deserialize<ExtendedCommandExecuteResult>(jsonString);
                //executeResult = JsonConvert.DeserializeObject<ExtendedCommandExecuteResult>(jsonString);
            }
            catch
            {

            }

            return executeResult;
        }

        #endregion

        #region internal extension

        internal static string FromBase64ToString(this string base64string)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64string);
            string decodedString = Encoding.UTF8.GetString(base64EncodedBytes);

            return decodedString;
        }

        internal static string FromStringToBase64String(this string @string)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(@string);
            string encodedToBase64String = Convert.ToBase64String(plainTextBytes);

            return encodedToBase64String;
        }

        internal static string FromStringToUrlEncodeString(this string @string)
        {
            string encodedToUrlEncodeString = HttpUtility.UrlEncode(@string);
            return encodedToUrlEncodeString;
        }

        internal static string FromUrlEncodeToString(this string @string)
        {
            string encodedToUrlEncodeString = HttpUtility.UrlDecode(@string);
            return encodedToUrlEncodeString;
        }

        internal async static Task<ExtendedCommandExecuteResult> ToExtendedCommandResult(this HttpResponseMessage? response)
        {
            if(response == null)
                return new ExtendedCommandExecuteResult();

            var jsonString = await response.Content.ReadAsStringAsync();
            ExtendedCommandExecuteResult result = JsonSerializer.Deserialize<ExtendedCommandExecuteResult>(jsonString);
            return result;
        }

        internal async static Task<CommandExecuteResult> ToCommandResult(this HttpResponseMessage? response)
        {
            if(response == null)
                return new CommandExecuteResult();

            var jsonString = await response.Content.ReadAsStringAsync();
            CommandExecuteResult result = JsonSerializer.Deserialize<CommandExecuteResult>(jsonString);
            return result;
        }

        #endregion
    }
}

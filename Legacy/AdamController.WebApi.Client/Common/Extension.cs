using AdamController.WebApi.Client.v1.ResponseModel;
using System.Text;
using System.Text.Json;
using System.Web;

namespace AdamController.WebApi.Client.Common
{
    public static class Extension
    {
        public async static Task<ExtendedCommandExecuteResult> ToExtendedCommandResult(this HttpResponseMessage? response)
        {
            if (response == null)
                return new ExtendedCommandExecuteResult();

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = jsonString.ToExtendedCommandResult();
            return result;
        }

        public async static Task<CommandExecuteResult> ToCommandResult(this HttpResponseMessage? response)
        {
            if (response == null)
                return new CommandExecuteResult();

            string jsonString = await response.Content.ReadAsStringAsync();

            var result = jsonString.ToCommandResult();
            return result;
        }

        public static CommandExecuteResult ToCommandResult(this string jsonString)
        {
            CommandExecuteResult result = new();

            try
            {
                CommandExecuteResult? deleserialize = JsonSerializer.Deserialize<CommandExecuteResult>(jsonString);

                if (deleserialize != null)
                    result = deleserialize;

                return result;
            }
            catch
            {
                return result;
            }
        }

        public static ExtendedCommandExecuteResult ToExtendedCommandResult(this string jsonString)
        {
            ExtendedCommandExecuteResult result = new();

            try
            {
                ExtendedCommandExecuteResult? deleserialize = JsonSerializer.Deserialize<ExtendedCommandExecuteResult>(jsonString);

                if (deleserialize != null)
                    result = deleserialize;

                return result;
            }
            catch
            {
                return result;
            }
        }

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

        #endregion
    }
}

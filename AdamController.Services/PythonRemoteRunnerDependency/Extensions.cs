using System.Text.Json;

namespace AdamController.Services.PythonRemoteRunnerDependency
{
    public static class Extensions
    {
        /// <summary>
        /// Deserealize jsonString to CommandExecuteResult
        /// </summary>
        /// <param name="jsonString">JSON string with CommandExecuteResult object</param>
        /// <returns>Returns the CommandExecuteResult object with the result, if deserialization is successful, or a new CommandExecuteResult object otherwise</returns>
        public static RemoteCommandExecuteResult ToCommandResult(this string jsonString)
        {
            if (jsonString == null)
                return new RemoteCommandExecuteResult();

            RemoteCommandExecuteResult executeResult = new();

            try
            {
                executeResult = JsonSerializer.Deserialize<RemoteCommandExecuteResult>(jsonString);
            }
            catch
            {

            }

            return executeResult;
        }
    }
}

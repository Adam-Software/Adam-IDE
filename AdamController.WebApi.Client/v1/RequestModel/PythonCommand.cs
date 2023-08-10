
namespace AdamController.WebApi.Client.v1.RequestModel
{
    public class PythonCommand
    {
        /// <summary>
        /// Python command or programm
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// Path by python interpriter
        /// </summary>
        public string PythonPath { get; set; } = string.Empty;
    }
}

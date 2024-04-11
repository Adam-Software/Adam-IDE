using AdamController.WebApi.Client.Common;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.Core.Helpers
{
    public class PythonScriptExecuteHelper
    {
        #region Declaration delegates and events

        public delegate void OnStandartOutput(string message);
        public static event OnStandartOutput OnStandartOutputEvent;

        public delegate void OnExecuteStart(string message);
        public static event OnExecuteStart OnExecuteStartEvent;

        public delegate void OnExecuteFinish(string message);
        public static event OnExecuteFinish OnExecuteFinishEvent;

        #endregion

        static PythonScriptExecuteHelper()
        {
            ComunicateHelper.OnAdamMessageReceivedEvent += OnAdamMessageReceivedAsync;
        }

        private static void OnAdamMessageReceivedAsync(string message)
        {
            switch (message)
            {
                case "start":
                    OnExecuteStartEvent.Invoke(string.Empty);
                    break;

                case "error":
                    break;

                case string result when result.StartsWith("finish"):
                    OnExecuteFinishEvent.Invoke(FinishExecuteMessage(message.Remove(0, 6)));
                    break;

                default:
                    OnStandartOutputEvent.Invoke($"{message}\n");
                    break;
            }
        }

        private static string FinishExecuteMessage(string resultJson = null)
        {
            string message = "\n======================\n<<Выполнение программы завершено>>";

            if(string.IsNullOrEmpty(resultJson))
                return message;

            CommandExecuteResult executeResult = resultJson.ToCommandResult();
           
            string messageWithResult = $"{message}\n" +
                $"\n" +
                $"Отчет о выполнении\n" +
                $"======================\n" +
                $"Начало выполнения: {executeResult.StartTime}\n" +
                $"Завершение выполнения: {executeResult.EndTime}\n" +
                $"Общее время выполнения: {executeResult.RunTime}\n" +
                $"Код выхода: {executeResult.ExitCode}\n" +
                $"Статус успешности завершения: {executeResult.Succeeded}" +
                $"\n======================\n";

            if (!string.IsNullOrEmpty(executeResult.StandardError))
                messageWithResult += $"Ошибка: {executeResult.StandardError}" +
                    $"\n======================\n";

            return messageWithResult;

        }


    }
}

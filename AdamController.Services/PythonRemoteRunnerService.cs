using AdamController.Services.Interfaces;

namespace AdamController.Services
{
    public class PythonRemoteRunnerService : IPythonRemoteRunnerService
    {
        #region Events

        public event PythonStandartOutput RaisePythonStandartOutput;
        public event PythonScriptExecuteStart RaisePythonScriptExecuteStart;
        public event PythonScriptExecuteFinish RaisePythonScriptExecuteFinish;

        #endregion

        #region Services

        private readonly ICommunicationProviderService mCommunicationProvider;

        #endregion

        #region Const

        private const string cStartMessage = "start";
        private const string cErrorMessage = "error";
        private const string cFinishMessage = "finish";

        #endregion

        public PythonRemoteRunnerService(ICommunicationProviderService communicationProvider) 
        {
            mCommunicationProvider = communicationProvider;
        }

        #region Public method

        public void Dispose()
        {
            Unsubscribe();
        }

        #endregion

        #region Private methods

        private void MessageParser(string message)
        {
            switch (message)
            {
                case cStartMessage:
                    //OnExecuteStartEvent.Invoke(string.Empty);
                    OnRaisePythonScriptExecuteStart();
                    break;

                case cErrorMessage:
                    break;

                case string result when result.StartsWith(cFinishMessage):
                    //OnExecuteFinishEvent.Invoke(FinishExecuteMessage(message.Remove(0, 6)));
                    break;

                default:
                    var messagen = $"{message}";
                    OnRaisePythonStandartOutput(messagen);
                    //OnStandartOutputEvent.Invoke($"{message}\n");
                    break;
            }
        }

        private static string FinishExecuteMessage(string resultJson = null)
        {
            string message = "\n======================\n<<Выполнение программы завершено>>";

            if (string.IsNullOrEmpty(resultJson))
                return message;

            //CommandExecuteResult executeResult = resultJson.ToCommandResult();

            /*string messageWithResult = $"{message}\n" +
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
                    $"\n======================\n";*/

            return null; //messageWithResult;

        }
        #endregion

        #region Subscribses

        private void Subscribe()
        {
            mCommunicationProvider.RaiseUdpClientReceived += RaiseUdpClientReceived;
        }


        private void Unsubscribe() 
        {
            mCommunicationProvider.RaiseUdpClientReceived -= RaiseUdpClientReceived;
        }


        #endregion

        #region Event methods

        private void RaiseUdpClientReceived(object sender, string message)
        {
            MessageParser(message);
        }

        #endregion

        #region OnRaise events

        protected virtual void OnRaisePythonStandartOutput(string message)
        {
            PythonStandartOutput raiseEvent = RaisePythonStandartOutput;
            raiseEvent?.Invoke(this, message);  
        }

        protected virtual void OnRaisePythonScriptExecuteStart(string message = "")
        {
            PythonScriptExecuteStart raiseEvent = RaisePythonScriptExecuteStart;
            raiseEvent?.Invoke(this, message);
        }

        protected virtual void OnRaisePythonScriptExecuteFinish(string message)
        {
            PythonScriptExecuteFinish raiseEvent = RaisePythonScriptExecuteFinish;
            raiseEvent?.Invoke(this, message);
        }

        #endregion
    }
}

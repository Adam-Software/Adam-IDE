using AdamController.Services.Interfaces;
using AdamController.Services.PythonRemoteRunnerDependency;

namespace AdamController.Services
{
    public class PythonRemoteRunnerService : IPythonRemoteRunnerService
    {
        #region Events

        public event PythonStandartOutputEventHandler RaisePythonStandartOutputEvent;
        public event PythonScriptExecuteStartEventHandler RaisePythonScriptExecuteStartEvent;
        public event PythonScriptExecuteFinishEventHandler RaisePythonScriptExecuteFinishEvent;

        #endregion

        #region Services

        private readonly ICommunicationProviderService mCommunicationProvider;

        #endregion

        #region Const

        private const string cStartMessage = "start";
        private const string cErrorMessage = "error";
        private const string cFinishMessage = "finish";

        #endregion

        #region ~

        public PythonRemoteRunnerService(ICommunicationProviderService communicationProvider) 
        {
            mCommunicationProvider = communicationProvider;

            Subscribe();
        }

        #endregion

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
                    OnRaisePythonScriptExecuteStartEvent();
                    break;

                case cErrorMessage:
                    break;

                case string result when result.StartsWith(cFinishMessage):
                    {
                        var cleanMessage = message.Remove(0, 6);
                        var finishMessage = ParseFinishExecuteMessage(cleanMessage);
                        OnRaisePythonScriptExecuteFinishEvent(finishMessage);
                        break;
                    }
                    
                default:
                    {
                        OnRaisePythonStandartOutputEvent($"{message}\n");
                        break;
                    }
            }
        }

        private static string ParseFinishExecuteMessage(string resultJson = null)
        {
            string message = "\n======================\n<<Выполнение программы завершено>>";

            if (string.IsNullOrEmpty(resultJson))
                return message;

            RemoteCommandExecuteResult executeResult = resultJson.ToCommandResult();

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

        #endregion

        #region Subscribses

        private void Subscribe()
        {
            mCommunicationProvider.RaiseUdpServiceClientReceivedEvent += RaiseUdpClientReceived;
        }


        private void Unsubscribe() 
        {
            mCommunicationProvider.RaiseUdpServiceClientReceivedEvent -= RaiseUdpClientReceived;
        }


        #endregion

        #region Event methods

        private void RaiseUdpClientReceived(object sender, string message)
        {
            MessageParser(message);
        }

        #endregion

        #region OnRaise events

        protected virtual void OnRaisePythonStandartOutputEvent(string message)
        {
            PythonStandartOutputEventHandler raiseEvent = RaisePythonStandartOutputEvent;
            raiseEvent?.Invoke(this, message);  
        }

        protected virtual void OnRaisePythonScriptExecuteStartEvent()
        {
            PythonScriptExecuteStartEventHandler raiseEvent = RaisePythonScriptExecuteStartEvent;
            raiseEvent?.Invoke(this);
        }

        protected virtual void OnRaisePythonScriptExecuteFinishEvent(string message)
        {
            PythonScriptExecuteFinishEventHandler raiseEvent = RaisePythonScriptExecuteFinishEvent;
            raiseEvent?.Invoke(this, message);
        }

        #endregion
    }
}

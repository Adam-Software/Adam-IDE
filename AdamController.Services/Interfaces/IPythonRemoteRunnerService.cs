using AdamController.WebApi.Client.v1.ResponseModel;
using System;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void PythonStandartOutputEventHandler(object sender, string message);
    public delegate void PythonScriptExecuteStartEventHandler(object sender);
    public delegate void PythonScriptExecuteFinishEventHandler(object sender, CommandExecuteResult remoteCommandExecuteResult);

    #endregion

    public interface IPythonRemoteRunnerService : IDisposable
    {
        #region Events

        public event PythonStandartOutputEventHandler RaisePythonStandartOutputEvent;
        public event PythonScriptExecuteStartEventHandler RaisePythonScriptExecuteStartEvent;
        public event PythonScriptExecuteFinishEventHandler RaisePythonScriptExecuteFinishEvent;

        #endregion
    }
}

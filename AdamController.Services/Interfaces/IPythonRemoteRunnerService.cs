using System;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void PythonStandartOutputEventHandler(object sender, string message);
    public delegate void PythonScriptExecuteStartEventHandler(object sender);
    public delegate void PythonScriptExecuteFinishEventHandler(object sender, string message);

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

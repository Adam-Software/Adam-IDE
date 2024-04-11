using System;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void PythonStandartOutput(object sender, string message);
    public delegate void PythonScriptExecuteStart(object sender, string message);
    public delegate void PythonScriptExecuteFinish(object sender, string message);

    #endregion

    public interface IPythonRemoteRunnerService : IDisposable
    {
        #region Events

        public event PythonStandartOutput RaisePythonStandartOutput;
        public event PythonScriptExecuteStart RaisePythonScriptExecuteStart;
        public event PythonScriptExecuteFinish RaisePythonScriptExecuteFinish;

        #endregion
    }
}

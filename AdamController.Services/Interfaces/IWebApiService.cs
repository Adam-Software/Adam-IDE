using AdamController.WebApi.Client.v1.RequestModel;
using AdamController.WebApi.Client.v1.ResponseModel;
using System;
using System.Threading.Tasks;

namespace AdamStudio.Services.Interfaces
{
    public interface IWebApiService : IDisposable
    {
        public Task<ExtendedCommandExecuteResult> StopPythonExecute();
        public Task<ExtendedCommandExecuteResult> GetPythonVersion();
        public Task<ExtendedCommandExecuteResult> GetPythonBinDir();
        public Task<ExtendedCommandExecuteResult> GetPythonWorkDir();
        public Task<ExtendedCommandExecuteResult> PythonExecuteAsync(PythonCommandModel command);
        public Task<ExtendedCommandExecuteResult> MoveToZeroPosition();

    }
}

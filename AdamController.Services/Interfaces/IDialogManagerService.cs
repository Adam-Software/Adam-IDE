using MessageDialogManagerLib;
using System;

namespace AdamController.Services.Interfaces
{
    /// <summary>
    /// The old dialog call type integrated into the service
    /// </summary>
    public interface IDialogManagerService : IMessageDialogManager, IDisposable
    {}
}

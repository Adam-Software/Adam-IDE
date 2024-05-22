using AdamController.Services.SystemDialogServiceDependency;
using Prism.Services.Dialogs;
using System;

namespace AdamController.Services.Interfaces
{
    public interface ISystemDialogService : IDisposable
    {
        public OpenFileDialogResult ShowOpenFileDialog(IDialogParameters parameters);
        public SaveFileDialogResult ShowSaveFileDialog(IDialogParameters parameters);
    }
}

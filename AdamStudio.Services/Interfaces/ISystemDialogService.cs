using AdamStudio.Services.SystemDialogServiceDependency;
using Prism.Services.Dialogs;
using System;

namespace AdamStudio.Services.Interfaces
{
    public interface ISystemDialogService : IDisposable
    {
        public OpenFileDialogResult ShowOpenFileDialog(IDialogParameters parameters);
        public SaveFileDialogResult ShowSaveFileDialog(IDialogParameters parameters);
        public OpenFolderDialogResult ShowOpenFolderDialog(IDialogParameters parameters);
    }
}

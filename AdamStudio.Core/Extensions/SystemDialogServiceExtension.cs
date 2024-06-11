using Prism.Services.Dialogs;
using System.Threading.Tasks;
using System;
using AdamStudio.Services.SystemDialogServiceDependency;
using AdamStudio.Services.Interfaces;

namespace AdamStudio.Core.Extensions
{
    public static class SystemDialogServiceExtension
    {
        public static Task<OpenFileDialogResult> ShowOpenFileDialog(this ISystemDialogService dialogService, IDialogParameters parameters)
        {
            var task = new TaskCompletionSource<OpenFileDialogResult>();

            try
            {
                dialogService.ShowOpenFileDialog(parameters);
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }

            return task.Task;

        }
    }
}

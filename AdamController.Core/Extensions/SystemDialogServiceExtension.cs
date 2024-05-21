using Prism.Services.Dialogs;
using System.Threading.Tasks;
using System;
using AdamController.Services.SystemDialogServiceDependency;
using AdamController.Services.Interfaces;

namespace AdamController.Core.Extensions
{
    public static class SystemDialogServiceExtension
    {
        public static Task<OpenFileDialogResult> ShowOpenFileDialog(this ISystemDialogService dialogService, IDialogParameters parameters)
        {
            var task = new TaskCompletionSource<OpenFileDialogResult>();

            try
            {
                dialogService.ShowOpenFileDialog(parameters);
                //dialogService.ShowDialog(nameof(OpenFileDialogView), parameters, task.SetResult);
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }

            return task.Task;

        }
    }
}

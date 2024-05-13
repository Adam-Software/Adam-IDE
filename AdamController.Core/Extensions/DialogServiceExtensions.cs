using AdamController.Core.Dialog.Views;
using Prism.Services.Dialogs;

namespace AdamController.Core.Extensions
{
    public static class DialogServiceExtensions
    {
        public static void ShowOpenFileDialog(this IDialogService dialogService)
        {
            dialogService.ShowDialog(nameof(OpenFileDialogView));
        }
    }
}

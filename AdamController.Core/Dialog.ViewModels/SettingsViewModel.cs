using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace AdamController.Core.Dialog.ViewModels
{
    public class SettingsViewModel : BindableBase, IDialogAware
    {
        public string Title { get; }

        #region ~

        public SettingsViewModel()
        {
            Title  = "Настройки Adam IDE";
        }

        #endregion

        #region Navigation

        public event Action<IDialogResult> RequestClose;

        private DelegateCommand<string> mCloseDialogCommand;

        public DelegateCommand<string> CloseDialogCommand => mCloseDialogCommand ??= new DelegateCommand<string>(CloseDialog);

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
                result = ButtonResult.OK;
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        #endregion
    }
}

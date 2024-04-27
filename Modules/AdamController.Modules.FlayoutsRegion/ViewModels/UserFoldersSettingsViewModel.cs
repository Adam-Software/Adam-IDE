using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using Prism.Commands;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class UserFoldersSettingsViewModel : FlyoutBase
    {
        #region DelegateCommands

        public DelegateCommand<string> ShowOpenFolderDialogDelegateCommand { get; private set; }

        #endregion

        #region Services

        private readonly IDialogManagerService mDialogManager;
        private readonly IFolderManagmentService mFolderManagment;

        #endregion

        #region Const

        private const string cDialogParamWorkspace = "workspace";
        private const string cDialogParamScript = "script";

        #endregion

        public UserFoldersSettingsViewModel(IDialogManagerService dialogManager, IFolderManagmentService folderManagment) 
        {
            mDialogManager = dialogManager;
            mFolderManagment = folderManagment;

            ShowOpenFolderDialogDelegateCommand = new DelegateCommand<string>(ShowOpenFolderDialog, ShowOpenFolderDialogCanExecute);

            BorderThickness = 0;
            Header = "Настройки расположения папок пользователя";
        }

        #region Navigation


        #endregion

        #region DelegateCommand methods

        private void ShowOpenFolderDialog(string commandParam)
        {
            string dialogPathType = commandParam;

            if (string.IsNullOrEmpty(dialogPathType)) 
                return;

            OpenDialog(dialogPathType);
        }

        private bool ShowOpenFolderDialogCanExecute(string dialogPathType)
        {
            return true;
        }

        private void OpenDialog(string dialogPathType)
        {
            bool allowMultiselect = false;

            switch (dialogPathType)
            {
                case cDialogParamWorkspace:
                    {
                        var initialPath = Settings.Default.SavedUserWorkspaceFolderPath;

                        if (!string.IsNullOrEmpty(initialPath))
                            initialPath = mFolderManagment.MyDocumentsUserDir;

                        mDialogManager.ShowFolderBrowser("Выберите директорию для сохранения рабочих областей", initialPath, allowMultiselect);
                        string selectedPart = mDialogManager.FolderPath;

                        if (string.IsNullOrEmpty(selectedPart))
                            return;
                        
                        Settings.Default.SavedUserWorkspaceFolderPath = selectedPart;

                        break;
                    }
                
                case cDialogParamScript:
                    {
                        var initialPath = Settings.Default.SavedUserScriptsFolderPath;

                        if (!string.IsNullOrEmpty(initialPath))
                            initialPath = mFolderManagment.MyDocumentsUserDir;

                        mDialogManager.ShowFolderBrowser("Выберите директорию для сохранения скриптов", initialPath, allowMultiselect);
                        string selectedPart = mDialogManager.FolderPath;

                        if (string.IsNullOrEmpty(selectedPart)) return;

                        Settings.Default.SavedUserScriptsFolderPath = selectedPart;
                        break;
                    }
            }
        }

        #endregion
    }
}

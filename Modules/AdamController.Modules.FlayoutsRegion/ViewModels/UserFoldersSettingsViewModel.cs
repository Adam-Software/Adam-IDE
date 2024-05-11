using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using Prism.Commands;
using System.Windows;

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
        private readonly ICultureProvider mCultureProvider;

        #endregion

        #region Const

        private const string cDialogParamWorkspace = "workspace";
        private const string cDialogParamScript = "script";

        #endregion

        #region Var

        private string mTitleSelectWorkspaceDialog;
        private string mTitleSelectScriptDialog;

        #endregion

        public UserFoldersSettingsViewModel(IDialogManagerService dialogManager, IFolderManagmentService folderManagment, ICultureProvider cultureProvider) 
        {
            mDialogManager = dialogManager;
            mFolderManagment = folderManagment;
            mCultureProvider = cultureProvider;

            BorderThickness = 1;
        }

        #region Navigation

        protected override void OnChanging(bool isOpening)
        {
            if(isOpening)
            {
                LoadResource();

                ShowOpenFolderDialogDelegateCommand = new DelegateCommand<string>(ShowOpenFolderDialog, ShowOpenFolderDialogCanExecute);
                BorderBrush = Application.Current.TryFindResource("MahApps.Brushes.Text").ToString();
                return;
            }

            if (!isOpening)
            {
                ShowOpenFolderDialogDelegateCommand = null;
                return;
            }
        }

        #endregion

        #region Private methods

        private void LoadResource()
        {
            Header = mCultureProvider.FindResource("UserFoldersSettingsView.ViewModel.Flyout.Header");
            mTitleSelectWorkspaceDialog = mCultureProvider.FindResource("UserFoldersSettingsView.ViewModel.TitleSelectWorkspaceDialog");
            mTitleSelectScriptDialog = mCultureProvider.FindResource("UserFoldersSettingsView.ViewModel.TitleSelectScriptDialog");


        }

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

                        mDialogManager.ShowFolderBrowser(mTitleSelectWorkspaceDialog, initialPath, allowMultiselect);
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

                        mDialogManager.ShowFolderBrowser(mTitleSelectScriptDialog, initialPath, allowMultiselect);
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

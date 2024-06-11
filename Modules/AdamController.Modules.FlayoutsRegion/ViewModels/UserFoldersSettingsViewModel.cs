﻿using AdamStudio.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamStudio.Core.Properties;
using AdamStudio.Services.Interfaces;
using AdamStudio.Services.SystemDialogServiceDependency;
using AdamStudio.Services.SystemDialogServiceDependency;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows;

namespace AdamStudio.Modules.FlayoutsRegion.ViewModels
{
    public class UserFoldersSettingsViewModel : FlyoutBase
    {
        #region DelegateCommands

        public DelegateCommand<string> ShowOpenFolderDialogDelegateCommand { get; private set; }

        #endregion

        #region Services

        private readonly IFolderManagmentService mFolderManagment;
        private readonly ICultureProvider mCultureProvider;
        private readonly ISystemDialogService mSystemDialogService;

        #endregion

        #region Const

        private const string cDialogParamWorkspace = "workspace";
        private const string cDialogParamScript = "script";

        #endregion

        #region Var

        private string mTitleSelectWorkspaceDialog;
        private string mTitleSelectScriptDialog;

        #endregion

        public UserFoldersSettingsViewModel(IFolderManagmentService folderManagment, ICultureProvider cultureProvider, ISystemDialogService systemDialogService) 
        {
            ShowOpenFolderDialogDelegateCommand = new DelegateCommand<string>(ShowOpenFolderDialog, ShowOpenFolderDialogCanExecute);

            mFolderManagment = folderManagment;
            mCultureProvider = cultureProvider;
            mSystemDialogService = systemDialogService;

            BorderThickness = 1;
        }

        #region Navigation

        protected override void OnChanging(bool isOpening)
        {
            if(isOpening)
            {
                LoadResource();
                BorderBrush = Application.Current.TryFindResource("MahApps.Brushes.Text").ToString();
                return;
            }
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
            switch (dialogPathType)
            {
                case cDialogParamWorkspace:
                    {
                        var initialPath = Settings.Default.SavedUserWorkspaceFolderPath;

                        if (!string.IsNullOrEmpty(initialPath))
                            initialPath = mFolderManagment.MyDocumentsUserDir;

                        var dialogParametrs = new DialogParameters
                        {
                            { DialogParametrsKeysName.TitleParametr, mTitleSelectWorkspaceDialog },
                            { DialogParametrsKeysName.InitialDirectoryParametr, initialPath },
                        };

                        OpenFolderDialogResult result = mSystemDialogService.ShowOpenFolderDialog(dialogParametrs);

                        if (result.IsSelectFolderCanceled)
                            return;

                        string selectedPart = result.SelectedFolderPath;

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

                        var dialogParametrs = new DialogParameters
                        {
                            { DialogParametrsKeysName.TitleParametr, mTitleSelectScriptDialog },
                            { DialogParametrsKeysName.InitialDirectoryParametr, initialPath },
                        };

                        var result = mSystemDialogService.ShowOpenFolderDialog(dialogParametrs);

                        if (result.IsSelectFolderCanceled)
                            return;

                        string selectedPart = result.SelectedFolderPath;

                        if (string.IsNullOrEmpty(selectedPart)) return;

                        Settings.Default.SavedUserScriptsFolderPath = selectedPart;
                        break;
                    }
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
    }
}

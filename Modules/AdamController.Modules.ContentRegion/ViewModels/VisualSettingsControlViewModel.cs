using AdamBlocklyLibrary.Enum;
using AdamController.Controls.CustomControls.Services;
using AdamController.Core;
using AdamController.Core.Mvvm;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using ControlzEx.Theming;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Media;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class VisualSettingsControlViewModel : RegionViewModelBase 
    {
        #region DelegateCommands

        public DelegateCommand ChangeSpacingToggleSwitchDelegateCommand { get; }
        public DelegateCommand OpenPortSettingsDelegateCommand { get; }
        public DelegateCommand OpenWebApiSettingsDelegateCommand { get; }
        public DelegateCommand OpenUserFolderSettingsDelegateCommand { get; }   

        #endregion

        #region Services

        private readonly IFlyoutManager mFlyoutManager;
        private readonly IThemeManagerService mThemeManager;
        private readonly ICultureProvider mCultureProvider;

        #endregion

        #region ~

        public VisualSettingsControlViewModel(IRegionManager regionManager, IFlyoutManager flyoutManager, IThemeManagerService themeManager, ICultureProvider cultureProvider) : base(regionManager)
        {
            mFlyoutManager = flyoutManager;
            mThemeManager = themeManager;
            mCultureProvider = cultureProvider;

            ChangeSpacingToggleSwitchDelegateCommand = new DelegateCommand(ChangeSpacingToggleSwitch, ChangeSpacingToggleSwitchCanExecute);
            OpenPortSettingsDelegateCommand = new DelegateCommand(OpenPortSettings, OpenPortSettingsCanExecute);
            OpenWebApiSettingsDelegateCommand = new DelegateCommand(OpenWebApiSettings, OpenWebApiSettingsCanExecute);
            OpenUserFolderSettingsDelegateCommand = new DelegateCommand(OpenUserFolderSettings, OpenUserFolderSettingsCanExecute);
        }

        #endregion

        #region  DelegateCommand methods

        private void ChangeSpacingToggleSwitch()
        {
            Settings.Default.BlocklyGridSpacing = 20;
        }

        private bool ChangeSpacingToggleSwitchCanExecute()
        {
            return true;
        }

        private void OpenPortSettings()
        {
            mFlyoutManager.OpenFlyout(FlyoutNames.FlyoutPortSettings);
        }

        private bool OpenPortSettingsCanExecute()
        {
            return true;
        }

        private void OpenWebApiSettings()
        {
            mFlyoutManager.OpenFlyout(FlyoutNames.FlyoutWebApiSettings);
        }

        private bool OpenWebApiSettingsCanExecute()
        {
            return true;
        }

        private void OpenUserFolderSettings()
        {
            mFlyoutManager.OpenFlyout(FlyoutNames.FlyoutUserFoldersSettings);
        }

        private bool OpenUserFolderSettingsCanExecute()
        {
            return true;
        }


        #endregion

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            ThemesCollection = mThemeManager.AppThemesCollection;
            SelectedTheme = mThemeManager.GetCurrentAppTheme();

            LanguageApp = mCultureProvider.SupportAppCultures;
            SelectedLanguageApp = mCultureProvider.CurrentAppCulture;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }


        #endregion

        #region Public fields

        private List<CultureInfo> languageApp;
        public List<CultureInfo> LanguageApp 
        {
            get => languageApp;
            private set => SetProperty(ref languageApp, value); 
        }

        private CultureInfo selectedLanguageApp;
        public CultureInfo SelectedLanguageApp
        {
            get => selectedLanguageApp;
            set
            {
                bool isNewValue = SetProperty(ref selectedLanguageApp, value);

                if(isNewValue)
                    ChangeAppLanguage(SelectedLanguageApp);
            }
        }

        public ReadOnlyObservableCollection<Theme> themesCollection;
        public ReadOnlyObservableCollection<Theme> ThemesCollection 
        {
            get => themesCollection;
            set => SetProperty(ref themesCollection, value);
        }

        public Theme selectedTheme;
        public Theme SelectedTheme
        {
            get => selectedTheme;
            set 
            {
                bool isNewValue = SetProperty(ref selectedTheme, value);

                if (isNewValue)
                    ChangeTheme(SelectedTheme);
            } 
        }

        #endregion

        #region Private methods

        private void ChangeTheme(Theme theme)
        {
            Theme chagedTheme = mThemeManager.ChangeAppTheme(theme);
            ChangeBlockllyTheme(chagedTheme.BaseColorScheme);

            Settings.Default.AppThemeName = chagedTheme.Name;
        }

        private void ChangeBlockllyTheme(string baseColorAppTheme)
        {
            if (!Settings.Default.ChangeBlocklyThemeToggleSwitchState)
            {
                if (baseColorAppTheme == "Dark")
                {
                    Settings.Default.BlocklyTheme = BlocklyTheme.Dark;
                    Settings.Default.BlocklyGridColour = Colors.White.ToString();
                }
                
                if (baseColorAppTheme == "Light")
                {
                    Settings.Default.BlocklyTheme = BlocklyTheme.Classic;
                    Settings.Default.BlocklyGridColour = Colors.Black.ToString();
                }
            }
        }

        private void ChangeAppLanguage(CultureInfo cultureInfo)
        {
            mCultureProvider.ChangeAppCulture(cultureInfo);
            ChangeBlocklyLanguage(cultureInfo);

            Settings.Default.AppLanguage = cultureInfo.Name;
        }

        private void ChangeBlocklyLanguage(CultureInfo cultureInfo)
        {
            if (cultureInfo.TwoLetterISOLanguageName == "en")
            {
                Settings.Default.BlocklyWorkspaceLanguage = BlocklyLanguage.en;
                Settings.Default.BlocklyToolboxLanguage = BlocklyLanguage.en;
            }
                
            if (cultureInfo.TwoLetterISOLanguageName == "ru")
            {
                Settings.Default.BlocklyWorkspaceLanguage = BlocklyLanguage.ru;
                Settings.Default.BlocklyToolboxLanguage = BlocklyLanguage.ru;
            }
        }

        #endregion
    }
}

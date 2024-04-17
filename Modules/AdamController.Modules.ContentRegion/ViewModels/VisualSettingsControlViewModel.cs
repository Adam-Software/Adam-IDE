using AdamBlocklyLibrary.Enum;
using AdamController.Controls.CustomControls.Services;
using AdamController.Core;
using AdamController.Core.DataSource;
using AdamController.Core.Model;
using AdamController.Core.Mvvm;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using ControlzEx.Theming;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class VisualSettingsControlViewModel : RegionViewModelBase 
    {
        #region DelegateCommands

        public DelegateCommand OpenAdvancedBlocklySettingsDelegateCommand { get; }

        public DelegateCommand<string> ChangeBaseThemeDelegateCommand { get; }
        
        //public DelegateCommand<string> ChangeThemeColorSchemeDelegateCommand { get; }

        #endregion

        #region Action

        //public static Action<string> ChangeBaseTheme { get; set; }

        public static Action<double> ChangeNotificationOpacity { get; set; }
        //public static Action<string> ChangeThemeColorScheme { get; set; }
        //public static Action<bool> OpenAdvancedBlocklySettings { get; set; }
        public static Action<BlocklyLanguageModel> SetToolboxLanguage { get; set; }
        public static Action<BlocklyThemeModel> SetBlocklyThemeAndGridColor { get; set; }

        #endregion

        #region Service

        private readonly IFlyoutManager mFlyoutManager;
        private readonly IWebViewProvider mWebViewProvider;

        #endregion

        #region ~

        public VisualSettingsControlViewModel(IRegionManager regionManager, IDialogService dialogService, IFlyoutManager flyoutManager, IWebViewProvider webViewProvider) : base(regionManager, dialogService)
        {
            mFlyoutManager = flyoutManager;
            mWebViewProvider = webViewProvider;

            OpenAdvancedBlocklySettingsDelegateCommand = new DelegateCommand(OpenAdvancedBlocklySettings, OpenAdvancedBlocklySettingsCanExecute);
            ChangeBaseThemeDelegateCommand = new DelegateCommand<string>(ChangeBaseTheme, ChangeBaseThemeCanExecute);
            //ChangeThemeColorScheme = new DelegateCommand<string>()
        }

        #endregion

        #region  DelegateCommand methods

        private void OpenAdvancedBlocklySettings()
        {
            mFlyoutManager.OpenFlyout(FlyoutNames.FlyotAdvancedBlocklySettings);
        }

        private bool OpenAdvancedBlocklySettingsCanExecute()
        {
            return true;
        }

        private void ChangeBaseTheme(string theme)
        {
            ChangeTheme(theme);
        }

        private bool ChangeBaseThemeCanExecute(string theme)
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
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }


        #endregion

        #region Private methods

        private void ChangeTheme(string newTheme)
        {
            _ = ThemeManager.Current.ChangeThemeBaseColor(Application.Current, newTheme);

            if (!Settings.Default.ChangeBlocklyThemeToggleSwitchState)
            {
                if (newTheme == "Dark")
                {
                    Settings.Default.BlocklyTheme = BlocklyTheme.Dark;
                    Settings.Default.BlocklyGridColour = Colors.White.ToString();
                }
                
                if (newTheme == "Light")
                {
                    Settings.Default.BlocklyTheme = BlocklyTheme.Classic;
                    Settings.Default.BlocklyGridColour = Colors.Black.ToString();
                }
            }
        }

        private void ChangeThemeColorScheme(string colorScheme)
        {
           ThemeManager.Current.ChangeThemeColorScheme(Application.Current, colorScheme);
        }

        #endregion

        public static ObservableCollection<BlocklyThemeModel> BlocklyThemes { get; private set; } = ThemesCollection.BlocklyThemes;

        public static ObservableCollection<AppLanguageModel> LanguageApp { get; private set; } = LanguagesCollection.AppLanguageCollection;

        private AppLanguageModel selectedLanguageApp = LanguageApp.FirstOrDefault(x => x.AppLanguage == Settings.Default.AppLanguage);
        public AppLanguageModel SelectedLanguageApp
        {
            get => selectedLanguageApp;
            set
            {
                if (value == selectedLanguageApp)
                {
                    return;
                }

                selectedLanguageApp = value;

                SetProperty(ref selectedLanguageApp, value);

                Settings.Default.AppLanguage = selectedLanguageApp.AppLanguage;
            }
        }

        public static ObservableCollection<BlocklyLanguageModel> BlocklyLanguageCollection { get; private set; } = LanguagesCollection.BlocklyLanguageCollection;
       
        private BlocklyLanguageModel selectedBlocklyWorkspaceLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyWorkspaceLanguage);
        public BlocklyLanguageModel SelectedBlocklyWorkspaceLanguage
        {
            get => selectedBlocklyWorkspaceLanguage;
            set
            {
                if (value == selectedBlocklyWorkspaceLanguage)
                {
                    return;
                }

                selectedBlocklyWorkspaceLanguage = value;

                SetProperty(ref selectedBlocklyWorkspaceLanguage, value);

                if (!Settings.Default.ChangeToolboxLanguageToggleSwitchState)
                {
                    SetToolboxLanguage(selectedBlocklyWorkspaceLanguage);
                }

                Settings.Default.BlocklyWorkspaceLanguage = selectedBlocklyWorkspaceLanguage.BlocklyLanguage;
            }
        }

        public ReadOnlyObservableCollection<string> ColorScheme { get; set; } = ThemeManager.Current.ColorSchemes;

        private string selectedColorScheme = ThemeManager.Current.DetectTheme(Application.Current).ColorScheme;
        public string SelectedColorScheme
        {
            get => selectedColorScheme;
            set
            {
                bool isNewValue = SetProperty(ref selectedColorScheme, value);

                if(isNewValue)
                    ChangeThemeColorScheme(selectedColorScheme);
            }
        }

        //TODO unused this variable
        private short blocklyGridLenth = Settings.Default.BlocklyGridLenth;
        public short BlocklyGridLenth
        {
            get => blocklyGridLenth;
            set
            {
                if (value == blocklyGridLenth)
                {
                    return;
                }

                blocklyGridLenth = value;

                SetProperty(ref blocklyGridLenth, value);
                Settings.Default.BlocklyGridLenth = BlocklyGridLenth;
            }
        }

        
        private double notificationOpacity = Settings.Default.NotificationOpacity;
        public double NotificationOpacity
        {
            get => notificationOpacity;
            set
            {
                if (value == notificationOpacity) return;

                notificationOpacity = value;

                ChangeNotificationOpacity(notificationOpacity);

                SetProperty(ref notificationOpacity, value);
            }
        }


        

        //private DelegateCommand openAdvancedBlocklySettingsCommand;
        //public DelegateCommand OpenAdvancedBlocklySettingsCommand => openAdvancedBlocklySettingsCommand ??= new DelegateCommand(() =>
        //{
        //    FlyoutManager.OpenFlyout(FlyoutNames.FlyotAdvancedBlocklySettings);
        //});

        //private DelegateCommand<string> changeBaseColorTheme;

        //public DelegateCommand<string> ChangeBaseColorTheme => changeBaseColorTheme ??= new DelegateCommand<string>(obj =>
        //{
            //string mainTheme = obj;

            //if (mainTheme == null) return;

            //ChangeBaseTheme(mainTheme);

            //if (!Settings.Default.ChangeBlocklyThemeToggleSwitchState)
            //{
            //    if (mainTheme == "Dark")
            //    {
            //        SetBlocklyThemeAndGridColor(BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Dark));
            //    }
            //    else if (mainTheme == "Light")
            //    {
            //        SetBlocklyThemeAndGridColor(BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Classic));
            //    }
            //}
        //});

    }
}

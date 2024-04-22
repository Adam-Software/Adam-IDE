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
        
        #endregion

        #region Services

        private readonly IFlyoutManager mFlyoutManager;        
        private readonly IAppThemeManager mAppThemeManager;

        #endregion

        #region ~

        public VisualSettingsControlViewModel(IRegionManager regionManager, IDialogService dialogService, IFlyoutManager flyoutManager, IAppThemeManager appThemeManager) : base(regionManager, dialogService)
        {
            mFlyoutManager = flyoutManager;
            mAppThemeManager = appThemeManager;

            OpenAdvancedBlocklySettingsDelegateCommand = new DelegateCommand(OpenAdvancedBlocklySettings, OpenAdvancedBlocklySettingsCanExecute);
            ChangeBaseThemeDelegateCommand = new DelegateCommand<string>(ChangeBaseTheme, ChangeBaseThemeCanExecute);
            var s = mAppThemeManager.Name;
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

            LanguageApp = new ObservableCollection<AppLanguageModel>
            {
                new AppLanguageModel { AppLanguage = "ru", LanguageName = "Русский" }
            };


            BlocklyLanguageCollection = new ObservableCollection<BlocklyLanguageModel>
            {
                new BlocklyLanguageModel { BlocklyLanguage = BlocklyLanguage.ru, LanguageName = "Русский" },
                new BlocklyLanguageModel { BlocklyLanguage =  BlocklyLanguage.en, LanguageName = "Английский" }
            };

            BlocklyThemes = ThemesCollection.BlocklyThemes;
            ColorScheme = ThemeManager.Current.ColorSchemes;

            //SelectedLanguageApp = LanguageApp.FirstOrDefault(x => x.AppLanguage == Settings.Default.AppLanguage);
            //SelectedBlocklyWorkspaceLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyWorkspaceLanguage);

            SelectedColorScheme = ThemeManager.Current.DetectTheme(Application.Current).ColorScheme;
            NotificationOpacity = Settings.Default.NotificationOpacity;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }


        #endregion

        #region Public fields

        private ObservableCollection<AppLanguageModel> languageApp;
        public ObservableCollection<AppLanguageModel> LanguageApp 
        {
            get => languageApp;
            private set => SetProperty(ref languageApp, value); 
        }

        private ObservableCollection<BlocklyLanguageModel> blocklyLanguageCollection;
        public ObservableCollection<BlocklyLanguageModel> BlocklyLanguageCollection 
        {
            get => blocklyLanguageCollection;
            private set => SetProperty(ref  blocklyLanguageCollection, value);
        }

        private ObservableCollection<BlocklyThemeModel> blocklyThemes;
        public ObservableCollection<BlocklyThemeModel> BlocklyThemes 
        { 
            get => blocklyThemes; 
            private set => SetProperty(ref blocklyThemes, value); 
        }

        private ReadOnlyObservableCollection<string> colorScheme;
        public ReadOnlyObservableCollection<string> ColorScheme 
        { 
            get => colorScheme;
            private set => SetProperty(ref colorScheme, value); 
        }

        /*private AppLanguageModel selectedLanguageApp;
        public AppLanguageModel SelectedLanguageApp
        {
            get => selectedLanguageApp;
            set
            {
                if (value == selectedLanguageApp)
                {
                    return;
                }

                bool isNewValue = SetProperty(ref selectedLanguageApp, value);

                if (isNewValue)
                    Settings.Default.AppLanguage = SelectedLanguageApp.AppLanguage;
            }
        }*/

        /*private BlocklyLanguageModel selectedBlocklyWorkspaceLanguage;
        public BlocklyLanguageModel SelectedBlocklyWorkspaceLanguage
        {
            get => selectedBlocklyWorkspaceLanguage;
            set
            {
                bool isNewValue = SetProperty(ref selectedBlocklyWorkspaceLanguage, value);

                if (isNewValue)
                    Settings.Default.BlocklyWorkspaceLanguage = SelectedBlocklyWorkspaceLanguage.BlocklyLanguage;
            }
        }*/

        private string selectedColorScheme;
        public string SelectedColorScheme
        {
            get => selectedColorScheme;
            set
            {
                bool isNewValue = SetProperty(ref selectedColorScheme, value);

                if (isNewValue)
                    ChangeThemeColorScheme(SelectedColorScheme);
            }
        }


        private double notificationOpacity;
        public double NotificationOpacity
        {
            get => notificationOpacity;
            set => SetProperty(ref notificationOpacity, value);
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
    }
}

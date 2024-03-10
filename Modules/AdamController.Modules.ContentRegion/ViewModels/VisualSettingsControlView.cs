using AdamBlocklyLibrary.Enum;
using AdamController.Core.DataSource;
using AdamController.Core.Model;
using AdamController.Core.Properties;
using AdamController.Core.ViewModels.HamburgerMenu;
using AdamController.ViewModels.HamburgerMenu;
using ControlzEx.Theming;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class VisualSettingsControlView : HamburgerMenuItemView
    {
        //public VisualSettingsControlView(HamburgerMenuView hamburgerMenuView) : base(hamburgerMenuView) {}

        public static ObservableCollection<BlocklyThemeModel> BlocklyThemes { get; private set; } = ThemesCollection.BlocklyThemes;

        #region LanguageSettings

        #region AppLanguage

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

        #endregion

        #region BlocklyLanguage

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

        #endregion

        #endregion

        #region ColorScheme Settings

        public ReadOnlyObservableCollection<string> ColorScheme { get; set; } = ThemeManager.Current.ColorSchemes;

        private string selectedColorScheme = ThemeManager.Current.DetectTheme(Application.Current).ColorScheme;
        public string SelectedColorScheme
        {
            get => selectedColorScheme;
            set
            {
                if (value == selectedColorScheme)
                {
                    return;
                }

                selectedColorScheme = value;

                ChangeThemeColorScheme(selectedColorScheme);

                SetProperty(ref selectedColorScheme, value);
            }
        }

        #endregion

        #region BlocklyGridLenth settings
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

        #endregion

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


        #region Command

        private DelegateCommand openAdvancedBlocklySettingsCommand;
        public DelegateCommand OpenAdvancedBlocklySettingsCommand => openAdvancedBlocklySettingsCommand ??= new DelegateCommand(() =>
        {
            OpenAdvancedBlocklySettings(true);
        });

        private DelegateCommand<string> changeBaseColorTheme;
        public DelegateCommand<string> ChangeBaseColorTheme => changeBaseColorTheme ??= new DelegateCommand<string>(obj =>
        {
            string mainTheme = obj;

            if (mainTheme == null) return;

            ChangeBaseTheme(mainTheme);

            if (!Settings.Default.ChangeBlocklyThemeToggleSwitchState)
            {
                if (mainTheme == "Dark")
                {
                    SetBlocklyThemeAndGridColor(BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Dark));
                }
                else if (mainTheme == "Light")
                {
                    SetBlocklyThemeAndGridColor(BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Classic));
                }
            }

        });

        #endregion

        #region Action

        public static Action<double> ChangeNotificationOpacity { get; set; }
        public static Action<string> ChangeBaseTheme { get; set; }
        public static Action<string> ChangeThemeColorScheme { get; set; }
        public static Action<bool> OpenAdvancedBlocklySettings { get; set; }
        public static Action<BlocklyLanguageModel> SetToolboxLanguage { get; set; }
        public static Action<BlocklyThemeModel> SetBlocklyThemeAndGridColor { get; set; }
      
        #endregion
    }
}

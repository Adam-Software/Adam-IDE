using AdamBlocklyLibrary.Enum;
using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Core.DataSource;
using AdamController.Core.Model;
using AdamController.Core.Properties;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;


namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class AdvancedBlocklySettingsViewModel : FlyoutBase 
    {
        public AdvancedBlocklySettingsViewModel() 
        {
            Header = "Продвинутые настройки скретч-редактора";
            
        }

        protected override void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
        }

        protected override void OnClosing(FlyoutParameters flyoutParameters)
        {
            base.OnClosing(flyoutParameters);
        }

        #region BlocklyGridColour settings

        private Color? selectedBlocklyGridColour = MahApps.Metro.Controls.ColorHelper.ColorFromString(Settings.Default.BlocklyGridColour);
        public Color? SelectedBlocklyGridColour
        {
            get => selectedBlocklyGridColour;
            set
            {
                if (value == selectedBlocklyGridColour)
                {
                    return;
                }
                selectedBlocklyGridColour = value;

                SetProperty(ref selectedBlocklyGridColour, value);
                Settings.Default.BlocklyGridColour = selectedBlocklyGridColour.ToString();
            }
        }

        #endregion


        #region BlocklyToolboxLanguage Settings

        public static ObservableCollection<BlocklyLanguageModel> BlocklyLanguageCollection { get; private set; } = LanguagesCollection.BlocklyLanguageCollection;

        private BlocklyLanguageModel selectedBlocklyToolboxLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyToolboxLanguage);
        public BlocklyLanguageModel SelectedBlocklyToolboxLanguage
        {
            get => selectedBlocklyToolboxLanguage;
            set
            {
                if (value == selectedBlocklyToolboxLanguage)
                {
                    return;
                }

                selectedBlocklyToolboxLanguage = value;

                SetProperty(ref selectedBlocklyToolboxLanguage, value);

                Settings.Default.BlocklyToolboxLanguage = selectedBlocklyToolboxLanguage.BlocklyLanguage;
            }
        }

        #endregion


        #region BlocklyTheme Settings

        public static ObservableCollection<BlocklyThemeModel> BlocklyThemes { get; private set; } = ThemesCollection.BlocklyThemes;

        private BlocklyThemeModel selectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == Settings.Default.BlocklyTheme);
        public BlocklyThemeModel SelectedBlocklyTheme
        {
            get => selectedBlocklyTheme;
            set
            {
                if (value == selectedBlocklyTheme)
                {
                    return;
                }

                selectedBlocklyTheme = value;
                SetProperty(ref selectedBlocklyTheme, value);

                Settings.Default.BlocklyTheme = selectedBlocklyTheme.BlocklyTheme;

                if (Settings.Default.ChangeGridColorSwitchToggleSwitchState) return;
                SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);
            }
        }

        #endregion

        #region SelectMainTheme

        private void SelectGridColorDependingSelectedTheme(BlocklyTheme theme)
        {
            switch (theme)
            {
                case BlocklyTheme.Dark:
                    {
                        SelectedBlocklyGridColour = Colors.White;
                        break;
                    }
                case BlocklyTheme.Classic:
                    {
                        SelectedBlocklyGridColour = Colors.Black;
                        break;
                    }
                default:
                    {
                        SelectedBlocklyGridColour = Colors.Black;
                        break;
                    }
            }
        }

        #endregion

        #region Commands

        private DelegateCommand<bool?> changeToolboxLanguageToggleSwitchCommand;
        public DelegateCommand<bool?> ChangeToolboxLanguageToggleSwitchCommand => changeToolboxLanguageToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) return;

            SelectedBlocklyToolboxLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyWorkspaceLanguage);

        });

        private DelegateCommand<bool?> changeGridColorToggleSwitchCommand;
        public DelegateCommand<bool?> ChangeGridColorToggleSwitchCommand => changeGridColorToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) return;

            SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);
        });

        private DelegateCommand<bool?> changeBlocklyThemeToogleSwitchCommand;
        public DelegateCommand<bool?> ChangeBlocklyThemeToogleSwitchCommand => changeBlocklyThemeToogleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) return;

            if (Settings.Default.BaseTheme == "Dark")
            {
                SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Dark);
            }
            else if (Settings.Default.BaseTheme == "Light")
            {
                SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Classic);
            }
        });

        private DelegateCommand<bool?> changeSpacingToggleSwitchCommand;
        public DelegateCommand<bool?> ChangeSpacingToggleSwitchCommand => changeSpacingToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) return;

            Settings.Default.BlocklyGridSpacing = 20;
        });

        private DelegateCommand enableShowGridCommand;
        public DelegateCommand EnableShowGridCommand => enableShowGridCommand ??= new DelegateCommand(() =>
        {
            Settings.Default.BlocklyShowGrid = true;
        });

        #endregion
    }

        
}

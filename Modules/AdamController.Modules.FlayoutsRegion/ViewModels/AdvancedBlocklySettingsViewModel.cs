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
        #region DelegateCommands

        public DelegateCommand<bool?> ChangeToolboxLanguageToggleSwitchDelegateCommand { get; private set; }
        public DelegateCommand<bool?> ChangeGridColorToggleSwitchDelegateCommand { get; private set; }
        public DelegateCommand<bool?> ChangeBlocklyThemeToogleSwitchDelegateCommand { get; private set; }
        public DelegateCommand<bool?> ChangeSpacingToggleSwitchDelegateCommand { get; private set; }
        public DelegateCommand EnableShowGridDelegateCommand { get; private set; }

        #endregion

        public AdvancedBlocklySettingsViewModel() 
        {
            SetFlyoutParametrs();
        }

        #region Navigation

        protected override void OnChanging(bool isOpening)
        {

            if (isOpening)
            {
                UpdatePublicFields();
                Subscribe();
                CreateDelegateCommand();
                return;
            }


            if (!isOpening)
            {
                ClearPublicFields();
                Unsubscribe();
                ResetDelegateCommand();
                return;
            }

            //base.OnChanging(isOpening);
        }

        #endregion

        #region Public fields

        private void UpdatePublicFields()
        {
            SelectedBlocklyGridColour = MahApps.Metro.Controls.ColorHelper.ColorFromString(Settings.Default.BlocklyGridColour);
            BlocklyLanguageCollection = new ObservableCollection<BlocklyLanguageModel>(LanguagesCollection.BlocklyLanguageCollection);
            SelectedBlocklyToolboxLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyToolboxLanguage);
            BlocklyThemes = new ObservableCollection<BlocklyThemeModel>(ThemesCollection.BlocklyThemes);
            SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == Settings.Default.BlocklyTheme);
        }

        private void ClearPublicFields()
        {
            //SelectedBlocklyGridColour = null;
            BlocklyLanguageCollection = null;
            //SelectedBlocklyToolboxLanguage = null;
            BlocklyThemes = null;
            //SelectedBlocklyTheme = null;
        }

        private Color? selectedBlocklyGridColour;
        public Color? SelectedBlocklyGridColour
        {
            get => selectedBlocklyGridColour;
            set
            {
                bool isNewValue = SetProperty(ref selectedBlocklyGridColour, value);

                if (isNewValue)
                    Settings.Default.BlocklyGridColour = SelectedBlocklyGridColour.ToString();
            }
        }

        private ObservableCollection<BlocklyLanguageModel> blocklyLanguageCollection;
        public ObservableCollection<BlocklyLanguageModel> BlocklyLanguageCollection 
        {
            get => blocklyLanguageCollection;
            set => SetProperty(ref blocklyLanguageCollection, value);
        }

        private BlocklyLanguageModel selectedBlocklyToolboxLanguage;
        public BlocklyLanguageModel SelectedBlocklyToolboxLanguage
        {
            get => selectedBlocklyToolboxLanguage;
            set => SetProperty(ref selectedBlocklyToolboxLanguage, value);
        }

        private ObservableCollection<BlocklyThemeModel> blocklyThemes;
        public ObservableCollection<BlocklyThemeModel> BlocklyThemes 
        {
            get => blocklyThemes;
            set => SetProperty(ref blocklyThemes, value);
        }

        private BlocklyThemeModel selectedBlocklyTheme;
        public BlocklyThemeModel SelectedBlocklyTheme
        {
            get => selectedBlocklyTheme;
            set => SetProperty(ref selectedBlocklyTheme, value);
        }

        #endregion

        #region Private metods

        private void SetFlyoutParametrs()
        {
            Theme = FlyoutTheme.Inverse;
            Header = "Продвинутые настройки скретч-редактора";
            IsModal = true;
        }

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

        private void CreateDelegateCommand()
        {
            ChangeToolboxLanguageToggleSwitchDelegateCommand = new DelegateCommand<bool?>(ChangeToolboxLanguageToggleSwitch, ChangeToolboxLanguageToggleSwitchCanExecute);
            ChangeGridColorToggleSwitchDelegateCommand = new DelegateCommand<bool?>(ChangeGridColorToggleSwitch, ChangeGridColorToggleSwitchCanExecute);
            ChangeBlocklyThemeToogleSwitchDelegateCommand = new DelegateCommand<bool?>(ChangeBlocklyThemeToogleSwitch, ChangeBlocklyThemeToogleSwitchCanExecute);
            ChangeSpacingToggleSwitchDelegateCommand = new DelegateCommand<bool?>(ChangeSpacingToggleSwitch, ChangeSpacingToggleSwitchCanExecute);
            EnableShowGridDelegateCommand = new DelegateCommand(EnableShowGridDelegate, EnableShowGridDelegateCanExecute);
        }

        private void ResetDelegateCommand()
        {
            ChangeToolboxLanguageToggleSwitchDelegateCommand = null;
            ChangeGridColorToggleSwitchDelegateCommand = null;
            ChangeBlocklyThemeToogleSwitchDelegateCommand = null;
            ChangeSpacingToggleSwitchDelegateCommand = null;
            EnableShowGridDelegateCommand = null;
        }

        #endregion

        #region Delegate command methods

        private void ChangeToolboxLanguageToggleSwitch(bool? obj)
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) 
                return;

            SelectedBlocklyToolboxLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyWorkspaceLanguage);
            Settings.Default.BlocklyToolboxLanguage = SelectedBlocklyToolboxLanguage.BlocklyLanguage;
        }

        private bool ChangeToolboxLanguageToggleSwitchCanExecute(bool? nullable)
        {
            return true;
        }

        private void ChangeGridColorToggleSwitch(bool? obj)
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) 
                return;

            SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);

            //if (isNewValue)
            //{
            //    Settings.Default.BlocklyTheme = SelectedBlocklyTheme.BlocklyTheme;

            if (Settings.Default.ChangeGridColorSwitchToggleSwitchState)
                    return;

            SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);
            Settings.Default.BlocklyTheme = SelectedBlocklyTheme.BlocklyTheme;

            //    SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);
            //}
        }

        private bool ChangeGridColorToggleSwitchCanExecute(bool? nullable)
        {
            return true;
        }

        private void ChangeBlocklyThemeToogleSwitch(bool? obj)
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) 
                return;

            if (Settings.Default.BaseTheme == "Dark")
            {
                 SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Dark);
            }
            else if (Settings.Default.BaseTheme == "Light")
            {
                SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Classic);
            }
        }

        private bool ChangeBlocklyThemeToogleSwitchCanExecute(bool? nullable)
        {
            return true;
        }

        private void ChangeSpacingToggleSwitch(bool? obj)
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) 
                return;
                
            Settings.Default.BlocklyGridSpacing = 20;
        }

        private bool ChangeSpacingToggleSwitchCanExecute(bool? nullable)
        {
            return true;
        }

        private void EnableShowGridDelegate()
        {
            Settings.Default.BlocklyShowGrid = true;
        }

        private bool EnableShowGridDelegateCanExecute()
        {
            return true;
        }

        #endregion

        #region Subscriptions

        private void Subscribe()
        {
            
        }

        private void Unsubscribe()
        {
            
        }

        #endregion

        #region Commands

        //private DelegateCommand<bool?> changeToolboxLanguageToggleSwitchCommand;
        //public DelegateCommand<bool?> ChangeToolboxLanguageToggleSwitchCommand => changeToolboxLanguageToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        //{
            //bool? toogleSwitchState = obj;

            //if (toogleSwitchState == true) return;

            //SelectedBlocklyToolboxLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyWorkspaceLanguage);

        //});

        //private DelegateCommand<bool?> changeGridColorToggleSwitchCommand;
        //public DelegateCommand<bool?> ChangeGridColorToggleSwitchCommand => changeGridColorToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        //{
        //    bool? toogleSwitchState = obj;

        //    if (toogleSwitchState == true) return;

        //    SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);
        //});

        //private DelegateCommand<bool?> changeBlocklyThemeToogleSwitchCommand;
        //public DelegateCommand<bool?> ChangeBlocklyThemeToogleSwitchCommand => changeBlocklyThemeToogleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        //{
        //    bool? toogleSwitchState = obj;

        //    if (toogleSwitchState == true) return;

        //    if (Settings.Default.BaseTheme == "Dark")
        //    {
        //        SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Dark);
        //    }
        //    else if (Settings.Default.BaseTheme == "Light")
        //    {
        //        SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Classic);
        //    }
        //});

        //private DelegateCommand<bool?> changeSpacingToggleSwitchCommand;
        //public DelegateCommand<bool?> ChangeSpacingToggleSwitchCommand => changeSpacingToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        //{
        //    bool? toogleSwitchState = obj;

        //    if (toogleSwitchState == true) return;

        //    Settings.Default.BlocklyGridSpacing = 20;
        //});

        //private DelegateCommand enableShowGridCommand;
        //public DelegateCommand EnableShowGridCommand => enableShowGridCommand ??= new DelegateCommand(() =>
        //{
        //    Settings.Default.BlocklyShowGrid = true;
        //});

        #endregion
    }

        
}

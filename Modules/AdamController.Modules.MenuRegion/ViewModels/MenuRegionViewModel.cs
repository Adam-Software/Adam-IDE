using AdamController.Core.Extensions;
using AdamController.Core;
using AdamController.Core.Mvvm;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;
using System;

namespace AdamController.Modules.MenuRegion.ViewModels
{
    public class MenuRegionViewModel : RegionViewModelBase
    {

        public DelegateCommand CloseAppCommand { get; }
        public DelegateCommand<string> ShowDialogCommand { get; }
        public DelegateCommand<string> ShowRegionCommand { get; }

        public MenuRegionViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
            CloseAppCommand = new DelegateCommand(CloseApp);
            ShowDialogCommand = new DelegateCommand<string>(ShowDialog);
            ShowRegionCommand = new DelegateCommand<string>(ShowRegion);
        }

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


        #region Command methods

        private void ShowDialog(string dialogNames)
        {
            switch (dialogNames)
            {
                case DialogNames.SettingsDialog:
                    DialogService.ShowSettingsDialog();
                    break;
            }
        }

        private void ShowRegion(string subRegionName)
        {
            switch (subRegionName)
            {
                case SubRegionNames.SubRegionScratch:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionScratch);
                    break;
                case SubRegionNames.SubRegionScriptEditor:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionScriptEditor);
                    break;
                case SubRegionNames.SubRegionComputerVisionControl:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionComputerVisionControl);
                    break;
                case SubRegionNames.SubRegionVisualSettings:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionVisualSettings);
                    break;
            }
        }

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}

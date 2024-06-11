using AdamController.Core;
using AdamController.Core.Mvvm;
using Prism.Commands;
using Prism.Regions;
using System.Windows;
using System;
using AdamController.Services.Interfaces;

namespace AdamController.Modules.MenuRegion.ViewModels
{
    public class MenuRegionViewModel : RegionViewModelBase
    {

        #region DelegateCommands

        public DelegateCommand CloseAppCommand { get; }    
        public DelegateCommand<string> ShowRegionCommand { get; }

        #endregion

        #region Services

        private readonly ISubRegionChangeAwareService mSubRegionChangeAware;

        #endregion

        public MenuRegionViewModel(IRegionManager regionManager,ISubRegionChangeAwareService subRegionChangeAware) : base(regionManager)
        {
            mSubRegionChangeAware = subRegionChangeAware;

            CloseAppCommand = new DelegateCommand(CloseApp);
            ShowRegionCommand = new DelegateCommand<string>(ShowRegion);
        }

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Subscribe();

            base.OnNavigatedTo(navigationContext);
        }

        public override void Destroy()
        {
            Unsubscribe();
        }

        #endregion

        #region Public fields

        private bool isCheckedScratchMenuItem;
        public bool IsCheckedScratchMenuItem
        {
            get => isCheckedScratchMenuItem;   
            set => SetProperty(ref isCheckedScratchMenuItem, value);
        }

        private bool isCheckedComputerVisionMenuItem;
        public bool IsCheckedComputerVisionMenuItem
        {
            get => isCheckedComputerVisionMenuItem;
            set => SetProperty(ref isCheckedComputerVisionMenuItem, value);
        }

        private bool isCheckedVisualSettingsMenuItem;
        public bool IsCheckedVisualSettingsMenuItem
        {
            get => isCheckedVisualSettingsMenuItem;
            set => SetProperty(ref isCheckedVisualSettingsMenuItem, value);
        }

        #endregion

        #region Private methods

        private void ChangeCheckedMenuItem(string selectedSubRegionName)
        {
            ResetIsCheckedMenuItem();

            switch (selectedSubRegionName)
            {
                case SubRegionNames.SubRegionScratch:
                    IsCheckedScratchMenuItem = true;
                    break;
                case SubRegionNames.SubRegionComputerVisionControl:
                    IsCheckedComputerVisionMenuItem = true;
                    break;
                case SubRegionNames.SubRegionVisualSettings:    
                    IsCheckedVisualSettingsMenuItem = true;
                    break;
            }
        }

        private void ResetIsCheckedMenuItem()
        {
            IsCheckedScratchMenuItem = false;
            IsCheckedComputerVisionMenuItem = false;
            IsCheckedVisualSettingsMenuItem = false;
        }

        #endregion

        #region Subscription

        private void Subscribe()
        {
            mSubRegionChangeAware.RaiseSubRegionChangeEvent += RaiseSubRegionChangeEvent;
        }

        private void Unsubscribe() 
        {
            mSubRegionChangeAware.RaiseSubRegionChangeEvent -= RaiseSubRegionChangeEvent;
        }

        #endregion

        #region Event methods

        private void RaiseSubRegionChangeEvent(object sender)
        {
            ChangeCheckedMenuItem(mSubRegionChangeAware.InsideRegionNavigationRequestName);
        }

        #endregion

        #region Command methods

        private void ShowRegion(string subRegionName)
        {
            switch (subRegionName)
            {
                case SubRegionNames.SubRegionScratch:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionScratch);
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

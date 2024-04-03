using AdamController.Core;
using AdamController.Core.Mvvm;
using AdamController.Modules.FlayoutsRegion.Views;
using AdamController.Services.FlayoutsRegionEventAwareServiceDependency;
using AdamController.Services.Interfaces;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class FlayoutsViewModel : FlyoutBase//:  RegionViewModelBase
    {
        IFlyoutManager FlyoutManager { get; }


        public FlayoutsViewModel()
        {
            Position = FlyoutPosition.Right;
            Theme = FlyoutTheme.Dark;   
        }

        protected override void OnChanging(bool isOpening)
        {
            base.OnChanging(isOpening);
        }

        protected override void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
            IsOpen = true;
        }

        //public FlayoutsViewModel(IRegionManager regionManager, IDialogService dialogService, FlyoutManager flyoutManager) : base(regionManager, dialogService)
        //{
        //    FlyoutManager = flyoutManager;
        //
        //    FlyoutManager.SetDefaultFlyoutRegion(FlayoutsRegionNames.FlayoutsInsideRegion);
        //}



        //public FlayoutsViewModel(IRegionManager regionManager, IDialogService dialogService, IFlyoutManager flyoutManager) : base(regionManager, dialogService)
        //{
        //    FlyoutManager = flyoutManager;
        //}

        //public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        //{
        //    SubFlayoutsRequestNavigate(FlayoutsRegionNames.FlayotAdvancedBlocklySettings, navigationContext.Parameters);
        //}

        //public override void OnNavigatedTo(NavigationContext navigationContext)
        //{

        //}

        private void SubFlayoutsRequestNavigate(string uri, NavigationParameters parameters)
        {
            if (string.IsNullOrEmpty(uri))
                return;


            switch (uri)
            {
                case FlayoutsRegionNames.FlayotAdvancedBlocklySettings:
                    //RegionManager.RequestNavigate(FlayoutsRegionNames.FlayoutsInsideRegion, nameof(AdvancedBlocklySettingsView), parameters);
                    break;

            }
        }
    }
}

using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Controls.CustomControls.Services;
using AdamController.Core;
using Prism.Regions;

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

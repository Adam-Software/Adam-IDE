using AdamController.Core;
using AdamController.Core.Mvvm;
using AdamController.Modules.FlayoutsRegion.Views;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class FlayoutsViewModel : RegionViewModelBase
    {
        public FlayoutsViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {

        }

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            SubFlayoutsRequestNavigate(FlayoutsRegionNames.FlayotAdvancedBlocklySettings, navigationContext.Parameters);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        private void SubFlayoutsRequestNavigate(string uri, NavigationParameters parameters)
        {
            if (string.IsNullOrEmpty(uri))
                return;


            switch (uri)
            {
                case FlayoutsRegionNames.FlayotAdvancedBlocklySettings:
                    RegionManager.RequestNavigate(FlayoutsRegionNames.FlayoutsInsideRegion, nameof(AdvancedBlocklySettingsView), parameters);
                    break;

            }
        }
    }
}

using AdamController.Core;
using AdamController.Modules.FlayoutsRegion.Views;
using AdamController.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamController.Modules.FlayoutsRegion
{
    public class FlayoutsRegionModule : IModule
    {
        private readonly IRegionManager mRegionManager;
        private readonly IFlyoutManager mFlyoutManager;

        public FlayoutsRegionModule(IRegionManager regionManager, IFlyoutManager flyoutManager)
        {
            mRegionManager = regionManager;
            mFlyoutManager = flyoutManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //mRegionManager.RequestNavigate(RegionNames.FlayoutsRegion, nameof(FlayoutsView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

            //containerRegistry.RegisterForNavigation<FlayoutsView>(nameof(FlayoutsView));

            mFlyoutManager.RegisterFlyout<FlayoutsView>("FlayoutsView", RegionNames.FlayoutsRegion);
            //mFlyoutManager.RegisterFlyout<AdvancedBlocklySettingsView>("AdvancedBlocklySettingsView", FlayoutsRegionNames.FlayotAdvancedBlocklySettings);

            //containerRegistry.RegisterForNavigation<NotificationView>(nameof(NotificationView));
            //containerRegistry.RegisterForNavigation<AdvancedBlocklySettingsView>(nameof(AdvancedBlocklySettingsView));
        }
    }
}
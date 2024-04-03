using AdamController.Controls.CustomControls.Services;
using AdamController.Core;
using AdamController.Modules.FlayoutsRegion.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace AdamController.Modules.FlayoutsRegion
{
    public class FlayoutsRegionModule : IModule
    {
        private readonly IFlyoutManager mFlyoutManager;

        public FlayoutsRegionModule(IFlyoutManager flyoutManager)
        {
            mFlyoutManager = flyoutManager;
        }

        public void OnInitialized(IContainerProvider containerProvider){}

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            mFlyoutManager.RegisterFlyout<AdvancedBlocklySettingsView>(nameof(AdvancedBlocklySettingsView), RegionNames.FlayoutsRegion);
            mFlyoutManager.RegisterFlyout<NotificationView>(nameof(NotificationView), RegionNames.FlayoutsRegion);
        }
    }
}
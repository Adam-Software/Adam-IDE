using AdamStudio.Controls.CustomControls.Services;
using AdamStudio.Core;
using AdamStudio.Modules.FlayoutsRegion.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace AdamStudio.Modules.FlayoutsRegion
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
            mFlyoutManager.SetDefaultFlyoutRegion(RegionNames.FlayoutsRegion);

            mFlyoutManager.RegisterFlyoutWithDefaultRegion<NotificationView>(FlyoutNames.FlyoutNotification);
            mFlyoutManager.RegisterFlyoutWithDefaultRegion<PortSettingsView>(FlyoutNames.FlyoutPortSettings);
            mFlyoutManager.RegisterFlyoutWithDefaultRegion<UserFoldersSettingsView>(FlyoutNames.FlyoutUserFoldersSettings);
            mFlyoutManager.RegisterFlyoutWithDefaultRegion<WebApiSettingsView>(FlyoutNames.FlyoutWebApiSettings);
        }
    }
}
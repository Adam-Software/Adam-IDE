using AdamController.Core;
using AdamController.Modules.FlayoutsRegion.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamController.Modules.FlayoutsRegion
{
    public class FlayoutsRegionModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public FlayoutsRegionModule(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            mRegionManager.RequestNavigate(FlayoutsRegionNames.FlayoutsRegion, nameof(FlayoutsView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<FlayoutsView>();

            containerRegistry.RegisterForNavigation<NotificationView>();
            containerRegistry.RegisterForNavigation<AdvancedBlocklySettingsView>();
        }
    }
}
using AdamController.Core;
using AdamController.Modules.StatusBarRegion.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamController.Modules.StatusBarRegion
{
    public class StatusBarRegionModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public StatusBarRegionModule(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            mRegionManager.RequestNavigate(RegionNames.StatusBarRegion, nameof(StatusBarView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StatusBarView>();

            containerRegistry.RegisterForNavigation<StatusBarView>(nameof(StatusBarView));
        }
    }
}
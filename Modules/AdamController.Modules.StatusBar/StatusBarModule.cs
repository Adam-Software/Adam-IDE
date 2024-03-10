using AdamController.Core;
using AdamController.Modules.StatusBar.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamController.Modules.StatusBar
{
    public class StatusBarModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public StatusBarModule(IRegionManager regionManager)
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
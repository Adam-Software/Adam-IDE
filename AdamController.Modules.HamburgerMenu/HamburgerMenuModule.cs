using AdamController.Core;
using AdamController.Modules.HamburgerMenu.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamController.Modules.HamburgerMenu
{
    public class HamburgerMenuModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public HamburgerMenuModule(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            mRegionManager.RequestNavigate(RegionNames.HamburgerMenuRegion, nameof(HamburgerMenuView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<HamburgerMenuView>();

            containerRegistry.RegisterForNavigation<HamburgerMenuView>(nameof(HamburgerMenuView));
        }
    }
}
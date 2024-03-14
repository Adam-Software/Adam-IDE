using AdamController.Core;
using AdamController.Modules.HamburgerMenuRegion.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamController.Modules.HamburgerMenu
{
    public class HamburgerMenuRegionModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public HamburgerMenuRegionModule(IRegionManager regionManager)
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
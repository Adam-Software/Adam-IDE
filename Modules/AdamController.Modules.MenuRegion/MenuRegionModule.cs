﻿using AdamController.Core;
using AdamController.Modules.MenuRegion.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamController.Modules.MenuRegion
{
    public class MenuRegionModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public MenuRegionModule(IRegionManager regionManager) 
        {
            mRegionManager = regionManager;
        }


        public void OnInitialized(IContainerProvider containerProvider)
        {
            mRegionManager.RequestNavigate(RegionNames.MenuRegion, nameof(MenuRegionView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MenuRegionView>();

            containerRegistry.RegisterForNavigation<MenuRegionView>(nameof(MenuRegionView));
        }
    }
}
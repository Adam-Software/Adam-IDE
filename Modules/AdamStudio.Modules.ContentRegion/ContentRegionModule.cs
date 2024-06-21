﻿using AdamStudio.Core;
using AdamStudio.Modules.ContentRegion.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamStudio.Modules.ContentRegion
{
    public class ContentRegionModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public ContentRegionModule(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            mRegionManager.RequestNavigate(RegionNames.ContentRegion, nameof(ScratchControlView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ScratchControlView>(nameof(ScratchControlView));
            //containerRegistry.RegisterForNavigation<ComputerVisionControlView>(nameof(ComputerVisionControlView));
            containerRegistry.RegisterForNavigation<SettingsControlView>(nameof(SettingsControlView));
        }
    }
}
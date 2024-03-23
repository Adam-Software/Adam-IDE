using AdamController.Core;
using AdamController.Modules.ContentRegion.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AdamController.Modules.ContentRegion
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
            mRegionManager.RequestNavigate(RegionNames.ContentRegion, nameof(ContentRegionView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<ContentRegionView>();
            containerRegistry.RegisterForNavigation<ContentRegionView>(nameof(ContentRegionView));

            containerRegistry.RegisterForNavigation<ScratchControlView>(nameof(ScratchControlView));
            containerRegistry.RegisterForNavigation<ComputerVisionControlView>(nameof(ComputerVisionControlView));
            containerRegistry.RegisterForNavigation<ScriptEditorControlView>(nameof(ScriptEditorControlView));
            containerRegistry.RegisterForNavigation<VisualSettingsControlView>(nameof(VisualSettingsControlView));
        }
    }
}
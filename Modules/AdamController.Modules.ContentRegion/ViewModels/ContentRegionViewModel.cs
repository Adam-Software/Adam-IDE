using AdamStudio.Core;
using AdamStudio.Core.Mvvm;
using AdamStudio.Modules.ContentRegion.Views;
using AdamStudio.Services.Interfaces;
using Prism.Regions;
using System;

namespace AdamStudio.Modules.ContentRegion.ViewModels
{
    public class ContentRegionViewModel : RegionViewModelBase
    {
        private ISubRegionChangeAwareService RegionChangeAwareService { get; }

        public ContentRegionViewModel(IRegionManager regionManager, ISubRegionChangeAwareService regionChangeAwareService) : base(regionManager)
        {
            RegionChangeAwareService = regionChangeAwareService;
        }

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (navigationContext.NavigationService.Region.Name == RegionNames.ContentRegion)
            {
                string insideRegionName = navigationContext.Uri.OriginalString;

                RegionChangeAwareService.InsideRegionNavigationRequestName = insideRegionName;
                SubRegionsRequestNavigate(insideRegionName, navigationContext.Parameters);
            }

            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

        }

        public override void Destroy()
        {
            base.Destroy();
        }

        #endregion

        #region Private methods

        private void SubRegionsRequestNavigate(string uri, NavigationParameters parameters)
        {
            if (string.IsNullOrEmpty(uri))
                return;

            
            switch (uri)
            {
                case SubRegionNames.SubRegionScratch:
                    
                    RegionManager.RequestNavigate(SubRegionNames.InsideConentRegion, nameof(ScratchControlView), parameters);
                    break;

                case SubRegionNames.SubRegionComputerVisionControl:
                    RegionManager.RequestNavigate(SubRegionNames.InsideConentRegion, nameof(ComputerVisionControlView), parameters);
                    break;

                case SubRegionNames.SubRegionVisualSettings:
                    RegionManager.RequestNavigate(SubRegionNames.InsideConentRegion, nameof(SettingsControlView), parameters);
                    break;

            }
        }

        #endregion
    }
}

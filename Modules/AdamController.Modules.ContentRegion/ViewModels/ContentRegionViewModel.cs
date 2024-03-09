using AdamController.Core.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ContentRegionViewModel : RegionViewModelBase
    {
        public ContentRegionViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
        }
    }
}

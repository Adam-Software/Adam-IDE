using AdamController.Core.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class FlayoutsViewModel : RegionViewModelBase
    {
        public FlayoutsViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
        }
    }
}

using AdamController.Core.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.ContentRegion.ViewModels.Flayouts
{
    public class NotificationViewModel : RegionViewModelBase
    {
        public NotificationViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
        }
    }
}

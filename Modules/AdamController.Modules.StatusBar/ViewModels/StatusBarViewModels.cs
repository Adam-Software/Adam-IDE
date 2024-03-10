using AdamController.Core.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.StatusBar.ViewModels
{
    public class StatusBarViewModels : RegionViewModelBase
    {
        public StatusBarViewModels(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
        }
    }
}

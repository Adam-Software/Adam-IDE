using AdamController.Core.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.MenuRegion.ViewModels
{
    public class MenuRegionViewModel : RegionViewModelBase
    {
        public MenuRegionViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
        }

        //public static RelayCommand ShowSettingsWindow => new(obj =>
        //{
        //    new WindowShowerHelpers(new SettingsWindow(), new SettingsWindowView()).Show();
        //});


        //public static RelayCommand ExitAppCommand => new(obj =>
        //{
        //    Application.Current.Shutdown();
        //});

    }
}

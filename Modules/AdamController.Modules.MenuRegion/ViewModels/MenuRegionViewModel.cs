using AdamController.Core.Mvvm;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;

namespace AdamController.Modules.MenuRegion.ViewModels
{
    public class MenuRegionViewModel : RegionViewModelBase
    {

        public DelegateCommand CloseAppCommand { get; }

        public MenuRegionViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
            CloseAppCommand = new DelegateCommand(Application.Current.Shutdown);
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

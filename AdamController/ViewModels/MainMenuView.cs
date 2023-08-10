using AdamController.Commands;
using AdamController.Helpers;
using AdamController.Views;
using AdamController.Views.Window;
using System.Windows;

namespace AdamController.ViewModels
{
    public class MainMenuView
    {
        
        #region Show window

        public static RelayCommand ShowSettingsWindow => new(obj =>
        {
            new WindowShowerHelpers(new SettingsWindow(), new SettingsWindowView()).Show();
        });

        /*public static RelayCommand ShowNetworkTestWindow => new(obj => 
        {
            new WindowShowerHelpers(new NetworkTestWindow(), new NetworkTestView()).Show();
        });*/

        #endregion

        public static RelayCommand ExitAppCommand => new(obj =>
        {
            Application.Current.Shutdown();
        });

    }
}

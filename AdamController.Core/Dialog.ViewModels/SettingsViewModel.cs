using AdamController.Core.Mvvm;

namespace AdamController.Core.Dialog.ViewModels
{
    /// <summary>
    /// The template for calling a dialog box through a service
    /// </summary>
    public class SettingsViewModel : DialogViewModelBase
    {
        /*  For Calling from MenuRegion:
         *  
         *  MenuRegionViewModel:
         *  public DelegateCommand<string> ShowDialogCommand { get; }
         *  ShowDialogCommand = new DelegateCommand<string>(ShowDialog);
         *  
         *  private void ShowDialog(string dialogNames)
         *  {
         *      switch (dialogNames)
         *      {
         *          case DialogNames.SettingsDialog:
         *              DialogService.ShowSettingsDialog();
         *              break;
         *      }
         *   }
         *   
         *  MenuRegionView:
         *  <MenuItem Header="{DynamicResource MainMenu.Tools.MainHeader}"
         *           Style="{DynamicResource MahApps.Styles.MenuItem}"
         *           Background="Transparent">
         *
         *     <MenuItem Header="{DynamicResource MainMenu.Tools.SettingsHeader}" 
         *               Command="{Binding ShowDialogCommand}"
         *               CommandParameter="{x:Static core:DialogNames.SettingsDialog}"/>
         *
         * </MenuItem>
         * 
         * In DialogNames:
         * public const string SettingsDialog = nameof(SettingsView);
         * 
         * In DialogServiceExtensions
         * public static class DialogServiceExtensions
         * {
         *      public static void ShowSettingsDialog(this IDialogService dialogService)
         *      {
         *          dialogService.ShowDialog(nameof(SettingsView));
         *      }
         *  }
         *  
         * 
         */
    }
}

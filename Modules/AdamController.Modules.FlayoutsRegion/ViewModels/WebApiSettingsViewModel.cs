using AdamStudio.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamStudio.Services.Interfaces;
using System.Windows;

namespace AdamStudio.Modules.FlayoutsRegion.ViewModels
{
    public class WebApiSettingsViewModel : FlyoutBase
    {
        private readonly ICultureProvider mCultureProvider;

        public WebApiSettingsViewModel(ICultureProvider cultureProvider) 
        {
            mCultureProvider = cultureProvider; 
            BorderThickness = 1;   
        }

        #region Navigation

        protected override void OnChanging(bool isOpening)
        {
            if(isOpening)
            {
                Header = mCultureProvider.FindResource("WebApiSettingsView.ViewModel.Flyout.Header");
                BorderBrush = Application.Current.TryFindResource("MahApps.Brushes.Text").ToString();
                return;
            }
        }

        #endregion
    }
}

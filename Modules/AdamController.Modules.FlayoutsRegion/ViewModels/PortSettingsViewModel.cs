using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Services.Interfaces;
using System.Windows;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class PortSettingsViewModel : FlyoutBase
    {

        #region Services

        private readonly ICultureProvider mCultureProvider;

        #endregion

        public PortSettingsViewModel(ICultureProvider cultureProvider) 
        {
            BorderThickness = 1;
            mCultureProvider = cultureProvider;
        }

        protected override void OnChanging(bool isOpening)
        {
            if (isOpening)
            {
                Header = mCultureProvider.FindResource("PortSettingsView.ViewModel.Flyout.Header");
                BorderBrush = Application.Current.TryFindResource("MahApps.Brushes.Text").ToString();
                return;
            }
        }
    }
}

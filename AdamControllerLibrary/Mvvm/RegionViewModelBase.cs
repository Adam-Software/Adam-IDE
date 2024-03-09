using Prism.Regions;
using Prism.Services.Dialogs;
using System;


namespace AdamController.Core.Mvvm
{
    public class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest
    {

        #region private service

        protected IRegionManager RegionManager { get; }
        protected IDialogService DialogService { get; }

        #endregion

        #region ~

        public RegionViewModelBase(IRegionManager regionManager, IDialogService dialogService)
        {
            RegionManager = regionManager;
            DialogService = dialogService;
        }

        #endregion

        /// <summary>
        /// Occurs when the navigation area is called
        /// </summary>
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback?.Invoke(true);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// On close region
        /// </summary>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// On load region
        /// </summary>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}

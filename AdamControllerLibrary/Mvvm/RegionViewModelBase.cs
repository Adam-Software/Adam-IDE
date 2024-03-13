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
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback?.Invoke(true);
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// On close region
        /// </summary>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// On load region
        /// </summary>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}

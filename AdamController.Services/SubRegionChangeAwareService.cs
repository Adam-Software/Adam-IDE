using AdamController.Services.Interfaces;
using Prism.Mvvm;

namespace AdamController.Services
{
    public class SubRegionChangeAwareService : BindableBase, ISubRegionChangeAwareService
    {
        #region Events

        public event SubRegionChangeEventHandler RaiseSubRegionChangeEvent;

        #endregion

        #region ~
        public SubRegionChangeAwareService() { }

        #endregion

        #region Public fields

        private string insideRegionNavigationRequestName;
        public string InsideRegionNavigationRequestName
        {
            get { return insideRegionNavigationRequestName; }
            set
            {
                bool isNewValue = SetProperty(ref insideRegionNavigationRequestName, value);
                
                if (isNewValue) 
                    OnRaiseRegionChangeEvent();
            }
        }

        public void Dispose() { }

        #endregion

        #region OnRaise events

        protected virtual void OnRaiseRegionChangeEvent()
        {
            SubRegionChangeEventHandler raiseEvent = RaiseSubRegionChangeEvent;
            raiseEvent?.Invoke(this);
        }

        #endregion

    }
}

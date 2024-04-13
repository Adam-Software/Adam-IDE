using AdamController.Services.Interfaces;
using Prism.Mvvm;

namespace AdamController.Services
{
    public class SubRegionChangeAwareService : BindableBase, ISubRegionChangeAwareService
    {
        public SubRegionChangeAwareService() { }

        public event SubRegionChangeEventHandler RaiseSubRegionChangeEvent;

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

        protected virtual void OnRaiseRegionChangeEvent()
        {
            SubRegionChangeEventHandler raiseEvent = RaiseSubRegionChangeEvent;
            raiseEvent?.Invoke(this);
        }

        public void Dispose(){}
    }
}

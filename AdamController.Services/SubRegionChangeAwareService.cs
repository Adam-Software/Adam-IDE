using AdamController.Services.Interfaces;

namespace AdamController.Services
{
    public class SubRegionChangeAwareService : ISubRegionChangeAwareService
    {
        public SubRegionChangeAwareService() { }

        public event SubRegionChangeEventHandler RaiseSubRegionChangeEvent;

        private string mRegionNavigtedToName;
        public string InsideRegionNavigationRequestName
        {
            get { return mRegionNavigtedToName; }
            set
            {
                mRegionNavigtedToName = value;
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

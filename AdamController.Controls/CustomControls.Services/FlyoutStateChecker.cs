
namespace AdamController.Controls.CustomControls.Services
{
    public class FlyoutStateChecker : IFlyoutStateChecker
    {
        public event IsOpenedStateChangeEventHandler IsOpenedStateChangeEvent;

        private bool _isFlyoutEnabled;
        public bool IsOpened 
        { 
            get {  return _isFlyoutEnabled; }
            set 
            {  
                if (_isFlyoutEnabled == value) 
                    return;

                _isFlyoutEnabled = value;
                OnOpenedStateChangeEvent();
            } 
        }

        protected void OnOpenedStateChangeEvent()
        {
            IsOpenedStateChangeEventHandler raiseEvent = IsOpenedStateChangeEvent;
            raiseEvent?.Invoke(this);
        }

    }
}

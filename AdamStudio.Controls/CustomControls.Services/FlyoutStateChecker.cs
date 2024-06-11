
namespace AdamStudio.Controls.CustomControls.Services
{
    public class FlyoutStateChecker : IFlyoutStateChecker
    {
        public event IsNotificationFlyoutOpenedStateChangeEventHandler IsNotificationFlyoutOpenedStateChangeEvent;

        private bool isNotificationFlyoutOpened;
        public bool IsNotificationFlyoutOpened 
        { 
            get {  return isNotificationFlyoutOpened; }
            set 
            {  
                if (isNotificationFlyoutOpened == value) 
                    return;

                isNotificationFlyoutOpened = value;
                OnNotificationFlyoutOpenedStateChangeEvent();
            } 
        }

        protected void OnNotificationFlyoutOpenedStateChangeEvent()
        {
            IsNotificationFlyoutOpenedStateChangeEventHandler raiseEvent = IsNotificationFlyoutOpenedStateChangeEvent;
            raiseEvent?.Invoke(this);
        }
    }
}

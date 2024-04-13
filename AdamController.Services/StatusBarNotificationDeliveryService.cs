using AdamController.Services.Interfaces;
using Prism.Mvvm;


namespace AdamController.Services
{
    public class StatusBarNotificationDeliveryService : BindableBase, IStatusBarNotificationDeliveryService
    {

        #region Events

        public event ChangeProgressRingStateEventHandler RaiseChangeProgressRingStateEvent;
        public event NewCompileLogMessageEventHandler RaiseNewCompileLogMessageEvent;
        public event NewAppLogMessageEventHandler RaiseNewAppLogMessageEvent;
        public event NewNotificationBadgeMessageEventHandler RaiseNewNotificationBadgeMessageEvent;

        #endregion

        #region ~

        public StatusBarNotificationDeliveryService() { }

        #endregion

        #region Public fields

        private bool progressRingStart;
        public bool ProgressRingStart 
        { 
            get =>   progressRingStart; 
            set 
            { 
                bool isNewValue = SetProperty(ref progressRingStart, value);

                if (isNewValue)
                    OnRaiseChangeProgressRingStateEvent(ProgressRingStart);
                
            }
        }

        private string compileLogMessage = string.Empty;
        public string CompileLogMessage 
        {
            get => compileLogMessage; 
            set 
            { 
                bool isNewValue = SetProperty(ref compileLogMessage, value);
                
                if (isNewValue)
                    OnRaiseNewCompileLogMessageEvent(CompileLogMessage);
            }
        }
       
        private string appLogMessage = string.Empty;
        public string AppLogMessage 
        { 
            get => appLogMessage; 
            set 
            {
                bool isNewValue = SetProperty(ref appLogMessage, value);

                if (isNewValue)
                    OnRaiseNewAppLogMessageEvent(AppLogMessage);
            } 
        }

        private string notificationBadgeMessage = string.Empty;
        public string NotificationBadgeMessage 
        { 
            get => notificationBadgeMessage;
            set
            {
                bool isNewValue = SetProperty(ref notificationBadgeMessage, value);

                if (isNewValue)
                    OnRaiseNewNotificationBadgeMessageEvent(NotificationBadgeMessage);
            } 
        }

        public void Dispose()
        {
            
        }

        #endregion


        #region OnRaise methods

        protected virtual void OnRaiseChangeProgressRingStateEvent(bool newState)
        {
            ChangeProgressRingStateEventHandler raiseEvent = RaiseChangeProgressRingStateEvent;
            raiseEvent?.Invoke(this, newState);
        }

        protected virtual void OnRaiseNewCompileLogMessageEvent(string message) 
        {
            NewCompileLogMessageEventHandler raiseEvent = RaiseNewCompileLogMessageEvent;
            raiseEvent?.Invoke(this, message);
        }

        protected virtual void OnRaiseNewAppLogMessageEvent(string message)
        {
            NewAppLogMessageEventHandler raiseEvent = RaiseNewAppLogMessageEvent;
            raiseEvent?.Invoke(this, message);
        }

        protected virtual void OnRaiseNewNotificationBadgeMessageEvent(string message)
        {
            NewNotificationBadgeMessageEventHandler raiseEvent = RaiseNewNotificationBadgeMessageEvent;
            raiseEvent?.Invoke(this, message);
        }

        #endregion
    }
}

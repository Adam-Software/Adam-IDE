using AdamController.Services.Interfaces;
using Prism.Mvvm;


namespace AdamController.Services
{
    public class StatusBarNotificationDeliveryService : BindableBase, IStatusBarNotificationDeliveryService
    {
        public event NewCompileLogMessageEventHandler RaiseNewCompileLogMessageEvent;
        public StatusBarNotificationDeliveryService() { }

        private string compileLogMessage = string.Empty;
        public string CompileLogMessage 
        {
            get { return compileLogMessage; }
            set 
            { 
                bool isNewValue = SetProperty(ref compileLogMessage, value);
                
                if (isNewValue)
                    OnRaiseNewCompileLogMessageEvent(CompileLogMessage);
            }
        }

        protected virtual void OnRaiseNewCompileLogMessageEvent(string message) 
        {
            NewCompileLogMessageEventHandler raiseEvent = RaiseNewCompileLogMessageEvent;
            raiseEvent.Invoke(this, message);

        }
    }
}

namespace AdamController.Services.Interfaces
{
    public delegate void NewCompileLogMessageEventHandler(object sender, string message);

    public interface IStatusBarNotificationDeliveryService
    {
        public event NewCompileLogMessageEventHandler RaiseNewCompileLogMessageEvent;

        public string CompileLogMessage { get; set; }

    }
}

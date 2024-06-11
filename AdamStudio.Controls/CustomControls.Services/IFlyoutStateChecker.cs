namespace AdamStudio.Controls.CustomControls.Services
{
    public delegate void IsNotificationFlyoutOpenedStateChangeEventHandler(object sender);

    public interface IFlyoutStateChecker
    {
        public event IsNotificationFlyoutOpenedStateChangeEventHandler IsNotificationFlyoutOpenedStateChangeEvent;

        public bool IsNotificationFlyoutOpened { get; set; }
    }
}

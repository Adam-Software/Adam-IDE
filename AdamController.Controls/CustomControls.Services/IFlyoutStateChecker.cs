namespace AdamController.Controls.CustomControls.Services
{
    public delegate void IsOpenedStateChangeEventHandler(object sender);

    public interface IFlyoutStateChecker
    {
        public event IsOpenedStateChangeEventHandler IsOpenedStateChangeEvent;

        public bool IsOpened { get; set; }
    }
}

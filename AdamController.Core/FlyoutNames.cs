namespace AdamController.Core
{
    /// <summary>
    /// The flyout name must match the view
    /// Binding to the viewmodel must be enabled in the view (prism:ViewModelLocator.AutoWireViewModel="True")
    /// </summary>
    public class FlyoutNames
    {
        public const string FlyoutNotification = "NotificationView";
        public const string FlyoutPortSettings = "PortSettingsView";
        public const string FlyoutWebApiSettings = "WebApiSettingsView";
        
    }
}

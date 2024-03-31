using System;

namespace AdamController.Services.FlayoutsRegionEventAwareServiceDependency
{
    public class FlyoutEventArgs : EventArgs
    {
        public FlyoutAction FlyoutAction { get; set; }

        public FlyoutEventArgs(FlyoutAction flyoutAction)
        {
            FlyoutAction = flyoutAction;
        }
    }
}

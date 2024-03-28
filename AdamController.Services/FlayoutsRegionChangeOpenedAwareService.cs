using AdamController.Services.Interfaces;

namespace AdamController.Services
{
    public class FlayoutsRegionChangeOpenedAwareService : IFlayoutsRegionChangeOpenedAwareService
    {
        public event AdvancedBlocklySettingsIsOpenChangeEventHandler RaiseAdvancedBlocklySettingsIsOpenChange;
 

        private bool mAdvancedBlocklySettingsIsOpen;
        public bool AdvancedBlocklySettingsIsOpen
        { 
            get { return mAdvancedBlocklySettingsIsOpen; } 
            set 
            {
                mAdvancedBlocklySettingsIsOpen = value;
                OnRaiseAdvancedBlocklySettingsIsOpenChange();
            }
        }

        private bool mNotificationFlayoutsIsOpen;

        public bool NotificationFlayoutsIsOpen 
        { 
            get { return mNotificationFlayoutsIsOpen; } 
            set
            {
                mNotificationFlayoutsIsOpen = value;
            } 
        }


        protected virtual void OnRaiseAdvancedBlocklySettingsIsOpenChange()
        {
            AdvancedBlocklySettingsIsOpenChangeEventHandler raiseEvent = RaiseAdvancedBlocklySettingsIsOpenChange;
            raiseEvent?.Invoke(this);

        }


    }
}

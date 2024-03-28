namespace AdamController.Services.Interfaces
{
    public delegate void AdvancedBlocklySettingsIsOpenChangeEventHandler(object sender);

    public interface IFlayoutsRegionChangeOpenedAwareService
    {
        public event AdvancedBlocklySettingsIsOpenChangeEventHandler RaiseAdvancedBlocklySettingsIsOpenChange;
        public bool AdvancedBlocklySettingsIsOpen { get; set; }
        public bool NotificationFlayoutsIsOpen { get; set; }
    }
}

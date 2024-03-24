namespace AdamController.Services.Interfaces
{
    public delegate void SubRegionChangeEventHandler(object sender);

    public interface ISubRegionChangeAwareService
    {
        public event SubRegionChangeEventHandler RaiseSubRegionChangeEvent;
        public string InsideRegionNavigationRequestName { get; set; }
    }
}

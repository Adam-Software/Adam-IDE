using System;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void SubRegionChangeEventHandler(object sender);

    #endregion

    public interface ISubRegionChangeAwareService : IDisposable
    {
        #region Events

        public event SubRegionChangeEventHandler RaiseSubRegionChangeEvent;

        #endregion

        #region Public fields

        public string InsideRegionNavigationRequestName { get; set; }

        #endregion
    }
}

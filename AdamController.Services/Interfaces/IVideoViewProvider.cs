using System;

namespace AdamController.Services.Interfaces
{
    #region Delegates

    public delegate void FrameRateUpdateEventHandler(object sender);

    #endregion


    public interface IVideoViewProvider : IDisposable
    {
        #region Events

        public event FrameRateUpdateEventHandler RaiseFrameRateUpdateEvent;

        #endregion

        #region public field

        public double FrameRate { get; set; }

        #endregion

        #region public methods

        public void ClearFrameRate();

        #endregion
    }
}

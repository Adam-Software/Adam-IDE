using AdamController.Services.Interfaces;
using Prism.Mvvm;

namespace AdamController.Services
{
    public class VideoViewProvider : BindableBase, IVideoViewProvider
    {
        #region Events

        public event FrameRateUpdateEventHandler RaiseFrameRateUpdateEvent;

        #endregion

        private double frameRate = double.NaN;

        // set frame rate in view, get frame rate in view model
        public double FrameRate
        {
            get { return frameRate; }
            set 
            { 
                var isNewValue = SetProperty(ref frameRate, value); 

                if(isNewValue)
                    OnRaiseFrameRateUpdateEvent();
            }
        }

        #region OnRaise events

        protected virtual void OnRaiseFrameRateUpdateEvent()
        {
            FrameRateUpdateEventHandler raiseEvent = RaiseFrameRateUpdateEvent;
            raiseEvent?.Invoke(this);
        }

        public void ClearFrameRate()
        {
            FrameRate = double.NaN;
        }

        public void Dispose()
        {
            
        }

        #endregion

    }
}

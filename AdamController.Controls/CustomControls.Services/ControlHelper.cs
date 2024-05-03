using Prism.Mvvm;

namespace AdamController.Controls.CustomControls.Services
{
    public class ControlHelper : BindableBase, IControlHelper
    {
        public ControlHelper() { }

        private double actualWidth;
        public double ActualWidth
        {
            get { return actualWidth; } 
            set { SetProperty(ref actualWidth, value); }
        }


        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}

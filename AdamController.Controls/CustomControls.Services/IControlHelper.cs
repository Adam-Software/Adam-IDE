using AdamController.Controls.Enums;

namespace AdamController.Controls.CustomControls.Services
{
    public interface IControlHelper : IDisposable
    {
        public double MainGridActualWidth { get; set; }
        public double BlocklyColumnActualWidth { get; set; }
        public BlocklyViewMode CurrentBlocklyViewMode { get;  }
    }
}

using AdamController.Controls.Enums;

namespace AdamController.Controls.CustomControls.Services
{
    #region Delegates

    public delegate void BlocklyColumnWidthChangeEventHandler(object sender);

    #endregion

    public interface IControlHelper : IDisposable
    {
        #region Events

        public event BlocklyColumnWidthChangeEventHandler RaiseBlocklyColumnWidthChangeEvent;

        #endregion

        public double MainGridActualWidth { get; set; }
        public double BlocklyColumnActualWidth { get; set; }
        public double BlocklyColumnWidth { get; set; }
        public BlocklyViewMode CurrentBlocklyViewMode { get; set; }
    }
}

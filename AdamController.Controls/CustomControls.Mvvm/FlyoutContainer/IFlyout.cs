using System.Drawing;
using System.Windows.Input;

namespace AdamController.Controls.CustomControls.Mvvm.FlyoutContainer
{
    public interface IFlyout
    {
        public event EventHandler<FlyoutEventArgs> OnClosed;

        public event EventHandler<FlyoutEventArgs> OnOpened;

        public event EventHandler<FlyoutEventArgs> OnOpenChanged;

        public double BorderThickness { get; set; }

        public Color BorderBrush { get; set; }  

        public string Position { get; set; }

        public string Header { get; set; }

        public string Theme { get; set; }

        public bool IsModal { get; set; }

        public bool AreAnimationsEnabled { get; set; }

        public bool AnimateOpacity { get; set; }

        public ICommand CloseCommand { get; set; }

        public MouseButton ExternalCloseButton { get; set; }

        public bool CloseButtonIsCancel { get; set; }

        public bool IsPinned { get; set; }

        public bool CanClose(FlyoutParameters flyoutParameters);

        public void Close(FlyoutParameters flyoutParameters);

        public void Close();

        public bool CanOpen(FlyoutParameters flyoutParameters);

        public void Open(FlyoutParameters flyoutParameters);

        public void Open();
    }
}

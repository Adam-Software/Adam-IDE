using AdamController.Controls.Enums;
using MahApps.Metro.Controls;
using Prism.Mvvm;

namespace AdamController.Controls.CustomControls.Services
{
    public class ControlHelper : BindableBase, IControlHelper
    {
        public ControlHelper() { }

        private double mainGridActualWidth;
        public double MainGridActualWidth
        {
            get { return mainGridActualWidth; } 
            set 
            {
                SetProperty(ref mainGridActualWidth, value);
                UpdateCurrentBlocklyViewMode();
            }
        }

        private double blocklyColumnActualWidth;
        public double BlocklyColumnActualWidth
        {
            get { return blocklyColumnActualWidth;}
            set 
            { 
                SetProperty(ref blocklyColumnActualWidth, value);
                UpdateCurrentBlocklyViewMode();

            }
        }

        private BlocklyViewMode currentBlocklyViewMode;
        public BlocklyViewMode CurrentBlocklyViewMode 
        {
            get => currentBlocklyViewMode;
            private set => SetProperty(ref currentBlocklyViewMode, value);
        }
        

        private void UpdateCurrentBlocklyViewMode()
        {
            double mainGridWidth = MainGridActualWidth;
            double blocklyColumnWidth = BlocklyColumnActualWidth;

            if (BlocklyColumnActualWidth >= MainGridActualWidth - 10)
            {
                CurrentBlocklyViewMode = BlocklyViewMode.FullScreen;
                return;
            }

            if (BlocklyColumnActualWidth == 0)
            {
                CurrentBlocklyViewMode = BlocklyViewMode.Hidden;
                return;
            }

            CurrentBlocklyViewMode = BlocklyViewMode.MiddleScreen;
        }

        public void Dispose()
        {
        }
    }
}

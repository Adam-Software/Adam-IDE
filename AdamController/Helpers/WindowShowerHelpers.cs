using AdamController.Interface;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace AdamController.Helpers
{
    public class WindowShowerHelpers
    {
        private readonly MetroWindow mWindow;
        private readonly bool mIsModal;

        public WindowShowerHelpers(MetroWindow window, object dataContext)
        {
            IWindowParam param = dataContext as IWindowParam ?? new DefaultWindowParam();
            mIsModal = param.IsModal;
            
            mWindow = window;
            mWindow.DataContext = dataContext;
            mWindow.Title = param.WindowTitle;
            mWindow.Height = param.Height;
            mWindow.Width = param.Width;
            mWindow.ResizeMode = param.ResizeMode;
            mWindow.WindowStartupLocation = param.WindowStartupLocation;
            mWindow.TitleCharacterCasing = param.TitleCharacterCasing;
            mWindow.WindowState = param.WindowState;
            
            mWindow.Closed += (sender, e) => param.OnClosed(window);
        }

        public WindowShowerHelpers(UserControl userControl, object dataContext)
        {
            IWindowParam param = dataContext as IWindowParam ?? new DefaultWindowParam();
            mIsModal = param.IsModal;

            MetroWindow window = new()
            {
                Content = userControl,
                DataContext = dataContext,
                Title = param.WindowTitle,
                Width = param.Width,
                Height = param.Height,
                ResizeMode = param.ResizeMode,
                WindowStartupLocation = param.WindowStartupLocation,
                TitleCharacterCasing = param.TitleCharacterCasing,
                WindowState = param.WindowState
                
            };

            window.Closed += (sender, e) => param.OnClosed(window);
            mWindow = window;
        }

        public WindowShowerHelpers(UserControl userControl, params object[] dataContext)
        {
            IWindowParam param = dataContext[0] as IWindowParam ?? new DefaultWindowParam();
            mIsModal = param.IsModal;

            MetroWindow window = new()
            {
                Content = userControl,
                DataContext = dataContext,
                Title = param.WindowTitle,
                Width = param.Width,
                Height = param.Height,
                ResizeMode = param.ResizeMode,
                WindowStartupLocation = param.WindowStartupLocation,
                TitleCharacterCasing = param.TitleCharacterCasing,
                WindowState = param.WindowState
            };

            window.Closed += (sender, e) => param.OnClosed(window);
            mWindow = window;
        }

        #region Show

        public void Show()
        {
            if (mWindow == null)
            {
                return;
            }

            if (mIsModal)
            {
                try
                {
                    _ = mWindow.ShowDialog();
                }
                catch
                {

                }

            }
            else
            {
                try
                {
                    mWindow.Show();
                }
                catch
                {

                }

            }
        }

        #endregion
    }

    internal class DefaultWindowParam : IWindowParam
    {
        public string WindowTitle => @"Заголовок окна не переопределен";
        public bool IsModal => true;
        public double Height => 800;
        public double Width => 1200;
        public ResizeMode ResizeMode => ResizeMode.CanResize;
        public WindowStartupLocation WindowStartupLocation => WindowStartupLocation.CenterScreen;
        public CharacterCasing TitleCharacterCasing => CharacterCasing.Normal;
        public WindowState WindowState => WindowState.Normal;

        public void OnClosed(Window window)
        {
            if (window == null)
            {
                return;
            }

            window.Close();
        }
    }
}

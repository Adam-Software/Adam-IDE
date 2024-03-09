using AdamController.Interface;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace AdamController.Helpers
{
    public class WindowShowerHelpers
    {
        private readonly bool mIsModal;

        public WindowShowerHelpers(MetroWindow window, object dataContext)
        {
            IWindowParam param = dataContext as IWindowParam ?? new DefaultWindowParam();
            mIsModal = param.IsModal;
            
            Window = window;
            Window.DataContext = dataContext;
            Window.Title = param.WindowTitle;
            Window.Height = param.Height;
            Window.Width = param.Width;
            Window.ResizeMode = param.ResizeMode;
            Window.WindowStartupLocation = param.WindowStartupLocation;
            Window.TitleCharacterCasing = param.TitleCharacterCasing;
            Window.WindowState = param.WindowState;
            
            Window.Closed += (sender, e) => param.OnClosed(window);
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
            Window = window;
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
            Window = window;
        }

        public readonly MetroWindow Window;

        #region Show

        public void Show()
        {
            if (Window == null)
            {
                return;
            }

            if (mIsModal)
            {
                try
                {
                    _ = Window.ShowDialog();
                }
                catch
                {

                }

            }
            else
            {
                try
                {
                    Window.Show();
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

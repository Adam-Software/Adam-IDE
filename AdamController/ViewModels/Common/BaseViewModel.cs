using AdamController.Interface;
using System.Windows;
using System.Windows.Controls;

namespace AdamController.ViewModels.Common
{
    public abstract class BaseViewModel : BindableBase, IWindowParam
    {
        #region IWindowParam interface

        /// <summary>
        /// Window title.
        /// </summary>
        public abstract string WindowTitle { get; }

        /// <summary>
        /// Default window is modal. Ovverride this to false if window main.
        /// </summary>
        public virtual bool IsModal => true;
        public virtual double Height => 1000;
        public virtual double Width => 1400;

        /// <summary>
        /// Determines whether the window can be resized
        /// </summary>
        public virtual ResizeMode ResizeMode => ResizeMode.CanResize;
        public virtual WindowStartupLocation WindowStartupLocation => WindowStartupLocation.CenterScreen;
        public virtual CharacterCasing TitleCharacterCasing => CharacterCasing.Normal;
        public virtual WindowState WindowState => WindowState.Normal;

        /// <summary>
        /// This method is executed when the window is closed
        /// </summary>
        public virtual void OnClosed(Window window)
        {
            if (window == null)
            {
                return;
            }

            window.Close();
        }

        #endregion
    }
}

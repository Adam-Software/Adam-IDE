using System.Windows;
using System.Windows.Controls;

namespace AdamController.Interface
{
    public interface IWindowParam
    {
        double Height { get; }

        double Width { get; }

        string WindowTitle { get; }

        bool IsModal { get; }

        ResizeMode ResizeMode { get; }

        WindowStartupLocation WindowStartupLocation { get; }

        WindowState WindowState { get; }
        /// <summary>
        /// Title charster style
        /// </summary>
        CharacterCasing TitleCharacterCasing { get; }

        /// <summary>
        /// This method is executed when the window is closed
        /// </summary>
        abstract void OnClosed(Window window);
    }
}

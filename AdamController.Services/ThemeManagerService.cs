using AdamController.Services.Interfaces;
using ControlzEx.Theming;
using System.Collections.ObjectModel;

namespace AdamController.Services
{
    public class ThemeManagerService : IThemeManagerService
    {
        //ThemeManager mCurrentThemeManager;

        #region ~

        public ThemeManagerService(int a) 
        {
            //mCurrentThemeManager = ThemeManager.Current;

            //ReadOnlyObservableCollection<Theme> themesCollection = mCurrentThemeManager.Themes;
            //ReadOnlyObservableCollection<string> colorSchemes = mCurrentThemeManager.ColorSchemes;
            //ReadOnlyObservableCollection<string> baseColorsCollection = mCurrentThemeManager.BaseColors;
            //ReadOnlyObservableCollection<LibraryThemeProvider> themeProviders = mCurrentThemeManager.LibraryThemeProviders;

            var s = a;
            
        }

        #endregion


        #region Public methods

        public void Dispose()
        {
            
        }

        #endregion
    }
}

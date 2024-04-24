using AdamController.Services.Interfaces;
using ControlzEx.Theming;
using System.Collections.ObjectModel;
using System.Windows;

namespace AdamController.Services
{
    public class ThemeManagerService : IThemeManagerService
    {
        private readonly Application mCurrentApplication;
        private readonly ThemeManager mCurrentThemeManager;
        
        #region ~

        public ThemeManagerService() 
        {
            mCurrentApplication = Application.Current;
            mCurrentThemeManager = ThemeManager.Current;
            AppThemesCollection = mCurrentThemeManager.Themes;
        }

        #endregion

        #region Public filelds

        public ReadOnlyObservableCollection<Theme> AppThemesCollection {  get; private set; }

        #endregion

        #region Public methods

        public Theme ChangeAppTheme(string themeName) 
        {
            var isThemeExist = mCurrentThemeManager.GetTheme(themeName) != null;

            if (isThemeExist)
                return mCurrentThemeManager.ChangeTheme(mCurrentApplication, themeName, false);

            return null;
        }

        public Theme ChangeAppTheme(Theme themeName)
        {
            return mCurrentThemeManager.ChangeTheme(mCurrentApplication, themeName.Name, false);
        }

        public Theme GetCurrentAppTheme()
        {
            return mCurrentThemeManager.DetectTheme();
        }

        

        public void Dispose()
        {
            
        }

        #endregion
    }
}

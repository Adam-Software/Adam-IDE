using ControlzEx.Theming;
using System;
using System.Collections.ObjectModel;

namespace AdamStudio.Services.Interfaces
{
    public interface IThemeManagerService : IDisposable
    {
        #region Public fields
        public ReadOnlyObservableCollection<Theme> AppThemesCollection { get; }

        #endregion

        #region Public methods

        public Theme GetCurrentAppTheme();
        public Theme ChangeAppTheme(string themeName);
        public Theme ChangeAppTheme(Theme themeName);

        #endregion
    }
}

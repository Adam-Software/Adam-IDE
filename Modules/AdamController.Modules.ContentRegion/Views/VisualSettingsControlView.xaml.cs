using System.Windows.Controls;

namespace AdamController.Modules.ContentRegion.Views
{

    public partial class VisualSettingsControlView : UserControl
    {
        public VisualSettingsControlView()
        {
            InitializeComponent();

            //if (VisualSettingsControlView.ChangeBaseTheme == null)
            //{
             //   VisualSettingsControlView.ChangeBaseTheme = new Action<string>(theme => ChangeBaseTheme(theme));
            //}

            //if (VisualSettingsControlView.ChangeThemeColorScheme == null)
            //{
            //    VisualSettingsControlView.ChangeThemeColorScheme = new Action<string>(scheme => ChangeThemeColorScheme(scheme));
            //}   
        }

        //private void ChangeBaseTheme(string theme)
        //{
            //changes the base color of the application
            //_ = ThemeManager.Current.ChangeThemeBaseColor(Application.Current, theme);

            //changes the base color of the window
            //_ = ThemeManager.Current.ChangeThemeBaseColor(this, theme);
        //}

        //private void ChangeThemeColorScheme(string colorScheme)
        //{
            //changes the base color scheme of the application
            //_ = ThemeManager.Current.ChangeThemeColorScheme(Application.Current, colorScheme);

            //changes the base color scheme of the  window
            //_ = ThemeManager.Current.ChangeThemeColorScheme(this, colorScheme);
        //}
    }
}

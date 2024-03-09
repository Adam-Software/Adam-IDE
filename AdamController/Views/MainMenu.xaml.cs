using AdamController.ViewModels;
using System;
using System.Windows.Controls;

namespace AdamController.Views
{
    [Obsolete]
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();

            DataContext = new MainMenuView();
        }
    }
}

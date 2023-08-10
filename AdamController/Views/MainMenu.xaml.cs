using AdamController.ViewModels;
using System.Windows.Controls;

namespace AdamController.Views
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();

            DataContext = new MainMenuView();
        }
    }
}

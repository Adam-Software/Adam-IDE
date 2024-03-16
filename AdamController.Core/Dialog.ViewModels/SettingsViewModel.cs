using AdamController.Core.Mvvm;
using FolderBrowserEx;
using Prism.Commands;


namespace AdamController.Core.Dialog.ViewModels
{
    public class SettingsViewModel : DialogViewModelBase
    {
        private readonly IFolderBrowserDialog IFolderBrowser;

        #region ~

        public SettingsViewModel()
        {
            Title  = "Настройки Adam IDE";
            IFolderBrowser = new FolderBrowserDialog();
        }

        #endregion

        #region Command

        private DelegateCommand<string> showOpenFolderDialogCommand;
        public DelegateCommand<string> ShowOpenFolderDialogCommand => showOpenFolderDialogCommand ??= new DelegateCommand<string>(obj =>
        {
            string dialogPathType = obj;

            if (string.IsNullOrEmpty(dialogPathType)) return;

            switch (dialogPathType)
            {
                case "workspace":
                    {
                        string savedPath = OpenFolderDialog("Выберите директорию для сохранения рабочих областей", Properties.Settings.Default.SavedUserWorkspaceFolderPath);

                        if (string.IsNullOrEmpty(savedPath)) return;

                        Properties.Settings.Default.SavedUserWorkspaceFolderPath = savedPath;
                        break;
                    }
                case "toolbox":
                    {
                        string savedPath = OpenFolderDialog("Выберите директорию для сохранения панелей инструментов", Properties.Settings.Default.SavedUserToolboxFolderPath);

                        if (string.IsNullOrEmpty(savedPath)) return;

                        Properties.Settings.Default.SavedUserToolboxFolderPath = savedPath;
                        break;
                    }
                case "blocks":
                    {
                        string savedPath = OpenFolderDialog("Выберите директорию для сохранения блоков", Properties.Settings.Default.SavedUserCustomBlocksFolderPath);

                        if (string.IsNullOrEmpty(savedPath)) return;

                        Properties.Settings.Default.SavedUserCustomBlocksFolderPath = savedPath;
                        break;
                    }
                case "script":
                    {
                        string savedPath = OpenFolderDialog("Выберите директорию для сохранения скриптов", Properties.Settings.Default.SavedUserScriptsFolderPath);

                        if (string.IsNullOrEmpty(savedPath)) return;

                        Properties.Settings.Default.SavedUserScriptsFolderPath = savedPath;
                        break;
                    }
                case "testResult":
                    {
                        string savedPath = OpenFolderDialog("Выберите директорию для сохранения результатов тестов", Properties.Settings.Default.SavedResultsNetworkTestsFolderPath);

                        if (string.IsNullOrEmpty(savedPath)) return;

                        Properties.Settings.Default.SavedResultsNetworkTestsFolderPath = savedPath;
                        break;
                    }

                default:
                    break;
            }
        });

        #endregion

        #region Methods

        private string OpenFolderDialog(string title, string openDialogPath)
        {
            IFolderBrowser.Title = title;
            IFolderBrowser.InitialFolder = openDialogPath;
            IFolderBrowser.AllowMultiSelect = false;

            //if (IFolderBrowser.ShowDialog() == DialogResult.Cancel)
            //{
                //SettingWindowActivated = true;
            //    return null;
            //}

            //SettingWindowActivated = true;

            return IFolderBrowser.SelectedFolder;
        }

        
        #endregion
    }
}

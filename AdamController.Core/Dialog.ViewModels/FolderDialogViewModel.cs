using FolderBrowserEx;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Forms;

namespace AdamController.Core.Dialog.ViewModels
{
    public class FolderDialogViewModel : BindableBase
    {
        private readonly IFolderBrowserDialog IFolderBrowser;

        public FolderDialogViewModel()
        {
            IFolderBrowser = new FolderBrowserEx.FolderBrowserDialog();
        }

        #region UI behavior

        private bool settingWindowActivated;
        public bool SettingWindowActivated
        {
            get => settingWindowActivated;
            set
            {
                if (value == settingWindowActivated) return;

                settingWindowActivated = value;

                SetProperty(ref settingWindowActivated, value);
            }
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

            if (IFolderBrowser.ShowDialog() == DialogResult.Cancel)
            {
                SettingWindowActivated = true;
                return null;
            }

            SettingWindowActivated = true;

            return IFolderBrowser.SelectedFolder;
        }

        #endregion

    }
}

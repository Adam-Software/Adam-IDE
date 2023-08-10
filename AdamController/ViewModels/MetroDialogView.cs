using AdamController.Commands;
using MessageDialogManagerLib;
using System;
using System.Threading.Tasks;

namespace AdamController.ViewModels
{
    [Obsolete("Use it as example")]
    public class MetroDialogView
    {
        private readonly IMessageDialogManager IDialogManager;

        public MetroDialogView(IMessageDialogManager messageDialogManager)
        {
            IDialogManager = messageDialogManager;
            ShowFolderBrowserSingleCommand = new RelayCommand(obj => ShowFolderBrowserSingleCommandExecute(), obj =>  ShowFolderBrowserSingleCommandCanExecute());
            ShowFileBrowserSingleCommand = new RelayCommand(obj => ShowFileBrowserSingleCommandExecute(), obj =>  ShowFileBrowserSingleCommandCanExecute());
            ShowFolderBrowserMultipleCommand = new RelayCommand(obj => ShowFolderBrowserMultipleCommandExecute(), obj => ShowFolderBrowserMultipleCommandCanExecute());
            ShowFileBrowserMultipleCommand = new RelayCommand(obj => ShowFileBrowserMultipleCommandExecute(), obj => ShowFileBrowserMultipleCommandCanExecute());
            ShowInfoDialogCommand = new RelayCommand(obj => ShowInfoDialogCommandExecute(), obj => ShowInfoDialogCommandCanExecute());
            ShowProgressCommand = new RelayCommand(obj => ShowProgressCommandExecute(), obj => ShowProgressCommandCanExecute());
            ShowSaveFileDialogCommand = new RelayCommand(obj => ShowSaveFileDialogCommandExecute(), obj => ShowSaveFileDialogCommandCanExecute());

           
        }

        public static Action<string> AppLogStatusBarAction { get; set; }
        public RelayCommand ShowFolderBrowserSingleCommand { get; private set; }
        public RelayCommand ShowFileBrowserSingleCommand { get; private set; }
        public RelayCommand ShowFolderBrowserMultipleCommand { get; private set; }
        public RelayCommand ShowFileBrowserMultipleCommand { get; private set; }
        public RelayCommand ShowInfoDialogCommand { get; private set; }
        public RelayCommand ShowProgressCommand { get; private set; }
        public RelayCommand ShowSaveFileDialogCommand { get; private set; }

        private bool ShowFolderBrowserSingleCommandCanExecute()
        {
            return true;
        }
        private async void ShowFolderBrowserSingleCommandExecute()
        {
            if (IDialogManager.ShowFolderBrowser("Select a folder", ""))
                await IDialogManager.ShowInfoDialogAsync("Folder Selected", IDialogManager.FolderPath);
            else
                await IDialogManager.ShowInfoDialogAsync("No folder has been selected", IDialogManager.FolderPath);
        }

        private bool ShowFileBrowserSingleCommandCanExecute()
        {
            return true;
        }

        private void ShowFileBrowserSingleCommandExecute()
        {
            if (IDialogManager.ShowFileBrowser("Select a file", "", "*.* | *.*"))
                
                AppLogStatusBarAction($"Файл сохранен {IDialogManager.FilePath}");
            else
                AppLogStatusBarAction($"Файл не сохранен {IDialogManager.FilePath}");
        }

        private bool ShowFolderBrowserMultipleCommandCanExecute()
        {
            return true;
        }

        private async void ShowFolderBrowserMultipleCommandExecute()
        {
            if (IDialogManager.ShowFolderBrowser("Select a folder", "", true))
            {
                string selected = string.Empty;
                foreach (var folder in IDialogManager.FolderPaths)
                {
                    selected += $"{folder}\n";
                }
                await IDialogManager.ShowInfoDialogAsync("Folders Selected", selected);
            }
            else
                await IDialogManager.ShowInfoDialogAsync("No folder has been selected", IDialogManager.FolderPath);
        }

        private bool ShowFileBrowserMultipleCommandCanExecute()
        {
            return true;
        }

        private async void ShowFileBrowserMultipleCommandExecute()
        {
            if (IDialogManager.ShowFileBrowser("Select a file", "", "*.* | *.*", true))
            {
                string selected = string.Empty;
                foreach (string file in IDialogManager.FilePaths)
                {
                    selected += $"{file}\n";
                }
                await IDialogManager.ShowInfoDialogAsync("File Selected", selected);
            }
            else
                await IDialogManager.ShowInfoDialogAsync("No file has been selected", IDialogManager.FilePath);
        }

        private async void ShowInfoDialogCommandExecute()
        {
            await IDialogManager.ShowInfoDialogAsync("Info dialog", "This is a info dialog example");
        }

        private bool ShowInfoDialogCommandCanExecute()
        {
            return true;
        }

        
        private bool ShowProgressCommandCanExecute()
        {
            return true;
        }

        private async void ShowProgressCommandExecute()
        {
            await IDialogManager.ShowProgress("Progress", "This is a progress dialog");
            await DoSomeWorkAsync();
            await IDialogManager.CloseProgress();
        }

        private async Task DoSomeWorkAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                IDialogManager.UpdateProgress(i * 10);
                IDialogManager.UpdateMessageProgress($"Step {i} done");
            }
        }

        private bool ShowSaveFileDialogCommandCanExecute()
        {
            return true;
        }

        private void ShowSaveFileDialogCommandExecute()
        {
            if (IDialogManager.ShowSaveFileDialog("Save a file", "", "fileName", ".txt", "Text documents (.txt)|*.txt"))
                AppLogStatusBarAction("Файл сохранен");
            else
                AppLogStatusBarAction("Файл не сохранен");
        }
    }
}

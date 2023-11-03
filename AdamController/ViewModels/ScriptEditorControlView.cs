using AdamController.Commands;
using AdamController.Helpers;
using AdamController.ViewModels.HamburgerMenu;
using AdamController.WebApi.Client.v1;
using AdamController.WebApi.Client.v1.ResponseModel;
using MessageDialogManagerLib;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AdamController.ViewModels
{
    public class ScriptEditorControlView : HamburgerMenuItemView
    {
        public static Action<string> AppLogStatusBarAction { get; set; }

        private bool mIsWarningStackOwerflowAlreadyShow;
        private readonly IMessageDialogManager IDialogManager;

        public ScriptEditorControlView(HamburgerMenuView hamburgerMenuView = null) : base(hamburgerMenuView)
        {
            IDialogManager = new MessageDialogManagerMahapps(Application.Current);

            InitAction();
            PythonExecuteEvent();
        }

        #region Python execute event

        private void PythonExecuteEvent()
        {
            PythonScriptExecuteHelper.OnExecuteStartEvent += (message) =>
            {
                if (MainWindowView.GetSelectedPageIndex != 1)
                    return;

                ResultTextEditor = string.Empty;
                IsCodeExecuted = true;
                mIsWarningStackOwerflowAlreadyShow = false;
                ResultTextEditor += message;
            };

            PythonScriptExecuteHelper.OnStandartOutputEvent += (message) => 
            {
                if (MainWindowView.GetSelectedPageIndex != 1)
                    return;

                if (ResultTextEditorLength > 10000)
                {
                    if (!mIsWarningStackOwerflowAlreadyShow)
                    {
                        ResultTextEditor += "\nДальнейший вывод результата, приведет к переполнению буфера, поэтому будет скрыт.";
                        ResultTextEditor += "\nПрограмма продолжает выполняться в неинтерактивном режиме.";
                        ResultTextEditor += "\nДля остановки нажмите \"Stop\". Или дождитесь завершения.";

                        mIsWarningStackOwerflowAlreadyShow = true;
                    }

                    return;
                }

                ResultTextEditor += message;
            };
            
            PythonScriptExecuteHelper.OnExecuteFinishEvent += (message) =>
            {
                if (MainWindowView.GetSelectedPageIndex != 1)
                    return;

                IsCodeExecuted = false;
                ResultTextEditor += message;
            };
        }

        #endregion

        #region Source text editor

        private string sourceTextEditor;
        public string SourceTextEditor
        {
            get => sourceTextEditor;
            set
            {
                if (value == sourceTextEditor) return;

                sourceTextEditor = value;
                OnPropertyChanged(nameof(SourceTextEditor));
            }
        }

        private string selectedText;
        public string SelectedText
        {
            get => selectedText;
            set
            {
                if (value == selectedText) return;

                selectedText = value;
                OnPropertyChanged(nameof(SelectedText));
            }
        }

        #endregion

        #region SendSourceToScriptEditor Action

        private void InitAction()
        {
            if(ScratchControlView.SendSourceToScriptEditor == null)
            {
                ScratchControlView.SendSourceToScriptEditor = new Action<string>(source => SourceTextEditor = source);
            }
        }

        #endregion

        #region Result text editor

        private string resultTextEditor;
        public string ResultTextEditor
        {
            get => resultTextEditor;
            set
            {
                if (value == resultTextEditor) return;

                resultTextEditor = value;
                ResultTextEditorLength = value.Length;

                OnPropertyChanged(nameof(ResultTextEditor));
            }
        }

        #endregion

        #region ResultTextEditorLength

        private int resultTextEditorLength;
        public int ResultTextEditorLength
        {
            get => resultTextEditorLength;
            set
            {
                if (value == resultTextEditorLength) return;

                resultTextEditorLength = value;
                OnPropertyChanged(nameof(ResultTextEditorLength));
            }
        }

        #endregion

        #region IsCodeExecuted field

        private bool isCodeExecuted = false;
        public bool IsCodeExecuted
        {
            get => isCodeExecuted;
            set
            {
                if (value == isCodeExecuted) return;

                isCodeExecuted = value;
                OnPropertyChanged(nameof(IsCodeExecuted));
            }
        }

        #endregion

        #region ResultTextEditorError

        private string resultTextEditorError;
        public string ResultTextEditorError
        {
            get => resultTextEditorError;
            set
            {
                if (value == resultTextEditorError) return;

                if(value.Length > 0)
                    resultTextEditorError = $"Error: {value}";
                else
                    resultTextEditorError = value;

                OnPropertyChanged(nameof(ResultTextEditorError));
            }
        }

        #endregion

        #region Command

        #region Run code button

        private RelayCommand runCode;
        public RelayCommand RunCode => runCode ??= new RelayCommand(async obj =>
        {
            ResultTextEditorError = string.Empty;
            ExtendedCommandExecuteResult executeResult = new();

            try
            {
                if(ComunicateHelper.TcpClientIsConnected)
                {
                    var command = new WebApi.Client.v1.RequestModel.PythonCommand
                    {
                        Command = SourceTextEditor
                    };
                    executeResult = await BaseApi.PythonExecuteAsync(command);
                }
                
            }
            catch (Exception ex)
            {
                ResultTextEditorError = ex.Message.ToString();
            }

            if (Properties.Settings.Default.ChangeExtendedExecuteReportToggleSwitchState)
            {
                ResultTextEditor += "Отчет о инициализации процесса программы\n" +
                 "======================\n" +
                 $"Начало инициализации: {executeResult.StartTime}\n" +
                 $"Завершение инициализации: {executeResult.EndTime}\n" +
                 $"Общее время инициализации: {executeResult.RunTime}\n" +
                 $"Код выхода: {executeResult.ExitCode}\n" +
                 $"Статус успешности инициализации: {executeResult.Succeesed}" +
                 "\n======================\n";
            }

            if (!string.IsNullOrEmpty(executeResult?.StandardError))
                ResultTextEditor += $"Ошибка: {executeResult?.StandardError}" +
                    "\n======================";

        }, canExecute => !string.IsNullOrEmpty(SourceTextEditor));

        #endregion

        private RelayCommand copyToClipboard;
        public RelayCommand CopyToClipboard => copyToClipboard ??= new RelayCommand(obj =>
        {
            if (SelectedText == null)
                return;

            Clipboard.SetText(SelectedText);
        });

        private RelayCommand cutToClipboard;
        public RelayCommand CutToClipboard => cutToClipboard ??= new RelayCommand(obj =>
        {
            if (SourceTextEditor == null)
                return;

            Clipboard.SetText(SourceTextEditor);
            SourceTextEditor = string.Empty;
        });

        private RelayCommand pasteFromClipboard;
        public RelayCommand PasteFromClipboard => pasteFromClipboard ??= new RelayCommand(obj =>
        {
            string text = Clipboard.GetText();
            SourceTextEditor += text;
        });

        private RelayCommand stopExecute;
        public RelayCommand StopExecute => stopExecute ??= new RelayCommand(async obj =>
        {
            try
            {
                await BaseApi.StopPythonExecute();
            }
            catch(Exception ex)
            {
                ResultTextEditorError = ex.Message; 
            }
           
        });

        private RelayCommand cleanExecuteEditor;
        public RelayCommand CleanExecuteEditor => cleanExecuteEditor ??= new RelayCommand(async obj =>
        {
            await Task.Run(() =>
            {
                ResultTextEditorError = string.Empty;
                ResultTextEditor = string.Empty; 
            });
        });

        #region OpenFileDialogCommand

        private RelayCommand showOpenFileDialogCommand;
        public RelayCommand ShowOpenFileDialogCommand => showOpenFileDialogCommand ??= new RelayCommand(async obj =>
        {
            if (IDialogManager.ShowFileBrowser("Выберите файл с исходным кодом программы", 
                Properties.Settings.Default.SavedUserScriptsFolderPath, "PythonScript file (.py)|*.py|Все файлы|*.*"))
            {
                string path = IDialogManager.FilePath;
                if (path == "") return;

                string pythonProgram = await FileHelper.ReadTextAsStringAsync(path);
                SourceTextEditor = pythonProgram;

                AppLogStatusBarAction($"Файл {path} загружен");
            }
            else
            {
                AppLogStatusBarAction("Файл c исходным кодом не выбран");
            }
        });

        #endregion

        #region ShowSaveFileDialogCommand

        private RelayCommand showSaveFileDialogCommand;
        public RelayCommand ShowSaveFileDialogCommand => showSaveFileDialogCommand ??= new RelayCommand(async obj =>
        {
            string pythonProgram = SourceTextEditor;

            if (IDialogManager.ShowSaveFileDialog("Сохранить файл программы", Properties.Settings.Default.SavedUserScriptsFolderPath,
                "new_program", ".py", "PythonScript file (.py)|*.py|Все файлы|*.*"))
            {
                string path = IDialogManager.FilePathToSave;
                await FileHelper.WriteAsync(path, pythonProgram);

                AppLogStatusBarAction($"Файл {IDialogManager.FilePathToSave} сохранен");
            }
            else
            {
                AppLogStatusBarAction("Файл не сохранен");
            }
        }, canExecute => SourceTextEditor?.Length > 0);

        #endregion

        #endregion
    }
}

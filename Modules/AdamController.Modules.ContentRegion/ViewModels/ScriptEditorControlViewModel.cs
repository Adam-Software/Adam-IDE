using AdamController.Core;
using AdamController.Core.Mvvm;
using AdamController.Services.Interfaces;
using AdamController.WebApi.Client.v1;
using AdamController.WebApi.Client.v1.ResponseModel;
using ICSharpCode.AvalonEdit.Highlighting;
using MessageDialogManagerLib;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ScriptEditorControlViewModel : RegionViewModelBase 
    {
        #region Services

        private readonly ICommunicationProviderService mCommunicationProvider;
        private readonly IPythonRemoteRunnerService mPythonRemoteRunner;
        private readonly IStatusBarNotificationDeliveryService mStatusBarNotificationDelivery;
        private readonly IFileManagmentService mFileManagment;
        private readonly IWebApiService mWebApiService;
        private readonly IDialogManagerService mDialogManager;

        #endregion

        private bool mIsWarningStackOwerflowAlreadyShow;

        public ScriptEditorControlViewModel(IRegionManager regionManager, IDialogService dialogService, ICommunicationProviderService communicationProvider, 
                    IPythonRemoteRunnerService pythonRemoteRunner, IStatusBarNotificationDeliveryService statusBarNotificationDelivery, 
                    IFileManagmentService fileManagment, IWebApiService webApiService, IAvalonEditService avalonEditService, IDialogManagerService dialogManager) : base(regionManager, dialogService)
        {
            mCommunicationProvider = communicationProvider;
            mPythonRemoteRunner = pythonRemoteRunner;
            mStatusBarNotificationDelivery = statusBarNotificationDelivery;
            mFileManagment = fileManagment;
            mWebApiService = webApiService;
            mDialogManager = dialogManager;

            HighlightingDefinition = avalonEditService.GetDefinition(HighlightingName.AdamPython);
        }

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            mPythonRemoteRunner.RaisePythonScriptExecuteStartEvent += OnRaisePythonScriptExecuteStart;
            mPythonRemoteRunner.RaisePythonStandartOutputEvent += OnRaisePythonStandartOutput;
            mPythonRemoteRunner.RaisePythonScriptExecuteFinishEvent += OnRaisePythonScriptExecuteFinish;

            var sourceCode = string.Empty;

            navigationContext.Parameters.TryGetValue(NavigationParametersKey.SourceCode, out sourceCode);
            
            if(sourceCode != null)
            {
                SourceTextEditor = sourceCode;
            }
            
            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            mPythonRemoteRunner.RaisePythonScriptExecuteStartEvent -= OnRaisePythonScriptExecuteStart;
            mPythonRemoteRunner.RaisePythonStandartOutputEvent -= OnRaisePythonStandartOutput;
            mPythonRemoteRunner.RaisePythonScriptExecuteFinishEvent -= OnRaisePythonScriptExecuteFinish;

            base.OnNavigatedFrom(navigationContext);
        }


        #endregion

        #region Event methods

        private void OnRaisePythonScriptExecuteStart(object sender)
        {
            ResultTextEditor = string.Empty;
            IsCodeExecuted = true;
            mIsWarningStackOwerflowAlreadyShow = false;
        }

        private void OnRaisePythonStandartOutput(object sender, string message)
        {
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
        }

        private void OnRaisePythonScriptExecuteFinish(object sender, ExtendedCommandExecuteResult remoteCommandExecuteResult)
        {
            IsCodeExecuted = false;
            //ResultTextEditor += message;
        }

        #endregion

        #region Public fields

        private IHighlightingDefinition highlightingDefinition;
        public IHighlightingDefinition HighlightingDefinition
        {
            get => highlightingDefinition;
            set => SetProperty(ref highlightingDefinition, value);
        }

        #endregion

        #region Python execute event

        private void PythonExecuteEvent()
        {
            //PythonScriptExecuteHelper.OnExecuteStartEvent += (message) =>
            //{
                //if (MainWindowViewModel.GetSelectedPageIndex != 1)
                //    return;

                //ResultTextEditor = string.Empty;
                //IsCodeExecuted = true;
                //mIsWarningStackOwerflowAlreadyShow = false;
                //ResultTextEditor += message;
            //};

            //PythonScriptExecuteHelper.OnStandartOutputEvent += (message) => 
            //{
                //if (MainWindowViewModel.GetSelectedPageIndex != 1)
                //    return;

                //if (ResultTextEditorLength > 10000)
                //{
                    //if (!mIsWarningStackOwerflowAlreadyShow)
                    //{
                    //    ResultTextEditor += "\nДальнейший вывод результата, приведет к переполнению буфера, поэтому будет скрыт.";
                    //    ResultTextEditor += "\nПрограмма продолжает выполняться в неинтерактивном режиме.";
                    //    ResultTextEditor += "\nДля остановки нажмите \"Stop\". Или дождитесь завершения.";

                    //    mIsWarningStackOwerflowAlreadyShow = true;
                    //}

                    //return;
                //}

                //ResultTextEditor += message;
            //};
            
            //PythonScriptExecuteHelper.OnExecuteFinishEvent += (message) =>
            //{
                //if (MainWindowViewModel.GetSelectedPageIndex != 1)
                //    return;

                //IsCodeExecuted = false;
                //ResultTextEditor += message;
            //};
        }

        #endregion

        #region Source text editor

        private string sourceTextEditor;
        public string SourceTextEditor
        {
            get => sourceTextEditor;
            set => SetProperty(ref sourceTextEditor, value);
        }

        private string selectedText;
        public string SelectedText
        {
            get => selectedText;
            set => SetProperty(ref selectedText, value);
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

                SetProperty(ref resultTextEditor, value);
            }
        }

        #endregion

        #region ResultTextEditorLength

        private int resultTextEditorLength;
        public int ResultTextEditorLength
        {
            get => resultTextEditorLength;
            set => SetProperty(ref resultTextEditorLength, value);
        }

        #endregion

        #region IsCodeExecuted field

        private bool isCodeExecuted = false;
        public bool IsCodeExecuted
        {
            get => isCodeExecuted;
            set => SetProperty(ref isCodeExecuted, value);
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

                SetProperty(ref resultTextEditorError, value);
            }
        }

        #endregion

        #region Command

        #region Run code button

        private DelegateCommand runCode;
        public DelegateCommand RunCode => runCode ??= new DelegateCommand(async () =>
        {
            ResultTextEditorError = string.Empty;
            ExtendedCommandExecuteResult executeResult = new();

            try
            {
                if(mCommunicationProvider.IsTcpClientConnected)
                {
                    var command = new WebApi.Client.v1.RequestModel.PythonCommandModel
                    {
                        Command = SourceTextEditor
                    };
                    executeResult = await mWebApiService.PythonExecuteAsync(command);
                }
                
            }
            catch (Exception ex)
            {
                ResultTextEditorError = ex.Message.ToString();
            }

            if (Core.Properties.Settings.Default.ChangeExtendedExecuteReportToggleSwitchState)
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

        }, () => !string.IsNullOrEmpty(SourceTextEditor));

        #endregion

        private DelegateCommand copyToClipboard;
        public DelegateCommand CopyToClipboard => copyToClipboard ??= new DelegateCommand(() =>
        {
            if (SelectedText == null)
                return;

            Clipboard.SetText(SelectedText);
        });

        private DelegateCommand cutToClipboard;
        public DelegateCommand CutToClipboard => cutToClipboard ??= new DelegateCommand(() =>
        {
            if (SourceTextEditor == null)
                return;

            Clipboard.SetText(SourceTextEditor);
            SourceTextEditor = string.Empty;
        });

        private DelegateCommand pasteFromClipboard;
        public DelegateCommand PasteFromClipboard => pasteFromClipboard ??= new DelegateCommand(() =>
        {
            string text = Clipboard.GetText();
            SourceTextEditor += text;
        });

        private DelegateCommand stopExecute;
        public DelegateCommand StopExecute => stopExecute ??= new DelegateCommand(async () =>
        {
            try
            {
                await mWebApiService.StopPythonExecute();
            }
            catch(Exception ex)
            {
                ResultTextEditorError = ex.Message; 
            }
           
        });

        private DelegateCommand cleanExecuteEditor;
        public DelegateCommand CleanExecuteEditor => cleanExecuteEditor ??= new DelegateCommand(async () =>
        {
            await Task.Run(() =>
            {
                ResultTextEditorError = string.Empty;
                ResultTextEditor = string.Empty; 
            });
        });

        #region OpenFileDialogCommand

        private DelegateCommand showOpenFileDialogCommand;
        public DelegateCommand ShowOpenFileDialogCommand => showOpenFileDialogCommand ??= new DelegateCommand(async () =>
        {
            if (mDialogManager.ShowFileBrowser("Выберите файл с исходным кодом программы", 
                    Core.Properties.Settings.Default.SavedUserScriptsFolderPath, "PythonScript file (.py)|*.py|Все файлы|*.*"))
            {
                string path = mDialogManager.FilePath;
                if (path == "") return;

                string pythonProgram = await mFileManagment.ReadTextAsStringAsync(path);
                SourceTextEditor = pythonProgram;

                mStatusBarNotificationDelivery.AppLogMessage = $"Файл {path} загружен";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Файл c исходным кодом не выбран";
            }
        });

        #endregion

        #region ShowSaveFileDialogCommand

        private DelegateCommand showSaveFileDialogCommand;

        public DelegateCommand ShowSaveFileDialogCommand => showSaveFileDialogCommand ??= new DelegateCommand(async () =>
        {
            string pythonProgram = SourceTextEditor;

            if (mDialogManager.ShowSaveFileDialog("Сохранить файл программы", Core.Properties.Settings.Default.SavedUserScriptsFolderPath,
                "new_program", ".py", "PythonScript file (.py)|*.py|Все файлы|*.*"))
            {
                string path = mDialogManager.FilePathToSave;
                await mFileManagment.WriteAsync(path, pythonProgram);

                mStatusBarNotificationDelivery.AppLogMessage = $"Файл {mDialogManager.FilePathToSave} сохранен";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Файл не сохранен";
            }
        }, () => SourceTextEditor?.Length > 0);

        #endregion

        #endregion
    }
}

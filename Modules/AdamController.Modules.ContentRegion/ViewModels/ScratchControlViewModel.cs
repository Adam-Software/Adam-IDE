using AdamBlocklyLibrary;
using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using AdamBlocklyLibrary.ToolboxSets;
using AdamController.Core;
using AdamController.Core.Extensions;
using AdamController.Core.Mvvm;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using AdamController.Services.WebViewProviderDependency;
using AdamController.WebApi.Client.v1;
using AdamController.WebApi.Client.v1.ResponseModel;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ScratchControlViewModel : RegionViewModelBase 
    {
        #region DelegateCommands

        public DelegateCommand ReloadWebViewDelegateCommand { get; }
        public DelegateCommand ShowSaveFileDialogDelegateCommand { get; }
        public DelegateCommand ShowOpenFileDialogDelegateCommand { get; }
        public DelegateCommand ShowSaveFileSourceTextDialogDelegateCommand { get; }
        public DelegateCommand CleanExecuteEditorDelegateCommand { get; }
        public DelegateCommand RunPythonCodeDelegateCommand { get; }
        public DelegateCommand StopPythonCodeExecuteDelegateCommand { get; }
        public DelegateCommand SendCodeToExternalSourceEditorDelegateCommand { get; }
        public DelegateCommand ToZeroPositionDelegateCommand { get; } 

        #endregion

        #region Services

        private readonly ICommunicationProviderService mCommunicationProvider;
        private readonly IPythonRemoteRunnerService mPythonRemoteRunner;
        private readonly IStatusBarNotificationDeliveryService mStatusBarNotificationDelivery;
        private readonly IWebViewProvider mWebViewProvider;
        private readonly IDialogManagerService mDialogManager;
        private readonly IFileManagmentService mFileManagment;

        #endregion

        #region Const

        private const string cFilter = "XML documents (.xml) | *.xml";
        private const string cDebugColorScheme = "Red";

        #endregion

        #region Var

        private bool mIsWarningStackOwerflowAlreadyShow;

        //#8 p 6
        //private string mCurrentColorScheme;

        #endregion

        #region ~

        public ScratchControlViewModel(IRegionManager regionManager, IDialogService dialogService, ICommunicationProviderService communicationProvider, IPythonRemoteRunnerService pythonRemoteRunner, IStatusBarNotificationDeliveryService statusBarNotificationDelivery, IWebViewProvider webViewProvider, IDialogManagerService dialogManager, IFileManagmentService fileManagment) : base(regionManager, dialogService)
        {
            mCommunicationProvider = communicationProvider;
            mPythonRemoteRunner = pythonRemoteRunner;
            mStatusBarNotificationDelivery = statusBarNotificationDelivery;
            mWebViewProvider = webViewProvider;
            mDialogManager = dialogManager;
            mFileManagment = fileManagment;

            ReloadWebViewDelegateCommand = new DelegateCommand(ReloadWebView, ReloadWebViewCanExecute);
            ShowSaveFileDialogDelegateCommand = new DelegateCommand(ShowSaveFileDialog, ShowSaveFileDialogCanExecute);
            ShowOpenFileDialogDelegateCommand = new DelegateCommand(ShowOpenFileDialog, ShowOpenFileDialogCanExecute);
            ShowSaveFileSourceTextDialogDelegateCommand = new DelegateCommand(ShowSaveFileSourceTextDialog, ShowSaveFileSourceTextDialogCanExecute);
            CleanExecuteEditorDelegateCommand = new DelegateCommand(CleanExecuteEditor, CleanExecuteEditorCanExecute);
            RunPythonCodeDelegateCommand = new DelegateCommand(RunPythonCode, RunPythonCodeCanExecute);
            StopPythonCodeExecuteDelegateCommand = new DelegateCommand(StopPythonCodeExecute, StopPythonCodeExecuteCanExecute);
            SendCodeToExternalSourceEditorDelegateCommand = new(SendCodeToExternalSourceEditor, SendCodeToExternalSourceEditorCanExecute);
            ToZeroPositionDelegateCommand = new DelegateCommand(ToZeroPosition, ToZeroPositionCanExecute);
        }
        #endregion

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Subscribe();
            
            //#8 p 6
            //mCurrentColorScheme = ThemeManager.Current.DetectTheme(Application.Current).ColorScheme;

            //#29
            mWebViewProvider.ReloadWebView();

            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Unsubscribe();

            //#8 p 6
            //ThemeManager.Current.ChangeThemeColorScheme(Application.Current, mCurrentColorScheme);

            base.OnNavigatedFrom(navigationContext);
        }

        #endregion

        #region Public fields

        private bool isTcpClientConnected;
        public bool IsTcpClientConnected
        {
            get => isTcpClientConnected;
            set
            {
                bool isNewValue = SetProperty(ref isTcpClientConnected, value);

                if (isNewValue)
                {
                    RaiseDelegateCommandsCanExecuteChanged();
                }
            }
        }

        private bool isPythonCodeExecute;
        public bool IsPythonCodeExecute
        {
            get => isPythonCodeExecute;
            set
            {
                bool isNewValue = SetProperty(ref isPythonCodeExecute, value);

                if (isNewValue)
                {
                    OnPythonCodeExecuteStatusChange(IsPythonCodeExecute);
                    RaiseDelegateCommandsCanExecuteChanged();
                }
            }
        }

        private string sourceTextEditor;
        public string SourceTextEditor
        {
            get => sourceTextEditor;
            set
            {
                bool isNewValue = SetProperty(ref sourceTextEditor, value);

                if (isNewValue)
                {
                    RaiseDelegateCommandsCanExecuteChanged();
                }
            }
        }

        private string resultTextEditor;
        public string ResultTextEditor
        {
            get => resultTextEditor;
            set
            {
                bool isNewValue = SetProperty(ref resultTextEditor, value);

                if (isNewValue)
                {
                    ResultTextEditorLength = ResultTextEditor.Length;
                    RaiseDelegateCommandsCanExecuteChanged();
                }       
            }
        }

        private int resultTextEditorLength;
        public int ResultTextEditorLength
        {
            get => resultTextEditorLength;
            set => SetProperty(ref resultTextEditorLength, value);
        }

        private string resultTextEditorError;
        public string ResultTextEditorError
        {
            get => resultTextEditorError;
            set
            {
                bool isNewValue = SetProperty(ref resultTextEditorError, value);

                if (isNewValue)
                {
                    RaiseDelegateCommandsCanExecuteChanged();

                    if (ResultTextEditorError.Length > 0)
                        resultTextEditorError = $"Error: {ResultTextEditorError}";
                }
            }
        }

        private string pythonVersion;
        public string PythonVersion
        {
            get => pythonVersion;
            set => SetProperty(ref pythonVersion, value);
        }

        private string pythonBinPath;
        public string PythonBinPath
        {
            get => pythonBinPath;
            set => SetProperty(ref pythonBinPath, value);
        }

        private string pythonWorkDir;
        public string PythonWorkDir
        {
            get => pythonWorkDir;
            set => SetProperty(ref pythonWorkDir, value);
        }


        #endregion

        #region Subscribes

        private void Subscribe()
        {
            mCommunicationProvider.RaiseTcpServiceCientConnectedEvent += OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnectEvent += OnRaiseTcperviceClientDisconnect;

            mPythonRemoteRunner.RaisePythonScriptExecuteStartEvent += OnRaisePythonScriptExecuteStart;
            mPythonRemoteRunner.RaisePythonStandartOutputEvent += OnRaisePythonStandartOutput;
            mPythonRemoteRunner.RaisePythonScriptExecuteFinishEvent += OnRaisePythonScriptExecuteFinish;

            mWebViewProvider.RaiseWebViewMessageReceivedEvent += RaiseWebViewbMessageReceivedEvent;
            mWebViewProvider.RaiseWebViewNavigationCompleteEvent += RaiseWebViewNavigationCompleteEvent;
        }

        private void Unsubscribe()
        {
            mCommunicationProvider.RaiseTcpServiceCientConnectedEvent -= OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnectEvent -= OnRaiseTcperviceClientDisconnect;

            mPythonRemoteRunner.RaisePythonScriptExecuteStartEvent -= OnRaisePythonScriptExecuteStart;
            mPythonRemoteRunner.RaisePythonStandartOutputEvent -= OnRaisePythonStandartOutput;
            mPythonRemoteRunner.RaisePythonScriptExecuteFinishEvent -= OnRaisePythonScriptExecuteFinish;

            mWebViewProvider.RaiseWebViewMessageReceivedEvent -= RaiseWebViewbMessageReceivedEvent;
            mWebViewProvider.RaiseWebViewNavigationCompleteEvent -= RaiseWebViewNavigationCompleteEvent;

        }

        #endregion

        #region DelegateCommands methods

        private void ReloadWebView()
        {
            //Need to clear the field? Test
            //SourceTextEditor = string.Empty;
            mWebViewProvider.ReloadWebView();
        }

        private bool ReloadWebViewCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            return isPythonCodeNotExecute;
        }

        private async void ShowSaveFileDialog()
        {
            string workspace = await mWebViewProvider.ExecuteJavaScript("getSavedWorkspace()");
            string xmlWorkspace = JsonConvert.DeserializeObject<dynamic>(workspace);
            string dialogTitle = "Сохранить рабочую область";
            string initialPath = Settings.Default.SavedUserWorkspaceFolderPath;
            string fileName = "workspace";
            string defaultExt = ".xml";
            //string filter = "XML documents (.xml)|*.xml";

            if (mDialogManager.ShowSaveFileDialog(dialogTitle, initialPath, fileName, defaultExt, cFilter))
            {
                string path = mDialogManager.FilePathToSave;
                await mFileManagment.WriteAsync(path, xmlWorkspace);

                mStatusBarNotificationDelivery.AppLogMessage = $"Файл {mDialogManager.FilePathToSave} сохранен";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Файл не сохранен";
            }
        }

        private bool ShowSaveFileDialogCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            bool isSourceNotEmpty = SourceTextEditor?.Length > 0;

            return isPythonCodeNotExecute && isSourceNotEmpty;
        }

        private async void ShowOpenFileDialog()
        {
            string title = "Выберите сохранененную рабочую область";
            string initialPath = Settings.Default.SavedUserWorkspaceFolderPath;

            if (mDialogManager.ShowFileBrowser(title, initialPath, cFilter))
            {
                string path = mDialogManager.FilePath;
                if (path == "") return;

                string xml = await mFileManagment.ReadTextAsStringAsync(path);
                _ = await ExecuteScriptFunctionAsync("loadSavedWorkspace", new object[] { xml });

                mStatusBarNotificationDelivery.AppLogMessage = $"Файл {path} загружен";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Файл рабочей области не выбран";
            }
        }

        private bool ShowOpenFileDialogCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            return isPythonCodeNotExecute;
        }

        private async void ShowSaveFileSourceTextDialog()
        {
            string pythonProgram = SourceTextEditor;
            string title = "Сохранить файл программы";
            string initialPath = Settings.Default.SavedUserScriptsFolderPath;
            string fileName = "new_program";
            string defaultExt = ".py";
            string filter = "PythonScript file (.py)|*.py|Все файлы|*.*";


            if (mDialogManager.ShowSaveFileDialog(title, initialPath, fileName, defaultExt, filter))
            {
                string path = mDialogManager.FilePathToSave;
                await mFileManagment.WriteAsync(path, pythonProgram);

                mStatusBarNotificationDelivery.AppLogMessage = $"Файл {mDialogManager.FilePathToSave} сохранен";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Файл не сохранен";
            }
        }

        private bool ShowSaveFileSourceTextDialogCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            bool isSourceNotEmpty = SourceTextEditor?.Length > 0;
            return isPythonCodeNotExecute && isSourceNotEmpty;
        }

        private void CleanExecuteEditor()
        {
            ResultTextEditorError = string.Empty;
            ResultTextEditor = string.Empty;
        }

        private bool CleanExecuteEditorCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            var isResultNotEmpty = ResultTextEditor?.Length > 0;
            return isPythonCodeNotExecute &&  isResultNotEmpty;
        }

        private async void RunPythonCode()
        {
            ResultTextEditorError = string.Empty;
            WebApi.Client.v1.ResponseModel.ExtendedCommandExecuteResult executeResult = new();

            try
            {
                var command = new WebApi.Client.v1.RequestModel.PythonCommand
                {
                    Command = SourceTextEditor
                };

                executeResult = await BaseApi.PythonExecuteAsync(command);
            }
            catch (Exception ex)
            {
                ResultTextEditorError = ex.Message.ToString();
            }
            finally
            {
                
            }

            if (Settings.Default.ChangeExtendedExecuteReportToggleSwitchState)
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
                ResultTextEditor += $"Ошибка: {executeResult.StandardError}" +
                    "\n======================";
        }

        private bool RunPythonCodeCanExecute()
        {
            bool isSourceNotEmpty = SourceTextEditor?.Length > 0;
            bool isTcpConnected = IsTcpClientConnected;
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;

            return isSourceNotEmpty && isTcpConnected && isPythonCodeNotExecute;
        }

        private async void StopPythonCodeExecute()
        {
            IsPythonCodeExecute = false;

            try
            {
                await BaseApi.StopPythonExecute();
            }
            catch (Exception ex)
            {
                ResultTextEditorError = ex.Message.ToString();
            }
        }

        private bool StopPythonCodeExecuteCanExecute()
        {
            bool isConnected = IsTcpClientConnected;
            bool isPythonCodeExecute = IsPythonCodeExecute;

            return isConnected && isPythonCodeExecute;
        }

        private void SendCodeToExternalSourceEditor()
        {
            NavigationParameters parameters = new()
            {
                { NavigationParametersKey.SourceCode, SourceTextEditor }
            };

            RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionScriptEditor, parameters);
        }

        private bool SendCodeToExternalSourceEditorCanExecute()
        {
            bool isSourceNotEmpty = SourceTextEditor?.Length > 0;
            bool isTcpConnected = IsTcpClientConnected;
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;

            return isSourceNotEmpty && isTcpConnected && isPythonCodeNotExecute;
        }

        private async void ToZeroPosition()
        {
            try
            {
                await BaseApi.StopPythonExecute();
            }
            catch (Exception ex)
            {
                ResultTextEditorError = ex.Message.ToString();
            }
        }

        private bool ToZeroPositionCanExecute()
        {
            bool isTcpConnected = IsTcpClientConnected;
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;

            return isTcpConnected && isPythonCodeNotExecute;
        }


        #endregion

        #region Private methods

        private void OnPythonCodeExecuteStatusChange(bool isPythonCodeExecute)
        {
            
            if (!isPythonCodeExecute)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(async () =>
                {
                    await mWebViewProvider.ExecuteJavaScript(Scripts.ShadowDisable);
                }));

                mStatusBarNotificationDelivery.CompileLogMessage = "Сеанс отладки закончен";
                mStatusBarNotificationDelivery.ProgressRingStart = false;

                //#8 p 6
                //ThemeManager.Current.ChangeThemeColorScheme(Application.Current, mCurrentColorScheme);

                return;
            }

            mIsWarningStackOwerflowAlreadyShow = false;
            mStatusBarNotificationDelivery.CompileLogMessage = "Сеанс отладки запущен";
            mStatusBarNotificationDelivery.ProgressRingStart = true;

            //#8 p 6
            //ThemeManager.Current.ChangeThemeColorScheme(Application.Current, cDebugColorScheme);

            ResultTextEditor = string.Empty;

            if (!Settings.Default.ShadowWorkspaceInDebug) return;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(async () =>
            {
                await mWebViewProvider.ExecuteJavaScript(Scripts.ShadowEnable);
            }));
        }

        private void UpdatePythonInfo(string pythonVersion = "", string pythonBinPath = "", string pythonWorkDir = "")
        {
            if (string.IsNullOrEmpty(pythonVersion))
            {
                PythonVersion = "Не подключена";
                PythonBinPath = string.Empty;
                PythonWorkDir = string.Empty;
                return;
            }

            PythonVersion = pythonVersion;
            PythonBinPath = $"[{pythonBinPath}]";
            PythonWorkDir = $"Рабочая дирректория {pythonWorkDir}";
        }

        private void RaiseDelegateCommandsCanExecuteChanged()
        {
            ReloadWebViewDelegateCommand.RaiseCanExecuteChanged();
            ShowSaveFileDialogDelegateCommand.RaiseCanExecuteChanged();
            ShowOpenFileDialogDelegateCommand.RaiseCanExecuteChanged();
            ShowSaveFileSourceTextDialogDelegateCommand.RaiseCanExecuteChanged();
            CleanExecuteEditorDelegateCommand.RaiseCanExecuteChanged();
            RunPythonCodeDelegateCommand.RaiseCanExecuteChanged();
            StopPythonCodeExecuteDelegateCommand.RaiseCanExecuteChanged();
            SendCodeToExternalSourceEditorDelegateCommand.RaiseCanExecuteChanged();
            ToZeroPositionDelegateCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Event methods

        private void RaiseWebViewNavigationCompleteEvent(object sender)
        {
            InitBlockly();
        }

        private void RaiseWebViewbMessageReceivedEvent(object sender, WebMessageJsonReceived webMessageReceived)
        {
            if (webMessageReceived.Action == "sendSourceCode")
            {
                SourceTextEditor = webMessageReceived.Data;
            }
        }

        private async void OnRaiseTcpServiceCientConnected(object sender)
        {
            IsTcpClientConnected = mCommunicationProvider.IsTcpClientConnected;

            var pythonVersionResult = await BaseApi.GetPythonVersion();
            var pythonBinPathResult = await BaseApi.GetPythonBinDir();
            var pythonWorkDirResult = await BaseApi.GetPythonWorkDir();

            string pythonVersion = pythonVersionResult?.StandardOutput?.Replace("\n", "");
            string pythonBinPath = pythonBinPathResult?.StandardOutput?.Replace("\n", "");
            string pythonWorkDir = pythonWorkDirResult?.StandardOutput?.Replace("\n", "");

            UpdatePythonInfo(pythonVersion, pythonBinPath, pythonWorkDir);
        }

        private void OnRaiseTcperviceClientDisconnect(object sender)
        {
            IsTcpClientConnected = mCommunicationProvider.IsTcpClientConnected;

            UpdatePythonInfo();
        }

        private  void OnRaisePythonScriptExecuteStart(object sender)
        {
            IsPythonCodeExecute = true;
        }

        private void OnRaisePythonStandartOutput(object sender, string message)
        {
            if (ResultTextEditorLength > 10000)
            {
                if (!mIsWarningStackOwerflowAlreadyShow)
                {
                    ResultTextEditor += "\nДальнейший вывод результата, будет скрыт.";
                    ResultTextEditor += "\nПрограмма продолжает выполняться в неинтерактивном режиме.";
                    ResultTextEditor += "\nДля остановки нажмите \"Stop\". Или дождитесь завершения.";

                    mIsWarningStackOwerflowAlreadyShow = true;
                }

                return;
            }

            ResultTextEditor += message;
        }

        private void OnRaisePythonScriptExecuteFinish(object sender, CommandExecuteResult remoteCommandExecuteResult)
        {
            IsPythonCodeExecute = false;

            string message = "\n======================\n<<Выполнение программы завершено>>";

            if (remoteCommandExecuteResult == null)
                return;

            string messageWithResult = $"{message}\n" +
                $"\n" +
                $"Отчет о выполнении\n" +
                $"======================\n" +
                $"Начало выполнения: {remoteCommandExecuteResult.StartTime}\n" +
                $"Завершение выполнения: {remoteCommandExecuteResult.EndTime}\n" +
                $"Общее время выполнения: {remoteCommandExecuteResult.RunTime}\n" +
                $"Код выхода: {remoteCommandExecuteResult.ExitCode}\n" +
                $"Статус успешности завершения: {remoteCommandExecuteResult.Succeeded}" +
                $"\n======================\n";

            if (!string.IsNullOrEmpty(remoteCommandExecuteResult.StandardError))
                messageWithResult += $"Ошибка: {remoteCommandExecuteResult.StandardError}" +
                    $"\n======================\n";

            ResultTextEditor += messageWithResult;
        }

        #endregion

        #region Initialize Blockly

        private async void InitBlockly()
        {
            BlocklyLanguage language = Settings.Default.BlocklyWorkspaceLanguage;

            try
            {
                await LoadBlocklySrc();
                await LoadBlocklyBlockLocalLangSrc(language);

                await mWebViewProvider.ExecuteJavaScript(InitWorkspace());
                await mWebViewProvider.ExecuteJavaScript(Scripts.ListenerCreatePythonCode);
                await mWebViewProvider.ExecuteJavaScript(Scripts.ListenerSavedBlocks);

                if (Settings.Default.BlocklyRestoreBlockOnLoad)
                {
                    await mWebViewProvider.ExecuteJavaScript(Scripts.RestoreSavedBlocks);
                }

                mStatusBarNotificationDelivery.AppLogMessage = "Загрузка скретч редактора завершена";
            }
            catch
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Загрузка скретч-редактора внезапно прервана";
            }
        }

        private static string InitWorkspace()
        {
            Toolbox toolbox = InitDefaultToolbox(Settings.Default.BlocklyToolboxLanguage);
            BlocklyGrid blocklyGrid = InitGrid();

            Workspace workspace = new()
            {
                Toolbox = toolbox,
                BlocklyGrid = blocklyGrid,
                Theme = Settings.Default.BlocklyTheme,
                ShowTrashcan = Settings.Default.BlocklyShowTrashcan,
                Render = Render.thrasos,
                Collapse = true
            };

            string workspaceString = Scripts.SerealizeObjectToJsonString("init", new object[] { workspace });
            return workspaceString;
        }

        private static BlocklyGrid InitGrid()
        {
            BlocklyGrid blocklyGrid = new();

            if (Settings.Default.BlocklyShowGrid)
            {
                blocklyGrid.Length = Settings.Default.BlocklyGridSpacing;
                blocklyGrid.Spacing = Settings.Default.BlocklyGridSpacing;
            }
            else
            {
                blocklyGrid.Length = 0;
                blocklyGrid.Spacing = 0;
            }

            blocklyGrid.Colour = Settings.Default.BlocklyGridColour.HexToRbgColor();
            blocklyGrid.Snap = Settings.Default.BlocklySnapToGridNodes;

            return blocklyGrid;
        }

        private static Toolbox InitDefaultToolbox(BlocklyLanguage language)
        {
            Toolbox toolbox = new DefaultSimpleCategoryToolbox(language)
            {
                LogicCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyLogicCategoryState,
                    AlternateName = Settings.Default.BlocklyLogicCategoryAlternateName
                },
                ColourCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyColorCategoryState,
                    AlternateName = Settings.Default.BlocklyColourCategoryAlternateName
                },
                ListsCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyListsCategoryState,
                    AlternateName = Settings.Default.BlocklyListsCategoryAlternateName
                },
                LoopsCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyLoopCategoryState,
                    AlternateName = Settings.Default.BlocklyLoopCategoryAlternateName
                },
                MathCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyMathCategoryState,
                    AlternateName = Settings.Default.BlocklyMathCategoryAlternateName
                },
                TextCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyTextCategoryState,
                    AlternateName = Settings.Default.BlocklyTextCategoryAlternateName
                },
                ProcedureCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyProcedureCategoryState,
                    AlternateName = Settings.Default.BlocklyProcedureCategoryAlternateName
                },
                VariableDynamicCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyDynamicVariableCategoryState,
                    AlternateName = Settings.Default.BlocklyDynamicVariableCategoryAlternateName
                },
                VariableCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyVariableCategoryState,
                    AlternateName = Settings.Default.BlocklyVariableCategoryAlternateName
                },
                DateTimeCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyDateTimeCategoryState,
                    AlternateName = Settings.Default.BlocklyDateTimeCategoryAlternateName
                },
                AdamCommonCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyAdamCommonCategoryState,
                    AlternateName = Settings.Default.BlocklyAdamCommonCategoryAlternateName
                },
                AdamTwoCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyAdamTwoCategoryState,
                    AlternateName = Settings.Default.BlocklyAdamTwoCategoryAlternateName
                },
                AdamThreeCategoryParam = new ToolboxParam
                {
                    Hidden = !Settings.Default.BlocklyAdamThreeCategoryState,
                    AlternateName = Settings.Default.BlocklyAdamThreeCategoryAlternateName
                }
            }.Toolbox;

            return toolbox;
        }

        #endregion

        #region Blockly method

        private async Task LoadBlocklySrc()
        {
            string loadLocalSrc = Scripts.SerealizeObject("loadSrcs", new object[]
            {
                Scripts.BlocksCompressedSrc,
                Scripts.JavascriptCompressedSrc,
                Scripts.PythonCompressedSrc,
            });

            string loadLocalAdamBlockSrc = Scripts.SerealizeObject("loadSrcs", new object[]
            {
                Scripts.DateTimeBlockSrc,
                Scripts.ThreadBlockSrc,
                Scripts.SystemsBlockSrc,
                Scripts.AdamThreeBlockSrc,
                Scripts.AdamTwoBlockSrc,
                Scripts.AdamCommonBlockSrc,
            });


            string loadLocalAdamPythonGenSrc = Scripts.SerealizeObject("loadSrcs", new object[]
            {
                Scripts.DateTimePytnonGenSrc,
                Scripts.ThreadPytnonGenSrc,
                Scripts.SystemsPythonGenSrc,
                Scripts.AdamThreePytnonGenSrc,
                Scripts.AdamTwoPytnonGenSrc,
                Scripts.AdamCommonPytnonGenSrc
            });

            await mWebViewProvider.ExecuteJavaScript(loadLocalSrc);
            await mWebViewProvider.ExecuteJavaScript(loadLocalAdamBlockSrc);
            await mWebViewProvider.ExecuteJavaScript(loadLocalAdamPythonGenSrc);
        }

        /// <summary>
        /// Loading block language
        /// </summary>
        /// <param name="language"></param>
        private async Task LoadBlocklyBlockLocalLangSrc(BlocklyLanguage language)
        {
            switch (language)
            {
                case BlocklyLanguage.ru:
                    {
                        _ = await ExecuteScriptFunctionAsync("loadSrc", Scripts.BlockLanguageRu);
                        break;
                    }
                case BlocklyLanguage.zh:
                    {
                        _ = await ExecuteScriptFunctionAsync("loadSrc", Scripts.BlockLanguageZnHans);
                        break;
                    }
                case BlocklyLanguage.en:
                    {
                        _ = await ExecuteScriptFunctionAsync("loadSrc", Scripts.BlockLanguageEn);
                        break;
                    }
            }
        }

        #endregion

        #region ExecuteScripts

        private async Task<string> ExecuteScriptFunctionAsync(string functionName, params object[] parameters)
        {
            string script = Scripts.SerealizeObject(functionName, parameters);
            return await mWebViewProvider.ExecuteJavaScript(script);
        }

        #endregion

    }
}

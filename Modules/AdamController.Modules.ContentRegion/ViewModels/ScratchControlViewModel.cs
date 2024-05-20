﻿using AdamBlocklyLibrary;
using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using AdamBlocklyLibrary.ToolboxSets;
using AdamController.Core;
using AdamController.Core.Extensions;
using AdamController.Core.Mvvm;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using AdamController.Services.SystemDialogServiceDependency;
using AdamController.Services.WebViewProviderDependency;
using AdamController.WebApi.Client.v1.ResponseModel;
using ICSharpCode.AvalonEdit.Highlighting;
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
        public DelegateCommand CopyToClipboardDelegateCommand { get; }
        public DelegateCommand ReloadWebViewDelegateCommand { get; }
        public DelegateCommand ShowSaveFileDialogDelegateCommand { get; }
        public DelegateCommand ShowOpenFileDialogDelegateCommand { get; }
        public DelegateCommand ShowSaveFileSourceTextDialogDelegateCommand { get; }
        public DelegateCommand CleanExecuteEditorDelegateCommand { get; }
        public DelegateCommand RunPythonCodeDelegateCommand { get; }
        public DelegateCommand StopPythonCodeExecuteDelegateCommand { get; }
        public DelegateCommand ToZeroPositionDelegateCommand { get; } 

        #endregion

        #region Services

        private readonly ICommunicationProviderService mCommunicationProvider;
        private readonly IPythonRemoteRunnerService mPythonRemoteRunner;
        private readonly IStatusBarNotificationDeliveryService mStatusBarNotificationDelivery;
        private readonly IWebViewProvider mWebViewProvider;
        private readonly IDialogManagerService mDialogManager;
        private readonly IFileManagmentService mFileManagment;
        private readonly IWebApiService mWebApiService;
        private readonly ICultureProvider mCultureProvider;
        private readonly ISystemDialogService mSystemDialogService;
        #endregion

        #region Const

        private const string cFilter = "XML documents (.xml) | *.xml";

        #endregion

        #region Var

        private bool mIsWarningStackOwerflowAlreadyShow;

        private string mCompileLogMessageStartDebug; 
        private string mCompileLogMessageEndDebug;
        private string mFinishAppExecute;
        private string mWarningStackOwerflow1;
        private string mWarningStackOwerflow2;
        private string mWarningStackOwerflow3;

        private string mSaveFileDialogDialogTitle;
        private string mSaveScriptFileDialogDialogTitle;

        private string mOpenFileDialogDialogTitle;
        private string mFileNotSavedLogMessage;
        private string mFileNotSelectedLogMessage;
        private string mSaveFileDialogFilter;
        private string mFileSavedLogMessage;

        private string mScretchLoadedCompleteLogMessage;
        private string mScretchLoadedErrorLogMessage;
        

        #endregion

        #region ~

        public ScratchControlViewModel(IRegionManager regionManager, ICommunicationProviderService communicationProvider, IPythonRemoteRunnerService pythonRemoteRunner, 
                        IStatusBarNotificationDeliveryService statusBarNotificationDelivery, IWebViewProvider webViewProvider, IDialogManagerService dialogManager, 
                        IFileManagmentService fileManagment, IWebApiService webApiService, IAvalonEditService avalonEditService,
                        ICultureProvider cultureProvider, ISystemDialogService systemDialogService) : base(regionManager)
        {
            
            mCommunicationProvider = communicationProvider;
            mPythonRemoteRunner = pythonRemoteRunner;
            mStatusBarNotificationDelivery = statusBarNotificationDelivery;
            mWebViewProvider = webViewProvider;
            mDialogManager = dialogManager;
            mFileManagment = fileManagment;
            mWebApiService = webApiService;
            mCultureProvider = cultureProvider;
            mSystemDialogService = systemDialogService;

            CopyToClipboardDelegateCommand = new DelegateCommand(CopyToClipboard, CopyToClipboardCanExecute);
            ReloadWebViewDelegateCommand = new DelegateCommand(ReloadWebView, ReloadWebViewCanExecute);
            ShowSaveFileDialogDelegateCommand = new DelegateCommand(ShowSaveFileDialog, ShowSaveFileDialogCanExecute);
            ShowOpenFileDialogDelegateCommand = new DelegateCommand(ShowOpenFileDialog, ShowOpenFileDialogCanExecute);
            ShowSaveFileSourceTextDialogDelegateCommand = new DelegateCommand(ShowSaveFileSourceTextDialog, ShowSaveFileSourceTextDialogCanExecute);
            CleanExecuteEditorDelegateCommand = new DelegateCommand(CleanExecuteEditor, CleanExecuteEditorCanExecute);
            RunPythonCodeDelegateCommand = new DelegateCommand(RunPythonCode, RunPythonCodeCanExecute);
            StopPythonCodeExecuteDelegateCommand = new DelegateCommand(StopPythonCodeExecute, StopPythonCodeExecuteCanExecute);
            ToZeroPositionDelegateCommand = new DelegateCommand(ToZeroPosition, ToZeroPositionCanExecute);

            HighlightingDefinition = avalonEditService.GetDefinition(HighlightingName.AdamPython);

            LoadResources();
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
            
            //#29
            mWebViewProvider.ReloadWebView();

            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Unsubscribe();

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

        private string resultText;
        public string ResultText
        {
            get => resultText;
            set 
            {
                bool isNewValue = SetProperty(ref resultText, value);

                if(isNewValue)
                    CleanExecuteEditorDelegateCommand.RaiseCanExecuteChanged();
            } 
        }

        private ExtendedCommandExecuteResult resultExecutionTime;
        public ExtendedCommandExecuteResult ResultExecutionTime
        {
            get => resultExecutionTime;
            set => SetProperty(ref resultExecutionTime, value);    
        }

        private ExtendedCommandExecuteResult resultInitilizationTime;
        public ExtendedCommandExecuteResult ResultInitilizationTime
        {
            get => resultInitilizationTime;
            set => SetProperty(ref resultInitilizationTime, value);
        }

        private string pythonVersion;
        public string PythonVersion
        {
            get => pythonVersion;
            set => SetProperty(ref pythonVersion, value);
        }

        /*private string pythonBinPath;
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
        }*/

        private IHighlightingDefinition highlightingDefinition;
        public IHighlightingDefinition HighlightingDefinition
        {
            get => highlightingDefinition;
            set => SetProperty(ref highlightingDefinition, value);
        }

        #endregion

        #region Subscribes

        private void Subscribe()
        {
            mCommunicationProvider.RaiseTcpServiceCientConnectedEvent += OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnectEvent += OnRaiseTcpServiceClientDisconnect;

            mPythonRemoteRunner.RaisePythonScriptExecuteStartEvent += OnRaisePythonScriptExecuteStart;
            mPythonRemoteRunner.RaisePythonStandartOutputEvent += OnRaisePythonStandartOutput;
            mPythonRemoteRunner.RaisePythonScriptExecuteFinishEvent += OnRaisePythonScriptExecuteFinish;

            mWebViewProvider.RaiseWebViewMessageReceivedEvent += RaiseWebViewbMessageReceivedEvent;
            mWebViewProvider.RaiseWebViewNavigationCompleteEvent += RaiseWebViewNavigationCompleteEvent;

            mCultureProvider.RaiseCurrentAppCultureLoadOrChangeEvent += RaiseCurrentAppCultureLoadOrChangeEvent;
        }

        private void Unsubscribe()
        {
            mCommunicationProvider.RaiseTcpServiceCientConnectedEvent -= OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnectEvent -= OnRaiseTcpServiceClientDisconnect;

            mPythonRemoteRunner.RaisePythonScriptExecuteStartEvent -= OnRaisePythonScriptExecuteStart;
            mPythonRemoteRunner.RaisePythonStandartOutputEvent -= OnRaisePythonStandartOutput;
            mPythonRemoteRunner.RaisePythonScriptExecuteFinishEvent -= OnRaisePythonScriptExecuteFinish;

            mWebViewProvider.RaiseWebViewMessageReceivedEvent -= RaiseWebViewbMessageReceivedEvent;
            mWebViewProvider.RaiseWebViewNavigationCompleteEvent -= RaiseWebViewNavigationCompleteEvent;

            mCultureProvider.RaiseCurrentAppCultureLoadOrChangeEvent -= RaiseCurrentAppCultureLoadOrChangeEvent;
        }

        #endregion

        #region DelegateCommands methods

        private void CopyToClipboard()
        {
            Clipboard.SetText(SourceTextEditor);
        }

        private bool CopyToClipboardCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            bool isSourceNotEmpty = SourceTextEditor?.Length > 0;

            return isPythonCodeNotExecute && isSourceNotEmpty;
        }
        
        private void ReloadWebView()
        {
            mWebViewProvider.ReloadWebView();
        }

        private bool ReloadWebViewCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            return isPythonCodeNotExecute;
        }

        private async void ShowSaveFileDialog()
        {
            string workspace = await mWebViewProvider.ExecuteJavaScript("getSavedWorkspace()", true);
            string initialPath = Settings.Default.SavedUserWorkspaceFolderPath;
            string fileName = "workspace";
            string defaultExt = ".xml";
            
            if (mDialogManager.ShowSaveFileDialog(mSaveFileDialogDialogTitle, initialPath, fileName, defaultExt, cFilter))
            {
                string path = mDialogManager.FilePathToSave;
                await mFileManagment.WriteAsync(path, workspace);

                mStatusBarNotificationDelivery.AppLogMessage = $"{mFileSavedLogMessage} {mDialogManager.FilePathToSave}";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = mFileNotSavedLogMessage;
            }
        }

        private bool ShowSaveFileDialogCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            bool isSourceNotEmpty = SourceTextEditor?.Length > 0;

            return isPythonCodeNotExecute && isSourceNotEmpty;
        }

        private void ShowOpenFileDialog()
        {
            string initialPath = Settings.Default.SavedUserWorkspaceFolderPath;
            string title = "Открыть скрипт или рабочую область";

            var dialogParametrs = new DialogParameters
            {
                { DialogParametrsKeysName.TitleParametr, title },
                { DialogParametrsKeysName.InitialDirectoryParametr, initialPath}
            };

            OpenFileDialogResult result = mSystemDialogService.ShowOpenFileDialog(dialogParametrs);

            if (result.IsOpenFileCanceled)
            {
                mStatusBarNotificationDelivery.AppLogMessage = mFileNotSelectedLogMessage;
                return;
            }

            OpenSupportedFile(result);
        }

        private async void OpenSupportedFile(OpenFileDialogResult result)
        {
            string path = result.OpenFilePath;

            switch (result.OpenFileType)
            {
                case OpenFileType.Undefined:
                    mStatusBarNotificationDelivery.AppLogMessage = $"File unsupported {path}";
                    break;
                case OpenFileType.Script:
                    SourceTextEditor = await mFileManagment.ReadTextAsStringAsync(path);
                    mStatusBarNotificationDelivery.AppLogMessage = $"{mFileSavedLogMessage} {path}";
                    break;
                case OpenFileType.Workspace:
                    string xml = await mFileManagment.ReadTextAsStringAsync(path);
                    _ = await ExecuteScriptFunctionAsync("loadSavedWorkspace", new object[] { xml });
                    mStatusBarNotificationDelivery.AppLogMessage = $"{mFileSavedLogMessage} {path}";
                    break;

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
            string initialPath = Settings.Default.SavedUserScriptsFolderPath;
            string fileName = "new_program";
            string defaultExt = ".py";


            if (mDialogManager.ShowSaveFileDialog(mSaveScriptFileDialogDialogTitle, initialPath, fileName, defaultExt, mSaveFileDialogFilter))
            {
                string path = mDialogManager.FilePathToSave;
                await mFileManagment.WriteAsync(path, pythonProgram);

                mStatusBarNotificationDelivery.AppLogMessage = $"{mFileSavedLogMessage} {mDialogManager.FilePathToSave}";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = mFileNotSavedLogMessage;
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
            ClearResultText();   
        }

        private bool CleanExecuteEditorCanExecute()
        {
            bool isPythonCodeNotExecute = !IsPythonCodeExecute;
            var isResultNotEmpty = ResultText?.Length > 0;
            return isPythonCodeNotExecute &&  isResultNotEmpty;
        }

        private async void RunPythonCode()
        {

            string source = SourceTextEditor;
            
            try
            {
                ClearResultText();

                var command = new WebApi.Client.v1.RequestModel.PythonCommandModel
                {
                    Command = source
                };

                ExtendedCommandExecuteResult executeResult = await mWebApiService.PythonExecuteAsync(command);
                UpdateResultInitilizationTimeText(executeResult);
            }
            catch
            {
                
            }
            finally
            {
               
            }
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
                await mWebApiService.StopPythonExecute();
            }
            catch
            {

            }
        }

        private bool StopPythonCodeExecuteCanExecute()
        {
            bool isConnected = IsTcpClientConnected;
            bool isPythonCodeExecute = IsPythonCodeExecute;

            return isConnected && isPythonCodeExecute;
        }

        private async void ToZeroPosition()
        {
            try
            {
                await mWebApiService.StopPythonExecute();
                await mWebApiService.MoveToZeroPosition();
            }
            catch
            {

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

        private void UpdateResultText(string text, bool isFinishMessage = false)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                if (isFinishMessage)
                {
                    ResultText += "\n======================\n";
                    ResultText += $"<<{mFinishAppExecute}>>\n";
                }

                if(!isFinishMessage)
                {
                    if (ResultText?.Length > 500)
                    {
                        if (!mIsWarningStackOwerflowAlreadyShow)
                        {
                            string warningMessage = $"\n{mWarningStackOwerflow1}\n{mWarningStackOwerflow2}\n{mWarningStackOwerflow3}\n";
                            ResultText += warningMessage;
                            mIsWarningStackOwerflowAlreadyShow = true;
                        }

                        return;
                    }

                    ResultText += text;
                }
            }));
        }

        private void UpdateResultExecutionTimeText(ExtendedCommandExecuteResult executeResult)
        {
            
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                ExtendedCommandExecuteResult fixResult = new()
                {
                    StandardOutput = executeResult.StandardOutput,
                    StandardError = executeResult.StandardError,

                    StartTime = executeResult.StartTime,
                    EndTime = executeResult.EndTime,
                    RunTime = executeResult.RunTime,

                    ExitCode = executeResult.ExitCode,

                    // The server always returns False
                    // Therefore, the success of completion is determined by the exit code
                    Succeesed = executeResult.ExitCode == 0
                };

                ResultExecutionTime = fixResult;
                IsPythonCodeExecute = false;
            }));
        }

        private void UpdateResultInitilizationTimeText(ExtendedCommandExecuteResult executeResult)
        {
            ResultInitilizationTime = executeResult;
        }

        private void ClearResultText()
        {
            ResultText = string.Empty;
            ResultExecutionTime = null;
            ResultInitilizationTime = null;
        }

        private void OnPythonCodeExecuteStatusChange(bool isPythonCodeExecute)
        {
            
            if (!isPythonCodeExecute)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(async () =>
                {
                    await mWebViewProvider.ExecuteJavaScript(Scripts.ShadowDisable);
                }));

                mStatusBarNotificationDelivery.CompileLogMessage = mCompileLogMessageEndDebug;
                mStatusBarNotificationDelivery.ProgressRingStart = false;
                return;
            }

            mIsWarningStackOwerflowAlreadyShow = false;
            mStatusBarNotificationDelivery.CompileLogMessage = mCompileLogMessageStartDebug;
            mStatusBarNotificationDelivery.ProgressRingStart = true;


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
                PythonVersion = string.Empty;
                //PythonBinPath = string.Empty;
                //PythonWorkDir = string.Empty;
                return;
            }

            PythonVersion = pythonVersion;
            //PythonBinPath = $"[{pythonBinPath}]";
            //PythonWorkDir = $"Рабочая дирректория {pythonWorkDir}";
        }

        private void RaiseDelegateCommandsCanExecuteChanged()
        {
            //MoveSplitterDelegateCommand.RaiseCanExecuteChanged();
            CopyToClipboardDelegateCommand.RaiseCanExecuteChanged();
            ReloadWebViewDelegateCommand.RaiseCanExecuteChanged();
            ShowSaveFileDialogDelegateCommand.RaiseCanExecuteChanged();
            ShowOpenFileDialogDelegateCommand.RaiseCanExecuteChanged();
            ShowSaveFileSourceTextDialogDelegateCommand.RaiseCanExecuteChanged();
            CleanExecuteEditorDelegateCommand.RaiseCanExecuteChanged();
            RunPythonCodeDelegateCommand.RaiseCanExecuteChanged();
            StopPythonCodeExecuteDelegateCommand.RaiseCanExecuteChanged();
            ToZeroPositionDelegateCommand.RaiseCanExecuteChanged();
        }

        private void LoadResources()
        {
            mCompileLogMessageStartDebug = mCultureProvider.FindResource("DebuggerMessages.CompileLogMessageStartDebug");
            mCompileLogMessageEndDebug = mCultureProvider.FindResource("DebuggerMessages.CompileLogMessageEndDebug");
            mFinishAppExecute = mCultureProvider.FindResource("DebuggerMessages.ResultMessages.FinishAppExecute");

            mWarningStackOwerflow1 = mCultureProvider.FindResource("DebuggerMessages.ResultMessages.WarningStackOwerflow1");
            mWarningStackOwerflow2 = mCultureProvider.FindResource("DebuggerMessages.ResultMessages.WarningStackOwerflow2");
            mWarningStackOwerflow3 = mCultureProvider.FindResource("DebuggerMessages.ResultMessages.WarningStackOwerflow3");

            mSaveFileDialogDialogTitle = mCultureProvider.FindResource("ScratchControlViewModel.SaveFileDialog.DialogTitle");
            mOpenFileDialogDialogTitle = mCultureProvider.FindResource("ScratchControlViewModel.OpenFileDialog.DialogTitle");
            mFileNotSavedLogMessage = mCultureProvider.FindResource("ScratchControlViewModel.Dialogs.FileNotSaved.LogMessage");
            mFileNotSelectedLogMessage = mCultureProvider.FindResource("ScratchControlViewModel.Dialogs.FileNotSelected.LogMessage");
            mSaveFileDialogFilter = mCultureProvider.FindResource("ScratchControlViewModel.SaveFileDialog.Filter");

            mSaveScriptFileDialogDialogTitle = mCultureProvider.FindResource("ScratchControlViewModel.SaveFileDialog.DialogTitle2");
            mFileSavedLogMessage = mCultureProvider.FindResource("ScratchControlViewModel.Dialogs.FileSaved.LogMessage");
            mScretchLoadedCompleteLogMessage = mCultureProvider.FindResource("ScratchControlViewModel.ScretchLoadedComplete.LogMessage");
            mScretchLoadedErrorLogMessage = mCultureProvider.FindResource("ScratchControlViewModel.ScretchLoadedError.LogMessage");
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

            var pythonVersionResult = await mWebApiService.GetPythonVersion();
            var pythonBinPathResult = await mWebApiService.GetPythonBinDir();
            var pythonWorkDirResult = await mWebApiService.GetPythonWorkDir();

            string pythonVersion = pythonVersionResult?.StandardOutput?.Replace("\n", "");
            string pythonBinPath = pythonBinPathResult?.StandardOutput?.Replace("\n", "");
            string pythonWorkDir = pythonWorkDirResult?.StandardOutput?.Replace("\n", "");

            UpdatePythonInfo(pythonVersion, pythonBinPath, pythonWorkDir);
        }

        private void OnRaiseTcpServiceClientDisconnect(object sender, bool isUserRequest)
        {
            IsTcpClientConnected = mCommunicationProvider.IsTcpClientConnected;

            UpdatePythonInfo();
        }

        private void OnRaisePythonScriptExecuteStart(object sender)
        {
            IsPythonCodeExecute = true;
        }

        private void OnRaisePythonStandartOutput(object sender, string message)
        {
            UpdateResultText(message);
        }

        private void OnRaisePythonScriptExecuteFinish(object sender, ExtendedCommandExecuteResult remoteCommandExecuteResult)
        {
            if (remoteCommandExecuteResult == null)
                return;
   
            UpdateResultText("", true);
            UpdateResultExecutionTimeText(remoteCommandExecuteResult);
        }

        private void RaiseCurrentAppCultureLoadOrChangeEvent(object sender)
        {
            LoadResources();
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

                mStatusBarNotificationDelivery.AppLogMessage = mScretchLoadedCompleteLogMessage;
            }
            catch
            {
                mStatusBarNotificationDelivery.AppLogMessage = mScretchLoadedErrorLogMessage;
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

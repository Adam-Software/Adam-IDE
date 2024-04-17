using AdamBlocklyLibrary;
using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using AdamBlocklyLibrary.ToolboxSets;
using AdamController.Core;
using AdamController.Core.Helpers;
using AdamController.Core.Mvvm;
using AdamController.Core.Properties;
using AdamController.Modules.ContentRegion.Views;
using AdamController.Services.Interfaces;
using AdamController.Services.WebViewProviderDependency;
using AdamController.WebApi.Client.v1;
using ControlzEx.Standard;
using MessageDialogManagerLib;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ScratchControlViewModel : RegionViewModelBase 
    {

        #region Services

        private readonly ICommunicationProviderService mCommunicationProvider;
        private readonly IPythonRemoteRunnerService mPythonRemoteRunner;
        private readonly IStatusBarNotificationDeliveryService mStatusBarNotificationDelivery;
        private readonly IWebViewProvider mWebViewProvider;


        #endregion

        #region Action field 

        public static Action<string> SendSourceToScriptEditor { get; set; }
        //public Action ReloadWebView { get; set; }

        #endregion

        private readonly IMessageDialogManager IDialogManager;
        private bool mIsWarningStackOwerflowAlreadyShow;

        public ScratchControlViewModel(IRegionManager regionManager, IDialogService dialogService, ICommunicationProviderService communicationProvider, 
            IPythonRemoteRunnerService pythonRemoteRunner, IStatusBarNotificationDeliveryService statusBarNotificationDelivery, IWebViewProvider webViewProvider) : base(regionManager, dialogService)
        {
            mCommunicationProvider = communicationProvider;
            mPythonRemoteRunner = pythonRemoteRunner;
            mStatusBarNotificationDelivery = statusBarNotificationDelivery;
            mWebViewProvider = webViewProvider;

            IDialogManager = new MessageDialogManagerMahapps(Application.Current);

            //InitAction();
        }

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            mCommunicationProvider.RaiseTcpServiceCientConnectedEvent += OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnectEvent += OnRaiseTcperviceClientDisconnect;

            mPythonRemoteRunner.RaisePythonScriptExecuteStartEvent += OnRaisePythonScriptExecuteStart;
            mPythonRemoteRunner.RaisePythonStandartOutputEvent += OnRaisePythonStandartOutput;
            mPythonRemoteRunner.RaisePythonScriptExecuteFinishEvent += OnRaisePythonScriptExecuteFinish;

            mWebViewProvider.RaiseWebViewMessageReceivedEvent += RaiseWebViewbMessageReceivedEvent;
            mWebViewProvider.RaiseWebViewNavigationCompleteEvent += RaiseWebViewNavigationCompleteEvent;
            //ReloadWebViewCommand.Execute();

            base.OnNavigatedTo(navigationContext);
        }

        private void RaiseWebViewNavigationCompleteEvent(object sender)
        {
            InitBlockly();
            //throw new NotImplementedException();
        }

        private void RaiseWebViewbMessageReceivedEvent(object sender, WebMessageJsonReceived webMessageReceived)
        {
            if (webMessageReceived.Action == "sendSourceCode")
            {
                SourceTextEditor = webMessageReceived.Data;
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            mCommunicationProvider.RaiseTcpServiceCientConnectedEvent -= OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnectEvent -= OnRaiseTcperviceClientDisconnect;

            mPythonRemoteRunner.RaisePythonScriptExecuteStartEvent -= OnRaisePythonScriptExecuteStart;
            mPythonRemoteRunner.RaisePythonStandartOutputEvent -= OnRaisePythonStandartOutput;
            mPythonRemoteRunner.RaisePythonScriptExecuteFinishEvent -= OnRaisePythonScriptExecuteFinish;

            mWebViewProvider.RaiseWebViewMessageReceivedEvent -= RaiseWebViewbMessageReceivedEvent;
            mWebViewProvider.RaiseWebViewNavigationCompleteEvent -= RaiseWebViewNavigationCompleteEvent;

            base.OnNavigatedFrom(navigationContext);
        }


        #endregion

        #region Event methods

        private async void OnRaiseTcpServiceCientConnected(object sender)
        {
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
            UpdatePythonInfo();
        }


        private void OnRaisePythonScriptExecuteStart(object sender)
        {
            mIsWarningStackOwerflowAlreadyShow = false;
            StartExecuteProgram();
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

        private void OnRaisePythonScriptExecuteFinish(object sender, string message)
        {
            FinishExecuteProgram();
            ResultTextEditor += message;
        }

        #endregion

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

        #region Action initialize

        //private void InitAction()
        //{
        //    ScratchControlView.NavigationComplete ??= new Action(NavigationComplete);
        //    ScratchControlView.WebMessageReceived ??= new Action<WebMessageJsonReceived>(WebMessageReceived);
        //}

        #endregion

        #region Execute program event

        private async void StartExecuteProgram()
        {
            mStatusBarNotificationDelivery.CompileLogMessage = "Сеанс отладки запущен";
            mStatusBarNotificationDelivery.ProgressRingStart = true;

            IsEnabledShowOpenDialogButton = false;
            IsEnabledStopExecuteButton = true;
            ResultTextEditor = string.Empty;
            

            if (!Settings.Default.ShadowWorkspaceInDebug) return;

            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(async () =>
            {
                await mWebViewProvider.ExecuteJavaScript(Scripts.ShadowEnable);
                //await ScratchControlView.ExecuteScript(Scripts.ShadowEnable);
            }));
        }

        private void FinishExecuteProgram()
        {
            OnStopExecuteProgram("Сеанс отладки закончен");
        }

        private async void OnStopExecuteProgram(string compileLogStatusBarAction)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(async () =>
            {
                await mWebViewProvider.ExecuteJavaScript(Scripts.ShadowDisable);
                //await ScratchControlView.ExecuteScript(Scripts.ShadowDisable);
            }));

            mStatusBarNotificationDelivery.CompileLogMessage = compileLogStatusBarAction;
            mStatusBarNotificationDelivery.ProgressRingStart = false;

            IsEnabledShowOpenDialogButton = true;
            IsEnabledStopExecuteButton = false;
        }

        #endregion

        //private void WebMessageReceived(WebMessageJsonReceived results)
        //{
        //    if (results.Action == "sendSourceCode")
        //    {
        //        SourceTextEditor = results.Data;
        //    }
        //}

        #region IsEnabled buttons field

        private bool isEnabledStopExecuteButton = false;
        public bool IsEnabledStopExecuteButton
        {
            get => isEnabledStopExecuteButton;
            set => SetProperty(ref isEnabledStopExecuteButton, value);
        }

        private bool isEnabledShowOpenDialogButton = true;
        public bool IsEnabledShowOpenDialogButton
        {
            get => isEnabledShowOpenDialogButton;
            set => SetProperty(ref isEnabledShowOpenDialogButton, value);
        }

        #endregion

        #region Initialize Blockly

        /// <summary>
        /// Run js-script after web view navigation complete 
        /// </summary>
        //private void NavigationComplete()
        //{
        //    InitBlockly();
       // }

        private async void InitBlockly()
        {
            BlocklyLanguage language = Settings.Default.BlocklyWorkspaceLanguage;

            try
            {
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(async () =>
                {
                    await LoadBlocklySrc();
                    await LoadBlocklyBlockLocalLangSrc(language);

                    await mWebViewProvider.ExecuteJavaScript(InitWorkspace());
                    await mWebViewProvider.ExecuteJavaScript(Scripts.ListenerCreatePythonCode);
                    await mWebViewProvider.ExecuteJavaScript(Scripts.ListenerSavedBlocks);

                    //_ = await ScratchControlView.ExecuteScript(InitWorkspace());
                    //_ = await ScratchControlView.ExecuteScript(Scripts.ListenerCreatePythonCode);
                    //_ = await ScratchControlView.ExecuteScript(Scripts.ListenerSavedBlocks);

                    if (Settings.Default.BlocklyRestoreBlockOnLoad)
                    {
                        await mWebViewProvider.ExecuteJavaScript(Scripts.RestoreSavedBlocks);
                        //_ = await ScratchControlView.ExecuteScript(Scripts.RestoreSavedBlocks);
                    }


                }));

                mStatusBarNotificationDelivery.AppLogMessage = "Загрузка скретч редактора завершена";
            }
            catch
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Загрузка скретч-редактора внезапно прервана";
            }
        }

        private string InitWorkspace()
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

        private BlocklyGrid InitGrid()
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

            blocklyGrid.Colour = Settings.Default.BlocklyGridColour.ToRbgColor();
            blocklyGrid.Snap = Settings.Default.BlocklySnapToGridNodes;

            return blocklyGrid;
        }

        private Toolbox InitDefaultToolbox(BlocklyLanguage language)
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
            //_ = await ScratchControlView.ExecuteScript(loadLocalSrc);

            //Thread.Sleep(1000);
            //Thread.Sleep(500);
            await mWebViewProvider.ExecuteJavaScript(loadLocalAdamBlockSrc);
            //_ = await ScratchControlView.ExecuteScript(loadLocalAdamBlockSrc);

            //Thread.Sleep(1000);
            //Thread.Sleep(500);
            await mWebViewProvider.ExecuteJavaScript(loadLocalAdamPythonGenSrc);
            //_ = await ScratchControlView.ExecuteScript(loadLocalAdamPythonGenSrc);
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

        #region Result text editor

        private string resultTextEditor;
        public string ResultTextEditor
        {
            get => resultTextEditor;
            set
            {
                bool isNewValue = SetProperty(ref resultTextEditor, value);
                
                if (isNewValue)
                    ResultTextEditorLength = ResultTextEditor.Length;
            }
        }

        #endregion

        #region Result text editor length

        private int resultTextEditorLength;
        public int ResultTextEditorLength
        {
            get => resultTextEditorLength;
            set => SetProperty(ref resultTextEditorLength, value);
        }

        #endregion

        #region Result text editor panel field

        private string resultTextEditorError;
        public string ResultTextEditorError
        {
            get => resultTextEditorError;
            set
            {
                
                if (value == resultTextEditorError) return;

                if (value.Length > 0)
                    resultTextEditorError = $"Error: {value}";
                else
                    resultTextEditorError = value;

                SetProperty(ref resultTextEditorError, value);
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

        #region Source text editor

        private string sourceTextEditor;
        public string SourceTextEditor
        {
            get => sourceTextEditor;
            set => SetProperty(ref sourceTextEditor, value);
        }

        #endregion

        #region Command

        private DelegateCommand reloadWebViewCommand;
        public DelegateCommand ReloadWebViewCommand => reloadWebViewCommand ??= new DelegateCommand(() =>
        {
            SourceTextEditor = string.Empty;
            mWebViewProvider.ReloadWebView();
            //ReloadWebView();
        });

        private DelegateCommand sendToExternalSourceEditor;
        public DelegateCommand SendToExternalSourceEditor => sendToExternalSourceEditor ??= new DelegateCommand(() =>
        {
            SendSourceToScriptEditor(SourceTextEditor);

        }, () => SourceTextEditor?.Length > 0);

        private DelegateCommand showSaveFileDialogCommand;
        public DelegateCommand ShowSaveFileDialogCommand => showSaveFileDialogCommand ??= new DelegateCommand(async () =>
        {
            string workspace = await mWebViewProvider.ExecuteJavaScript("getSavedWorkspace()");
            //string workspace = await ScratchControlView.ExecuteScript("getSavedWorkspace()");
            string xmlWorkspace = JsonConvert.DeserializeObject<dynamic>(workspace);

            if (IDialogManager.ShowSaveFileDialog("Сохранить рабочую область", Settings.Default.SavedUserWorkspaceFolderPath, "workspace", ".xml", "XML documents (.xml)|*.xml"))
            {
                string path = IDialogManager.FilePathToSave;
                await FileHelper.WriteAsync(path, xmlWorkspace);

                mStatusBarNotificationDelivery.AppLogMessage = $"Файл {IDialogManager.FilePathToSave} сохранен";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Файл не сохранен";
            }
        });

        private DelegateCommand showOpenFileDialogCommand;
        public DelegateCommand ShowOpenFileDialogCommand => showOpenFileDialogCommand ??= new DelegateCommand(async () =>
            {
                if (IDialogManager.ShowFileBrowser("Выберите сохранененную рабочую область", Settings.Default.SavedUserWorkspaceFolderPath, "XML documents(.xml) | *.xml"))
                {
                    string path = IDialogManager.FilePath;
                    if (path == "") return;

                    string xml = await FileHelper.ReadTextAsStringAsync(path);
                    _ = await ExecuteScriptFunctionAsync("loadSavedWorkspace", new object[] { xml });

                    mStatusBarNotificationDelivery.AppLogMessage = $"Файл {path} загружен";
                }
                else
                {
                    mStatusBarNotificationDelivery.AppLogMessage = "Файл рабочей области не выбран";
                }
            });

        private DelegateCommand runCode;
        public DelegateCommand RunCode => runCode ??= new DelegateCommand(async () =>
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

        }, () => true/*!string.IsNullOrEmpty(SourceTextEditor)*/ && true /*mCommunicationProvider.IsTcpClientConnected*/);

        private DelegateCommand stopExecute;
        public DelegateCommand StopExecute => stopExecute ??= new DelegateCommand( async () =>
        {
            if (mCommunicationProvider.IsTcpClientConnected)
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
        });

        private DelegateCommand toZeroPositionCommand;
        public DelegateCommand ToZeroPositionCommand => toZeroPositionCommand ??= new DelegateCommand(async () =>
        {
            try
            {
                await BaseApi.StopPythonExecute();
            }
            catch (Exception ex)
            {
                ResultTextEditorError = ex.Message.ToString();
            }

        }, () => mCommunicationProvider.IsTcpClientConnected);

        private DelegateCommand cleanExecuteEditor;
        public DelegateCommand CleanExecuteEditor => cleanExecuteEditor ??= new DelegateCommand(async () =>
        {
            await Task.Run(() => 
            {
                ResultTextEditorError = string.Empty;
                ResultTextEditor = string.Empty;
            });
        });

        #region ShowSaveFileDialogCommand

        private DelegateCommand showSaveFileSourceTextDialogCommand;
        public DelegateCommand ShowSaveFileSourceTextDialogCommand => showSaveFileSourceTextDialogCommand ??= new DelegateCommand(async () =>
        {
            string pythonProgram = SourceTextEditor;

            if (IDialogManager.ShowSaveFileDialog("Сохранить файл программы", Core.Properties.Settings.Default.SavedUserScriptsFolderPath,
                "new_program", ".py", "PythonScript file (.py)|*.py|Все файлы|*.*"))
            {
                string path = IDialogManager.FilePathToSave;
                await FileHelper.WriteAsync(path, pythonProgram);

                mStatusBarNotificationDelivery.AppLogMessage = $"Файл {IDialogManager.FilePathToSave} сохранен";
            }
            else
            {
                mStatusBarNotificationDelivery.AppLogMessage = "Файл не сохранен";
            }
        }, () => SourceTextEditor?.Length > 0);

        #endregion

        #endregion

        #region ExecuteScripts

        private async Task<string> ExecuteScriptFunctionAsync(string functionName, params object[] parameters)
        {
            string script = Scripts.SerealizeObject(functionName, parameters);
            return await mWebViewProvider.ExecuteJavaScript(script);
            //return await ScratchControlView.ExecuteScript(script);
        }

        #endregion

    }
}

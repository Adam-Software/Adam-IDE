using AdamBlocklyLibrary;
using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using AdamBlocklyLibrary.ToolboxSets;
using AdamController.Commands;
using AdamController.Helpers;
using AdamController.Model;
using AdamController.Properties;
using AdamController.ViewModels.HamburgerMenu;
using AdamController.Views.HamburgerPage;
using AdamController.WebApi.Client.v1;
using AdamController.WebApi.Client.v1.ResponseModel;
using MessageDialogManagerLib;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AdamController.ViewModels
{
    public class ScratchControlView : HamburgerMenuItemView
    {
        #region Action field 

        public static Action<string> SendSourceToScriptEditor { get; set; }
        public static Action<int> SetSelectedPageIndex { get; set; }
        public static Action<string> AppLogStatusBarAction { get; set; }
        public static Action<string> CompileLogStatusBarAction { get; set; }
        public static Action<bool> ProgressRingStartAction { get; set; }
        public static Action ReloadWebView { get; set; }

        #endregion

        private readonly IMessageDialogManager IDialogManager;
        private bool mIsWarningStackOwerflowAlreadyShow;

        public ScratchControlView(HamburgerMenuView hamburgerMenuView) : base(hamburgerMenuView)
        {
            IDialogManager = new MessageDialogManagerMahapps(Application.Current);

            InitAction();
            PythonExecuteEvent();

            ComunicateHelper.OnAdamTcpConnectedEvent += OnAdamTcpConnectedEvent;
            ComunicateHelper.OnAdamTcpDisconnectedEvent += OnAdamTcpDisconnectedEvent;

        }

        private void OnAdamTcpDisconnectedEvent()
        {
            UpdatePythonInfo();
        }

        private async void OnAdamTcpConnectedEvent()
        {
            var pythonVersionResult = await BaseApi.GetPythonVersion();
            var pythonBinPathResult = await BaseApi.GetPythonBinDir();
            var pythonWorkDirResult = await BaseApi.GetPythonWorkDir();

            string pythonVersion = pythonVersionResult?.StandardOutput?.Replace("\n", "");
            string pythonBinPath = pythonBinPathResult?.StandardOutput?.Replace("\n", "");
            string pythonWorkDir = pythonWorkDirResult?.StandardOutput?.Replace("\n", "");

            UpdatePythonInfo(pythonVersion, pythonBinPath, pythonWorkDir);
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

        #region Action initialize

        private void InitAction()
        {
            ScratchControl.NavigationComplete ??= new Action(() => NavigationComplete());

            ScratchControl.WebMessageReceived ??= new Action<WebMessageJsonReceived>((results) => WebMessageReceived(results));
        }

        #endregion

        #region Native python execute event

        private void PythonExecuteEvent()
        {
            PythonScriptExecuteHelper.OnExecuteStartEvent += (message) =>
            {
                if (MainWindowViewModel.GetSelectedPageIndex != 0)
                    return;

                mIsWarningStackOwerflowAlreadyShow = false;

                StartExecuteProgram();
                
                ResultTextEditor += message;
            };

            PythonScriptExecuteHelper.OnStandartOutputEvent += (message) => 
            {
                if (MainWindowViewModel.GetSelectedPageIndex != 0)
                    return;
                
                if(ResultTextEditorLength > 10000)
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
            };

            PythonScriptExecuteHelper.OnExecuteFinishEvent += (message) =>
            {
                if (MainWindowViewModel.GetSelectedPageIndex != 0)
                    return;

                FinishExecuteProgram();
                ResultTextEditor += message;
            };
        }

        #endregion

        #region Execute program event

        private async void StartExecuteProgram()
        {
            CompileLogStatusBarAction("Сеанс отладки запущен");
            IsEnabledShowOpenDialogButton = false;
            IsEnabledStopExecuteButton = true;
            ResultTextEditor = string.Empty;
            ProgressRingStartAction(true);
            
            if (!Properties.Settings.Default.ShadowWorkspaceInDebug) return;

            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(async () =>
            {
                await ScratchControl.ExecuteScript(Scripts.ShadowEnable);
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
                await ScratchControl.ExecuteScript(Scripts.ShadowDisable);
            }));

            CompileLogStatusBarAction(compileLogStatusBarAction);
            ProgressRingStartAction(false);
            IsEnabledShowOpenDialogButton = true;
            IsEnabledStopExecuteButton = false;
        }

        #endregion

        private void WebMessageReceived(WebMessageJsonReceived results)
        { 
            if (results.Action == "sendSourceCode")
            {
               SourceTextEditor = results.Data;
            }
        }

        #region IsEnabled buttons field

        private bool isEnabledStopExecuteButton = false;
        public bool IsEnabledStopExecuteButton
        {
            get => isEnabledStopExecuteButton;
            set
            {
                if (value == isEnabledStopExecuteButton) return;

                isEnabledStopExecuteButton = value;
                OnPropertyChanged(nameof(IsEnabledStopExecuteButton));
            }
        }

        private bool isEnabledShowOpenDialogButton = true;
        public bool IsEnabledShowOpenDialogButton
        {
            get => isEnabledShowOpenDialogButton;
            set
            {
                if (value == isEnabledShowOpenDialogButton) return;

                isEnabledShowOpenDialogButton = value;
                OnPropertyChanged(nameof(IsEnabledShowOpenDialogButton));
            }
        }

        #endregion

        #region Initialize Blockly

        /// <summary>
        /// Run js-script after web view navigation complete 
        /// </summary>
        private void NavigationComplete()
        {
            InitBlockly();
        }

        private async void InitBlockly()
        {
            BlocklyLanguage language = Settings.Default.BlocklyWorkspaceLanguage;

            try
            {
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(async () =>
                {
                    await LoadBlocklySrc();
                    await LoadBlocklyBlockLocalLangSrc(language);
                    _ = await ScratchControl.ExecuteScript(InitWorkspace());
                    _ = await ScratchControl.ExecuteScript(Scripts.ListenerCreatePythonCode);
                    _ = await ScratchControl.ExecuteScript(Scripts.ListenerSavedBlocks);

                    if (Settings.Default.BlocklyRestoreBlockOnLoad)
                    {
                        _ = await ScratchControl.ExecuteScript(Scripts.RestoreSavedBlocks);
                    }
                        
                    
                }));
                
                AppLogStatusBarAction("Загрузка скретч-редактора закончена");
            }
            catch
            {
                //the error occurs when switching to another tab before blockly is fully loaded
                AppLogStatusBarAction("Загрузка скретч-редактора внезапно прервана");
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

            _ = await ScratchControl.ExecuteScript(loadLocalSrc);

            //Thread.Sleep(1000);
            Thread.Sleep(500);
            _ = await ScratchControl.ExecuteScript(loadLocalAdamBlockSrc);

            //Thread.Sleep(1000);
            Thread.Sleep(500);
            _ = await ScratchControl.ExecuteScript(loadLocalAdamPythonGenSrc);
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
                if (value == resultTextEditor) return;

                resultTextEditor = value;
                ResultTextEditorLength = value.Length;
                
                OnPropertyChanged(nameof(ResultTextEditor));
            }
        }

        #endregion

        #region Result text editor length

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

                OnPropertyChanged(nameof(ResultTextEditorError));
            }
        }

        private string pythonVersion; 
        public string PythonVersion
        {
            get => pythonVersion;
            set
            {
                if (value == pythonVersion) return;

                pythonVersion = value;
                OnPropertyChanged(nameof(PythonVersion));
            }
        }

        private string pythonBinPath;
        public string PythonBinPath
        {
            get => pythonBinPath;
            set
            {
                if (value == pythonBinPath) return;

                pythonBinPath = value;
                OnPropertyChanged(nameof(PythonBinPath));
            }
        }

        private string pythonWorkDir;
        public string PythonWorkDir
        {
            get => pythonWorkDir;
            set
            {
                if (value == pythonWorkDir) return;

                pythonWorkDir = value;
                OnPropertyChanged(nameof(PythonWorkDir));
            }
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

        #endregion

        #region Command

        private RelayCommand reloadWebViewCommand;
        public RelayCommand ReloadWebViewCommand => reloadWebViewCommand ??= new RelayCommand(obj => 
        {
            SourceTextEditor = string.Empty;
            ReloadWebView();
        });

        private RelayCommand sendToExternalSourceEditor;
        public RelayCommand SendToExternalSourceEditor => sendToExternalSourceEditor ??= new RelayCommand(obj => 
        {
            SetSelectedPageIndex(1);
            SendSourceToScriptEditor(SourceTextEditor);

        }, canExecute => SourceTextEditor?.Length > 0);

        private RelayCommand showSaveFileDialogCommand;
        public RelayCommand ShowSaveFileDialogCommand => showSaveFileDialogCommand ??= new RelayCommand(async obj =>
        {
            string workspace = await ScratchControl.ExecuteScript("getSavedWorkspace()");
            string xmlWorkspace = JsonConvert.DeserializeObject<dynamic>(workspace);

            if (IDialogManager.ShowSaveFileDialog("Сохранить рабочую область", Properties.Settings.Default.SavedUserWorkspaceFolderPath, "workspace", ".xml", "XML documents (.xml)|*.xml"))
            {
                string path = IDialogManager.FilePathToSave;
                await FileHelper.WriteAsync(path, xmlWorkspace);

                AppLogStatusBarAction($"Файл {IDialogManager.FilePathToSave} сохранен");
            }
            else
            {
                AppLogStatusBarAction("Файл не сохранен");
            }
        });

        private RelayCommand showOpenFileDialogCommand;
        public RelayCommand ShowOpenFileDialogCommand => showOpenFileDialogCommand ??= new RelayCommand(async obj =>
            {
                if (IDialogManager.ShowFileBrowser("Выберите сохранененную рабочую область", Properties.Settings.Default.SavedUserWorkspaceFolderPath, "XML documents(.xml) | *.xml"))
                {
                    string path = IDialogManager.FilePath;
                    if (path == "") return;

                    string xml = await FileHelper.ReadTextAsStringAsync(path);
                    _ = await ExecuteScriptFunctionAsync("loadSavedWorkspace", new object[] { xml });

                    AppLogStatusBarAction($"Файл {path} загружен");
                }
                else
                {
                    AppLogStatusBarAction("Файл рабочей области не выбран");
                }
            });

        private RelayCommand runCode;
        public RelayCommand RunCode => runCode ??= new RelayCommand(async obj =>
        {
            ResultTextEditorError = string.Empty;
            ExtendedCommandExecuteResult executeResult = new();

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

        }, canExecute => !string.IsNullOrEmpty(SourceTextEditor) && ComunicateHelper.TcpClientIsConnected);

        private RelayCommand stopExecute;
        public RelayCommand StopExecute => stopExecute ??= new RelayCommand(async obj=>
        {
            if (ComunicateHelper.TcpClientIsConnected)
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

        private RelayCommand toZeroPositionCommand;
        public RelayCommand ToZeroPositionCommand => toZeroPositionCommand ??= new RelayCommand(async obj =>
        {
            try
            {
                await BaseApi.StopPythonExecute();
                //await BaseApi.MoveToZeroPosition();    
            }
            catch (Exception ex)
            {
                ResultTextEditorError = ex.Message.ToString();
            }

        }, canExecute => ComunicateHelper.TcpClientIsConnected);

        private RelayCommand cleanExecuteEditor;
        public RelayCommand CleanExecuteEditor => cleanExecuteEditor ??= new RelayCommand(async obj =>
        {
            await Task.Run(() => 
            {
                ResultTextEditorError = string.Empty;
                ResultTextEditor = string.Empty;
            });
        });

        #region ShowSaveFileDialogCommand

        private RelayCommand showSaveFileSourceTextDialogCommand;
        public RelayCommand ShowSaveFileSourceTextDialogCommand => showSaveFileSourceTextDialogCommand ??= new RelayCommand(async obj =>
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

        #region ExecuteScripts

        private static async Task<string> ExecuteScriptFunctionAsync(string functionName, params object[] parameters)
        {
            string script = Scripts.SerealizeObject(functionName, parameters);
            return await ScratchControl.ExecuteScript(script);
        }

        #endregion

    }
}

using System;
using System.Globalization;
using System.Reflection;
using System.IO;
using AdamController.Core.Properties;

namespace AdamController.Helpers
{
    public static class FolderHelper
    {
        static FolderHelper()
        {
            SaveFolderPathToSettings();
        }

        /// <summary>
        /// starts when the application is first launched
        /// </summary>
        private static void SaveFolderPathToSettings()
        {
            if (string.IsNullOrEmpty(Settings.Default.SavedUserWorkspaceFolderPath))
            {
                Settings.Default.SavedUserWorkspaceFolderPath = SavedWorkspaceDocumentsDir;
            }

            if (string.IsNullOrEmpty(Settings.Default.SavedUserToolboxFolderPath))
            {
                Settings.Default.SavedUserToolboxFolderPath = SavedToolboxDocumentsDir;
            }

            if (string.IsNullOrEmpty(Settings.Default.SavedUserCustomBlocksFolderPath))
            {
                Settings.Default.SavedUserCustomBlocksFolderPath = SavedUserCustomBlocksDocumentsDir;
            }

            if (string.IsNullOrEmpty(Settings.Default.SavedUserScriptsFolderPath))
            {
                Settings.Default.SavedUserScriptsFolderPath = SavedUserScriptsDocumentsDir;
            }

            if (string.IsNullOrEmpty(Settings.Default.SavedResultsNetworkTestsFolderPath))
            {
                Settings.Default.SavedResultsNetworkTestsFolderPath = SavedResultsNetworkTestsDir;
            }
        }

        private static string AssemblyTitle => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// Get a path to the directory where the user store his documents
        /// </summary>
        public static string MyDocumentsUserDir => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// Главная дирректория для хранения файлов пользователя
        /// Путь C:\Users\UserName\AdamStudio
        /// </summary>
        public static string SpecialProgramDocumentsDir => MyDocumentsUserDir + Path.DirectorySeparatorChar + "AdamStudio";

        /// <summary>
        /// Дирректория для сохранения рабочих областей
        /// Путь C:\Users\UserName\AdamStudio\SavedWorkspace
        /// </summary>
        public static string SavedWorkspaceDocumentsDir => SpecialProgramDocumentsDir + Path.DirectorySeparatorChar + "MyWorkspaces";

        /// <summary>
        /// Дирректория для сохранения пользовательских панелей
        /// Путь C:\Users\UserName\AdamStudio\MyToolbox
        /// </summary>
        public static string SavedToolboxDocumentsDir => SpecialProgramDocumentsDir + Path.DirectorySeparatorChar + "MyToolboxes";

        /// <summary>
        /// Дирректория для сохранения пользовательских блоков
        /// Путь C:\Users\UserName\AdamStudio\MyBlocks
        /// </summary>
        public static string SavedUserCustomBlocksDocumentsDir => SpecialProgramDocumentsDir + Path.DirectorySeparatorChar + "MyBlocks";

        /// <summary>
        /// Дирректория для сохранения пользовательских скриптов
        /// Путь C:\Users\UserName\AdamStudio\MyScripts
        /// </summary>
        public static string SavedUserScriptsDocumentsDir => SpecialProgramDocumentsDir + Path.DirectorySeparatorChar + "MyScripts";

        /// <summary>
        /// Дирректория для сохранения тестов сети
        /// Путь C:\Users\UserName\AdamStudio\MyScripts
        /// </summary>
        /// SavedResultsNetworkTestsFolderPath
        public static string SavedResultsNetworkTestsDir => SpecialProgramDocumentsDir + Path.DirectorySeparatorChar + "NetworkResultsTests";

        /// <summary>
        /// Get a path to the directory where the application
        /// can persist/load user data on session exit and re-start.
        /// </summary>
        public static string DirAppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + AssemblyTitle;

        /// <summary>
        /// Get path and file name to application specific session file
        /// </summary>
        public static string DirFileAppSessionData => Path.Combine(DirAppData, string.Format(CultureInfo.InvariantCulture, "{0}.App.session", AssemblyTitle));

        /// <summary>
        /// Return common application data folder
        /// </summary>
        public static string CommonDirAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Path.DirectorySeparatorChar + Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// Create app folder in application data dirrectory
        /// </summary>
        /// <returns></returns>
        public static bool CreateAppDataFolder()
        {
            try
            {
                if (!Directory.Exists(DirAppData))
                {
                    _ = Directory.CreateDirectory(DirAppData);
                }

                if (!Directory.Exists(SpecialProgramDocumentsDir))
                {
                    _ = Directory.CreateDirectory(SpecialProgramDocumentsDir);
                }

                if (!Directory.Exists(SavedWorkspaceDocumentsDir))
                {
                    _ = Directory.CreateDirectory(SavedWorkspaceDocumentsDir);
                }

                if (!Directory.Exists(SavedToolboxDocumentsDir))
                {
                    _ = Directory.CreateDirectory(SavedToolboxDocumentsDir);
                }
                if (!Directory.Exists(SavedUserCustomBlocksDocumentsDir))
                {
                    _ = Directory.CreateDirectory(SavedUserCustomBlocksDocumentsDir);
                }
                if (!Directory.Exists(SavedUserScriptsDocumentsDir))
                {
                    _ = Directory.CreateDirectory(SavedUserScriptsDocumentsDir);
                }
                if (!Directory.Exists(SavedResultsNetworkTestsDir))
                {
                    _ = Directory.CreateDirectory(SavedResultsNetworkTestsDir);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}



using System;

namespace AdamController.Services.Interfaces
{
    public interface IFolderManagmentService : IDisposable
    {
        public string MyDocumentsUserDir { get; }

        public string SpecialProgramDocumentsDir { get; }

        public  string SavedWorkspaceDocumentsDir { get; }

        public string SavedToolboxDocumentsDir { get; }

        public  string SavedUserCustomBlocksDocumentsDir { get; }

        public  string SavedUserScriptsDocumentsDir { get; }

        public string SavedResultsNetworkTestsDir { get; }

        public string DirAppData { get; }

        public string DirFileAppSessionData { get; }

        public string CommonDirAppData { get; }

        public bool CreateAppDataFolder();
    }
}

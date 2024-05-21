using AdamController.Services.Interfaces;
using System.IO;
using Prism.Services.Dialogs;
using Microsoft.Win32;
using AdamController.Services.SystemDialogServiceDependency;


namespace AdamController.Services
{
    public class SystemDialogService : ISystemDialogService
    {
        private readonly OpenFileDialog mOpenFileDialog = new();

        public OpenFileDialogResult ShowOpenFileDialog(IDialogParameters parameters)
        {
            
            mOpenFileDialog.Multiselect = false;
            mOpenFileDialog.AddToRecent = true;
            mOpenFileDialog.Filter = "SupportedFiles (*.xml,*.py)|*.xml;*.py|All files (*.*)|*.*";
            mOpenFileDialog.Title = parameters.GetValue<string>("Title");
            mOpenFileDialog.InitialDirectory = parameters.GetValue<string>("InitialDirectory");
            
            bool? isOpen = mOpenFileDialog.ShowDialog();
            OpenFileDialogResult result = new();
            
            if (isOpen == true)
            {
                string filePathName = mOpenFileDialog.FileName;
                OpenFileType fileType = DetermineOpenFileType(filePathName);

                result.IsOpenFileCanceled = false;
                result.OpenFilePath = filePathName;
                result.OpenFileType = fileType;
                result.IsSupportedFileTypeOpened = fileType != OpenFileType.Undefined;
            }

            return result;
        }

        public static OpenFileType DetermineOpenFileType(string fileName)
        {
            string openExt = Path.GetExtension(fileName);

            OpenFileType result = openExt switch
            {
                ".xml" => OpenFileType.Workspace,
                ".py" => OpenFileType.Script,
                _ => OpenFileType.Undefined,
            };

            return result;
        }

        public void Dispose()
        {
            //mOpenFileDialog.Dispose();
        }
    }
}

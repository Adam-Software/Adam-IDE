namespace AdamController.Services.SystemDialogServiceDependency
{
    public class OpenFileDialogResult
    {
        public bool IsOpenFileCanceled = true;

        public bool IsSupportedFileTypeOpened = false;
        public OpenFileType OpenFileType { get; set; } = OpenFileType.Undefined;
        public string OpenFilePath { get; set; } = string.Empty;
    }
}

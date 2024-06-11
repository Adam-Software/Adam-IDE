namespace AdamStudio.Services.SystemDialogServiceDependency
{
    public class OpenFileDialogResult
    {
        public bool IsOpenFileCanceled = true;

        public bool IsSupportedFileTypeOpened = false;
        public SupportFileType OpenFileType { get; set; } = SupportFileType.Undefined;
        public string OpenFilePath { get; set; } = string.Empty;
    }
}

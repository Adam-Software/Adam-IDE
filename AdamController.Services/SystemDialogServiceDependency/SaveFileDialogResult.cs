namespace AdamController.Services.SystemDialogServiceDependency
{
    public class SaveFileDialogResult
    {
        public bool IsSaveFileCanceled = true;        
        public string SavedFilePath { get; set; } = string.Empty;
    }
}

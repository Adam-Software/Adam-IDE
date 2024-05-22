namespace AdamController.Services.SystemDialogServiceDependency
{
    public class OpenFolderDialogResult
    {
        public bool IsSelectFolderCanceled = true;
        public string SelectedFolderPath { get; set; } = string.Empty;
    }
}

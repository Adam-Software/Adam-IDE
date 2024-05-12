using AdamController.Services.Interfaces;
using MessageDialogManagerLib;
using System.Windows;

namespace AdamController.Services
{
    public class DialogManager : MessageDialogManagerMahapps, IDialogManagerService
    {
        public DialogManager(Application app) : base(app) {}

        public void Dispose(){}
    }
}

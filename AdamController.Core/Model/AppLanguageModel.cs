using Prism.Mvvm;

namespace AdamController.Core.Model
{
    public class AppLanguageModel : BindableBase
    {
        private string appLanguage;
        public string AppLanguage
        {
            get => appLanguage;
            set => SetProperty(ref appLanguage, value);
        }

        private string languageName;
        public string LanguageName
        {
            get => languageName;
            set => SetProperty(ref languageName, value);
        }
    }
}

using Prism.Mvvm;

namespace AdamController.Core.Model
{
    public class AppLanguageModel : BindableBase
    {
        private string appLanguage;
        public string AppLanguage
        {
            get => appLanguage;
            set
            {
                if (value == appLanguage)
                {
                    return;
                }

                appLanguage = value;

                SetProperty(ref appLanguage, value);
            }
        }

        private string languageName;
        public string LanguageName
        {
            get => languageName;
            set
            {
                if (value == languageName)
                {
                    return;
                }

                languageName = value;

                SetProperty (ref languageName, value);  
            }
        }
    }
}

using AdamController.Model.Common;

namespace AdamController.Model
{
    public class AppLanguageModel : BaseModel
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
                OnPropertyChanged(nameof(AppLanguage));
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
                OnPropertyChanged(nameof(LanguageName));
            }
        }
    }
}

using AdamBlocklyLibrary.Enum;
using Prism.Mvvm;

namespace AdamController.Core.Model
{
    public class BlocklyLanguageModel : BindableBase
    {
        private BlocklyLanguage blocklyLanguage;
        public BlocklyLanguage BlocklyLanguage 
        {
            get => blocklyLanguage;
            set => SetProperty(ref blocklyLanguage, value);
        }

        private string languageName;
        public string LanguageName 
        {
            get => languageName;
            set => SetProperty(ref languageName, value);
        }
    }
}

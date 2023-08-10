using AdamBlocklyLibrary.Enum;
using AdamController.Model.Common;

namespace AdamController.Model
{
    public class BlocklyLanguageModel : BaseModel
    {
        private BlocklyLanguage blocklyLanguage;
        public BlocklyLanguage BlocklyLanguage 
        {
            get { return blocklyLanguage; }
            set
            {
                if (value == blocklyLanguage)
                {
                    return;
                }

                blocklyLanguage = value;
                OnPropertyChanged(nameof(BlocklyLanguage));
            }
        }

        private string languageName;
        public string LanguageName 
        { 
            get { return languageName; }
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

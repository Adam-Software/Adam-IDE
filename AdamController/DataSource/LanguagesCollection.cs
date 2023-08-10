using AdamBlocklyLibrary.Enum;
using AdamController.Model;
using System.Collections.ObjectModel;

namespace AdamController.DataSource
{
    public class LanguagesCollection
    {
        public static ObservableCollection<AppLanguageModel> AppLanguageCollection { get; private set; } = new ObservableCollection<AppLanguageModel>
        {
            new AppLanguageModel { AppLanguage = "ru", LanguageName = "Русский" }
        };

        public static ObservableCollection<BlocklyLanguageModel> BlocklyLanguageCollection { get; private set; } = new ObservableCollection<BlocklyLanguageModel>
        {
            new BlocklyLanguageModel { BlocklyLanguage = BlocklyLanguage.ru, LanguageName = "Русский" },
            new BlocklyLanguageModel { BlocklyLanguage =  BlocklyLanguage.zh, LanguageName = "Китайский" },
            new BlocklyLanguageModel { BlocklyLanguage =  BlocklyLanguage.en, LanguageName = "Английский" }
        };
    }
}

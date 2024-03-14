using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.SpecificToolbox;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.ToolboxSets
{
    public class DefaultSimpleCategoryToolbox
    {
        public Toolbox.Toolbox Toolbox => InitToolbox();

        public ToolboxParam LogicCategoryParam { get; set; }
        public ToolboxParam ColourCategoryParam { get; set; }
        public ToolboxParam ListsCategoryParam { get; set; }
        public ToolboxParam LoopsCategoryParam { get; set; }
        public ToolboxParam MathCategoryParam { get; set; }
        public ToolboxParam ProcedureCategoryParam { get; set; }
        public ToolboxParam TextCategoryParam { get; set; }
        public ToolboxParam ThreadsCategoryParam { get; set; }
        public ToolboxParam SystemsCategoryParam { get; set; }
        public ToolboxParam VariableDynamicCategoryParam { get; set; }
        public ToolboxParam VariableCategoryParam { get; set; }
        public ToolboxParam DateTimeCategoryParam { get; set; }
        public ToolboxParam AdamCommonCategoryParam { get; set; }
        public ToolboxParam AdamThreeCategoryParam { get; set; }
        public ToolboxParam AdamTwoCategoryParam { get; set; }

        public DefaultSimpleCategoryToolbox(BlocklyLanguage language)
        {
            Properties.Resources.Culture = new System.Globalization.CultureInfo(language.ToString());
        }

        private Toolbox.Toolbox InitToolbox()
        {
            CategoryToolbox categoryLogic = new LogicToolbox(LogicCategoryParam.Hidden, LogicCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryColour = new ColourToolbox(ColourCategoryParam.Hidden, ColourCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryLists = new ListsToolbox(ListsCategoryParam.Hidden, ListsCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryLoops = new LoopsToolbox(LoopsCategoryParam.Hidden, LoopsCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryMath = new MathToolbox(MathCategoryParam.Hidden, MathCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryProcedure = new ProcedureToolbox(ProcedureCategoryParam.Hidden, ProcedureCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryText = new TextToolbox(TextCategoryParam.Hidden, TextCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryThreads = new ThreadToolbox(ThreadsCategoryParam.Hidden, ThreadsCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryVariablesDynamic = new VariablesDynamicToolbox(VariableDynamicCategoryParam.Hidden, VariableDynamicCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categoryVariables = new VariablesToolbox(VariableCategoryParam.Hidden, VariableCategoryParam.AlternateName).Toolbox;
            CategoryToolbox categorySystems = new SystemsToolbox(SystemsCategoryParam.Hidden, SystemsCategoryParam.AlternateName).Toolbox;

            //category for adam blocks
            ToolboxCategoryParam[] @params = new[]
            {
                new ToolboxCategoryParam { Hidden = AdamCommonCategoryParam.Hidden, Name = AdamCommonCategoryParam.AlternateName },
                new ToolboxCategoryParam { Hidden = AdamTwoCategoryParam.Hidden, Name = AdamTwoCategoryParam.AlternateName },
                new ToolboxCategoryParam { Hidden = AdamThreeCategoryParam.Hidden, Name = AdamThreeCategoryParam.AlternateName}
            };

            CategoryToolbox categoryAdamCommon = new AdamCommonToolbox(@params).Toolbox;
            
            IList<CategoryToolbox> categories = new List<CategoryToolbox>
            {
                categoryLogic,
                categoryColour,
                categoryLists,
                categoryLoops,
                categoryMath,
                categoryProcedure,
                categoryText,
                categoryThreads,
                categoryVariablesDynamic,
                categoryVariables,
                categorySystems,

                categoryAdamCommon
            };

            Toolbox.Toolbox toolbox  = new() { CategoryToolboxContents = categories };

            return toolbox;
        }
    }

    
}

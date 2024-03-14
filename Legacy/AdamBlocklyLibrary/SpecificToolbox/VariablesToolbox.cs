using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class VariablesToolbox : BaseSpecificToolbox
    {
        public VariablesToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.variable_category,

                SimpleToolboxContents = new List<SimpleToolbox>
                {
                    new SimpleToolbox()
                    {
                        Type = BlockType.variables_get
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.variable_with_continuation
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.variables_set
                    }
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.VariablesToolboxName;
        }
    }
}

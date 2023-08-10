using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class LogicToolbox : BaseSpecificToolbox
    {
        public LogicToolbox(bool hidden, string name) : base(hidden, name) {}

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.logic_category,

                SimpleToolboxContents = new List<SimpleToolbox>
                {
                    new SimpleToolbox()
                    {
                        Type = BlockType.logic_boolean
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.controls_if
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.controls_ifelse
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.logic_compare
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.logic_negate
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.logic_null
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.logic_operation
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.logic_ternary
                    }
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.LogicToolboxName;
        }
    }
}

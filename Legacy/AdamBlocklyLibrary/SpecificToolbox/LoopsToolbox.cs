using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class LoopsToolbox : BaseSpecificToolbox
    {
        public LoopsToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.loop_category,

                SimpleToolboxContents = new List<SimpleToolbox>
                {
                    new SimpleToolbox()
                    {
                        Type = BlockType.controls_flow_statements
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.controls_for
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.controls_forEach
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.controls_repeat
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.controls_repeat_ext
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.controls_whileUntil
                    }
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.LoopsToolboxName;
        }
    }
}

using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class ColourToolbox : BaseSpecificToolbox
    {
        public ColourToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new CategoryToolbox
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.colour_category,

                SimpleToolboxContents = new List<SimpleToolbox>
                {
                    new SimpleToolbox()
                    {
                        Type = BlockType.colour_blend
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.colour_picker
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.colour_random
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.colour_rgb
                    },
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.ColourToolboxName;
        }
    }
}

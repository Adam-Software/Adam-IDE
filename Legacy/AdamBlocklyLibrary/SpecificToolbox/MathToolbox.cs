using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class MathToolbox : BaseSpecificToolbox
    {
        public MathToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.math_category,

                SimpleToolboxContents = new List<SimpleToolbox>
                {
                    new SimpleToolbox()
                    {
                        Type = BlockType.math_arithmetic
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_atan2
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_change
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_constant
                    },

                        new SimpleToolbox()
                    {
                        Type = BlockType.math_constrain
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_modulo
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_number
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_number_property
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_on_list
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_random_float
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_random_int
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_round
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_single
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.math_trig
                    }
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.MathToolboxName;
        }
    }
}

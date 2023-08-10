using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Toolbox;
using AdamBlocklyLibrary.Enum;
using System.Collections.Generic;
using AdamBlocklyLibrary.Struct;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class TextToolbox : BaseSpecificToolbox
    {
        public TextToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.text_category,

                SimpleToolboxContents = new List<SimpleToolbox>
                {
                    new SimpleToolbox()
                    {
                        Type = BlockType.text_print
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.common_comment
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text
                    },

                     new SimpleToolbox()
                    {
                        Type = BlockType.text_with_continuation
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_multiline
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_join
                    },

                    /*new SimpleToolbox()
                    {
                        Type = BlockType.text_create_join_container
                    },*/

                    /*new SimpleToolbox()
                    {
                        Type = BlockType.text_create_join_item
                    },*/

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_append
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_length
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_isEmpty
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_indexOf
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_charAt
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_changeCase
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_trim
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_prompt_ext
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_prompt
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_count
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.text_replace
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_reverse
                    }
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.TextToolboxName;
        }
    }
}

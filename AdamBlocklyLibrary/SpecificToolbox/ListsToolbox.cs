using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class ListsToolbox : BaseSpecificToolbox
    {
        public ListsToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.list_category,

                SimpleToolboxContents = new List<SimpleToolbox>
                {
                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_create_empty
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_create_with
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_create_with_container
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_create_with_item
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_getIndex
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_getSublist
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_indexOf
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_isEmpty
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_length
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_repeat
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_reverse
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_setIndex
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_sort
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.lists_split
                    }
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.ListsToolboxName;
        }
    }
}

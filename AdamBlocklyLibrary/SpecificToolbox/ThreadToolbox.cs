
using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class ThreadToolbox : BaseSpecificToolbox
    {
        public ThreadToolbox(bool hidden, string name) : base(hidden, name) { }

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
                         Type = BlockType.time_sleep,
                     }
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.ThreadToolboxName;
        }
    }
}

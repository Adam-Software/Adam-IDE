using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class ProcedureToolbox : BaseSpecificToolbox
    {

        public ProcedureToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.procedure_category,

                SimpleToolboxContents = new List<SimpleToolbox>
                {
                    new SimpleToolbox()
                    {
                        Type = BlockType.procedures_callnoreturn
                    },

                    new SimpleToolbox()
                    {
                        Type = BlockType.procedures_callreturn
                    }
                }
            };

            return toolbox;
        }
    
        public override string ResourcesToolboxName()
        {
            return Resources.ProcedureToolboxName;
        }
    }
}

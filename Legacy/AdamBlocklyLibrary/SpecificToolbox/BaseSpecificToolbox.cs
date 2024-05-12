using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public abstract class  BaseSpecificToolbox
    {
        protected BaseSpecificToolbox()
        {
            ToolboxCategoryParam[] param = new[]
{
                new ToolboxCategoryParam
                {
                    Hidden = false, Name = ResourcesToolboxName()
                }
            };

            Toolbox = GetCategoryToolbox(param);
        }

        protected BaseSpecificToolbox(bool hidden)
        {
            ToolboxCategoryParam[] param = new[]
{
                new ToolboxCategoryParam
                {
                    Hidden = hidden, Name = ResourcesToolboxName()
                }
            };

            Toolbox = GetCategoryToolbox(param);
        }

        protected BaseSpecificToolbox(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = ResourcesToolboxName();
            }

            ToolboxCategoryParam[] param = new[]
{
                new ToolboxCategoryParam
                {
                    Hidden = false, Name = name
                }
            };

            Toolbox = GetCategoryToolbox(param);
        }

        protected BaseSpecificToolbox(bool hidden, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = ResourcesToolboxName();
            }

            ToolboxCategoryParam[] param = new[]
            { 
                new ToolboxCategoryParam 
                { 
                    Hidden = hidden, Name = name
                } 
            };

            Toolbox = GetCategoryToolbox(param);
        }

        protected BaseSpecificToolbox(ToolboxCategoryParam[] @params)
        {
            if (string.IsNullOrEmpty(@params[0].Name))
            {
                @params[0].Name = ResourcesToolboxName();
            }

            Toolbox = GetCategoryToolbox(@params);
        }

        public CategoryToolbox Toolbox { get; protected set; }
 
        public abstract CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params);

        public abstract string ResourcesToolboxName();
    }
}

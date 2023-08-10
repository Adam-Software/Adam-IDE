using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    internal class SystemsToolbox : BaseSpecificToolbox
    {
        public SystemsToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                CategoryStyle = CategoryStyle.loop_category,

                CategoryToolboxContents = new List<CategoryToolbox>
                {
                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Управление процессам",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.systems_os_system
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.systems_subprocess_call
                            }
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Утилиты и приложения",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.common_app_echo
                            },


                            new SimpleToolbox()
                            {
                                Type = BlockType.common_app_rhvoice
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_app_ls_dir
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_i2cdetect
                            },
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Терминал",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_pipeline
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_literal
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_left_curly_brace
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_right_curly_brace
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_redirect_input
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_redirect_output
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_whitespace
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_single_quotation
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_double_quotation
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_system_one_single_quotation
                            },
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Ресурсы и примеры",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            /*new SimpleToolbox()
                            {
                                Type = BlockType.common_system_example_flack_app
                            },*/

                             new SimpleToolbox()
                            {
                                Type = BlockType.common_system_resources_music
                            },
                        }
                    },
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.SystemsToolboxName;
        }
    }
}

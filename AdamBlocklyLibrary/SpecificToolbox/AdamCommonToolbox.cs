using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class AdamCommonToolbox : BaseSpecificToolbox
    {
        public AdamCommonToolbox(bool hidden, string name) : base(hidden, name) { }

        public AdamCommonToolbox(ToolboxCategoryParam[] @params) : base(@params) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {

            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,

                Expanded = true,
                CategoryStyle = CategoryStyle.list_category,

                CategoryToolboxContents = new List<CategoryToolbox>
                {
                    new AdamTwoToolbox(@params[1].Hidden, @params[1].Name).Toolbox,
                    new AdamThreeToolbox(@params[2].Hidden, @params[2].Name).Toolbox,

                    # region eyes

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Глаза",
                        CategoryStyle = CategoryStyle.list_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.common_eye_color
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_reg_constant
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_eye_diode_number
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_i2c_device_address
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_import_smbus
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_eye_pack
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_eye_pack_simple
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_write_i2c_block_data,
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_simple_write_i2c_block_data,
                            }

                        },
                    },

                    #endregion

                    #region music editor

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Редактор музыки",
                        CategoryStyle = CategoryStyle.math_category,

                        CategoryToolboxContents =  new List<CategoryToolbox>
                        {
                            #region mixer

                             new CategoryToolbox
                             {
                                Hidden = false,
                                Name = "Микшер",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_mixer_init
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_mixer_load
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_mixer_play
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_mixer_get_bussy
                                    },
                                },
                             },
                                
                            
                            #endregion

                            #region chord

                            new CategoryToolbox
                            {
                                Hidden = false,
                                Name = "Аккорды",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_major_chord
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_minor_chord
                                    },

                                      new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_minor_major_seventh_chords
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_minor_minor_seventh_chords
                                    },
                                    
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_major_chord_extended
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_minor_chord_extended
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_minor_major_seventh_chords_extended
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_minor_minor_seventh_chords_extended
                                    },

                                }
                            },

                            #endregion

                            #region note

                            new CategoryToolbox
                            {
                                Hidden = false,
                                Name = "Ноты",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_note
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_note_extended
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_classic_note
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_classic_note_extended
                                    },
                                }
                            },

                            #endregion

                            #region functions

                            new CategoryToolbox
                            {
                                Hidden = false,
                                Name = "Функции",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_function_create_chord
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_function_create_instrument
                                    },
                                    
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_function_play
                                    },
                                },
                            },

                            #endregion

                            #region specsymbols

                            new CategoryToolbox
                            {
                                Hidden = false,
                                Name = "Спецсимволы",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_spes_symbols_with_numeric
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_spes_symbols
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_music_fraction
                                    },
                                },
                            }

                            #endregion
                        }
                    },

                    #endregion

                    #region voice

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Управление голосом",
                        CategoryStyle = CategoryStyle.text_category,

                        CategoryToolboxContents =  new List<CategoryToolbox>
                        {
                             #region native func

                             new CategoryToolbox
                             {
                                Hidden = false,
                                Name = "Нативные функции",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_say_native
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_say_native_with_voice_param
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_say_voices_list
                                    },
                                },
                             },
                                
                             #endregion

                             #region library func

                             new CategoryToolbox
                             {
                                Hidden = false,
                                Name = "Библиотечные функции",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_say_sh
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_say_sh_with_voice_param
                                    }
                                },
                             },
                                
                             #endregion

                             #region file extension

                             new CategoryToolbox
                             {
                                Hidden = false,
                                Name = "Расширения файлов",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_say_save_file_dp
                                    },
                                },
                             },
                                
                            #endregion
                        }
                    }

                    #endregion
                },
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.AdamCommonToolboxName;
        }
    }
}

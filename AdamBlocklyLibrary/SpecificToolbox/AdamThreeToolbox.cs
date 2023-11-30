using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class AdamThreeToolbox : BaseSpecificToolbox
    {
        public AdamThreeToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                Expanded = false,
                CategoryStyle = CategoryStyle.list_category,



                CategoryToolboxContents = new List<CategoryToolbox>
                {
                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Общие блоки",
                        CategoryStyle = CategoryStyle.math_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.controller_new_instance_class
                            }
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Управление моторами",
                        CategoryStyle = CategoryStyle.math_category,

                        CategoryToolboxContents= new List<CategoryToolbox>
                        {
                            new CategoryToolbox
                            {

                                Hidden = false,
                                Name = "Управление по векторам",
                                CategoryStyle = CategoryStyle.math_category,

                                SimpleToolboxContents= new List<SimpleToolbox>
                                {
                                        new SimpleToolbox()
                                        {
                                            Type = BlockType.move_free_vector_variable
                                        },

                                        new SimpleToolbox()
                                        {
                                            Type = BlockType.move_free_vector
                                        }
                                }
                            },

                            new CategoryToolbox
                            {

                                Hidden = false,
                                Name = "Управление по направлениям",
                                CategoryStyle = CategoryStyle.math_category,

                                SimpleToolboxContents= new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_controller_vector
                                    },

                                    new SimpleToolbox()
                                    {
                                         Type = BlockType.move_vector_forward
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.speed_vector_value
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_right
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_back
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_left
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_right_and_forward
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_left_and_forward
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_left_and_back
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_right_and_back
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_turn_right
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_turn_left
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_vector_stop
                                    }
                                }
                            },

                            new CategoryToolbox
                            {

                                Hidden = false,
                                Name = "Адресное управление",
                                CategoryStyle = CategoryStyle.math_category,

                                SimpleToolboxContents= new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_controller
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_command_set_speed
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.speed_value
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.speed_stop
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.move_get_register
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.speed_clear_register
                                    },
                                }
                            },
                        },
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Управление сервоприводами",
                        CategoryStyle = CategoryStyle.logic_category,

                        CategoryToolboxContents = new List<CategoryToolbox>
                        {
                             new CategoryToolbox
                             {
                                 Hidden = false,
                                 Name = "Команды управления",
                                 CategoryStyle = CategoryStyle.logic_category,

                                 SimpleToolboxContents = new List<SimpleToolbox>
                                 {
                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_motor_command
                                     },

                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_angle_variable
                                     },

                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_handle_command
                                     },
                                     
                                     new SimpleToolbox()
                                     {
                                        Type = BlockType.controller_return_to_start_position_command
                                     }
                                }
                             },

                             new CategoryToolbox
                             {
                                 Hidden = false,
                                 Name = "Константы сервоприводов",
                                 CategoryStyle = CategoryStyle.logic_category,

                                 SimpleToolboxContents = new List<SimpleToolbox>
                                 {
                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_const_head
                                     },

                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_const_neck
                                     },

                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_const_right_hand
                                     },

                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_const_left_hand
                                     },

                                     new SimpleToolbox()
                                     {
                                        Type = BlockType.controller_const_right_lower_arm_up
                                     },

                                     new SimpleToolbox()
                                     {
                                        Type = BlockType.controller_const_left_lower_arm_up
                                     },

                                     new SimpleToolbox()
                                     {
                                        Type = BlockType.controller_const_right_upper_arm
                                     },

                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_const_left_upper_arm
                                     },

                                     new SimpleToolbox()
                                     {
                                          Type = BlockType.controller_const_right_shoulder
                                     },

                                     new SimpleToolbox()
                                     {
                                          Type = BlockType.controller_const_left_shoulder
                                     },

                                     new SimpleToolbox()
                                     {
                                          Type = BlockType.controller_const_chest
                                     },

                                     new SimpleToolbox()
                                     {
                                          Type = BlockType.controller_const_press
                                     },

                                     new SimpleToolbox()
                                     {
                                          Type = BlockType.controller_const_left_upper_leg
                                     },

                                     new SimpleToolbox()
                                     {
                                          Type = BlockType.controller_const_right_upper_leg
                                     },

                                     new SimpleToolbox()
                                     {
                                          Type = BlockType.controller_const_left_lower_leg
                                     },

                                     new SimpleToolbox()
                                     {
                                          Type = BlockType.controller_const_right_lower_leg
                                     },

                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_const_left_foot
                                     },

                                     new SimpleToolbox()
                                     {
                                         Type = BlockType.controller_const_right_foot
                                     },
                                 }
                             },

                        }
                    },
                    
                    new CategoryToolbox()
                    {
                        Hidden = false,
                        Name = "LCD глаза",
                        CategoryStyle = CategoryStyle.list_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.common_eye_new_eye
                            },
                            
                            new SimpleToolbox()
                            {
                                Type = BlockType.common_eye_run_anim
                            }
                        },
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Дальномеры",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.rangefinder_left
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.rangefinder_right
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.common_import_rangefinders_i2c
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.rangefinder_get_distance_i2c
                            },
                            new SimpleToolbox()
                            {
                                Type = BlockType.rangefinder_i2c_address
                            }
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Датчики",
                        CategoryStyle = CategoryStyle.text_category,

                        CategoryToolboxContents= new List<CategoryToolbox>
                        {
                            new CategoryToolbox
                            {

                                Hidden = false,
                                Name = "Датчики движения",
                                CategoryStyle = CategoryStyle.text_category,

                                SimpleToolboxContents= new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_mpu_9250_declaration
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_acceleration
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_gyro
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_magnetometer
                                    }
                                }
                            },

                            new CategoryToolbox
                            {

                                Hidden = false,
                                Name = "Датчики внешней среды",
                                CategoryStyle = CategoryStyle.text_category,

                                SimpleToolboxContents= new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_i2c_sensor_device
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_bmp280_addr_const,
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_declaration_extended,
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_set_pressure
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_temperature
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_pressure
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.adam_two_seven_sensor_altitude
                                    }
                                }
                            },
                        }
                    },
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.AdamThreeToolboxName; 
        }
    }
}

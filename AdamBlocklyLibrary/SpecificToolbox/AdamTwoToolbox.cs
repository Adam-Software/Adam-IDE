using AdamBlocklyLibrary.Enum;
using AdamBlocklyLibrary.Properties;
using AdamBlocklyLibrary.Struct;
using AdamBlocklyLibrary.Toolbox;
using System.Collections.Generic;

namespace AdamBlocklyLibrary.SpecificToolbox
{
    public class AdamTwoToolbox : BaseSpecificToolbox
    {
        public AdamTwoToolbox(bool hidden, string name) : base(hidden, name) { }

        public override CategoryToolbox GetCategoryToolbox(ToolboxCategoryParam[] @params)
        {
            CategoryToolbox toolbox = new()
            {
                Hidden = @params[0].Hidden,
                Name = @params[0].Name,
                Expanded = false,
                CategoryStyle = CategoryStyle.math_category,

                CategoryToolboxContents = new List<CategoryToolbox>
                {
                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Общие блоки",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.import_adam_servo_api
                            },
                            
                            new SimpleToolbox()
                            {
                                Type = BlockType.import_adam_rangefinders
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.import_adam_servo_api_with_param
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_speed_variable
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_angle_variable
                            }
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Сервоприводы",
                        CategoryStyle = CategoryStyle.math_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.adam_import_servo_lib
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.adam_import_ping_variable
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.baudrate_variable
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.select_baudrate
                            }
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Голова",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_head
                            },
                        },
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Руки",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_right_hand
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_right_hand_short
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_left_hand
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_left_hand_short
                            },
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Туловище",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_torso
                            },
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Пресс",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_press
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_press_short
                            }
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Ноги",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_legs
                            },
 
                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Пальцы",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_fingers
                            },
                            
                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_squeeze_fingers
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_left_squeeze_fingers
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_right_squeeze_fingers
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_unclench_fingers
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_left_unclench_fingers
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.servo_right_unclench_fingers
                            }

                        }
                    },

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Колеса",
                        CategoryStyle = CategoryStyle.loop_category,

                        SimpleToolboxContents = new List<SimpleToolbox>
                        {
                            new SimpleToolbox()
                            {
                                Type = BlockType.wheels_left_leg
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.wheels_right_leg
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.wheels_left_leg_extended
                            },

                            new SimpleToolbox()
                            {
                                Type = BlockType.wheels_right_leg_extended
                            }
                        }
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

                    #region BNO055 sensors

                    new CategoryToolbox
                    {
                        Hidden = false,
                        Name = "Датчики",
                        CategoryStyle = CategoryStyle.math_category,

                        CategoryToolboxContents =  new List<CategoryToolbox>
                        {
                            # region common

                            new CategoryToolbox
                            {
                                Hidden = false,
                                Name = "Шина и устройства I2C",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_i2c_sensor_device
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_i2c_sensor_device_extended
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_declaration
                                    },

                                      new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_declaration_extended
                                    },
                                }
                            },

                            #endregion

                            #region sensors

                            new CategoryToolbox
                            {
                                Hidden = false,
                                Name = "Датчики",
                                CategoryStyle = CategoryStyle.list_category,

                                SimpleToolboxContents = new List<SimpleToolbox>
                                {
                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_temperature
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_acceleration
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_magnetometer
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_gyro
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_euler
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_quaternion
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_linear_acceleration
                                    },

                                    new SimpleToolbox()
                                    {
                                        Type = BlockType.common_sensor_gravity
                                    },
                                }
                            }

                            #endregion
                        }
                    },

                    #endregion
                }
            };

            return toolbox;
        }

        public override string ResourcesToolboxName()
        {
            return Resources.AdamTwoToolboxName;
        }
    }
}

namespace AdamBlocklyLibrary.Enum
{
    public enum BlockType 
    {
        #region Colour

        /// <summary>
        /// Block for colour picker
        /// </summary>
        colour_picker,

        /// <summary>
        /// Block for random colour
        /// </summary>
        colour_random,

        /// <summary>
        /// Block for composing a colour from RGB components
        /// </summary>
        colour_rgb,

        /// <summary>
        /// Block for blending two colours together
        /// </summary>
        colour_blend,

        #endregion

        #region Lists

        /// <summary>
        /// Block for creating an empty list
        /// The 'list_create_with' block is preferred as it is more flexible.
        /// <block type="lists_create_with"><mutation items="0"></mutation></block>
        /// </summary>
        lists_create_empty,

        /// <summary>
        /// Block for creating a list with one element repeated.
        /// </summary>
        lists_repeat,

        /// <summary>
        /// Block for reversing a list
        /// </summary>
        lists_reverse,

        /// <summary>
        /// Block for checking if a list is empty
        /// </summary>
        lists_isEmpty,

        /// <summary>
        /// Block for getting the list length
        /// </summary>
        lists_length,

        /// <summary>
        /// Block for creating a list with any number of elements of any type
        /// </summary>
        lists_create_with,

        /// <summary>
        /// Mutator block for list container
        /// </summary>
        lists_create_with_container,

        /// <summary>
        /// Mutator block for adding items
        /// </summary>
        lists_create_with_item,

        /// <summary>
        /// Block for finding an item in the list
        /// </summary>
        lists_indexOf,

        /// <summary>
        /// Block for getting element at index
        /// </summary>
        lists_getIndex,

        /// <summary>
        /// Block for setting the element at index
        /// </summary>
        lists_setIndex,

        /// <summary>
        /// Block for getting sublist
        /// </summary>
        lists_getSublist,

        /// <summary>
        /// Block for sorting a list
        /// </summary>
        lists_sort,

        /// <summary>
        /// Block for splitting text into a list, or joining a list into text
        /// </summary>
        lists_split,

        #endregion

        #region Logic

        /// <summary>
        /// Block for boolean data type: true and false.
        /// </summary>
        logic_boolean,

        /// <summary>
        /// Block for if/elseif/else condition.
        /// </summary>
        controls_if,

        /// <summary>
        /// If/else block that does not use a mutator.
        /// </summary>
        controls_ifelse,

        /// <summary>
        /// Block for comparison operator.
        /// </summary>
        logic_compare,

        /// <summary>
        /// Block for logical operations: 'and', 'or'.
        /// </summary>
        logic_operation,

        /// <summary>
        /// Block for negation.
        /// </summary>
        logic_negate,

        /// <summary>
        /// Block for null data type.
        /// </summary>
        logic_null,

        /// <summary>
        /// Block for ternary operator.
        /// </summary>
        logic_ternary,

        #endregion

        #region Loops

        /// <summary>
        /// Block for repeat n times (external number)
        /// </summary>
        controls_repeat_ext,

        /// <summary>
        /// Block for repeat n times (internal number).
        /// The 'controls_repeat_ext' block is preferred as it is more flexible.
        /// </summary>
        controls_repeat,

        /// <summary>
        /// Block for 'do while/until' loop
        /// </summary>
        controls_whileUntil,

        /// <summary>
        /// Block for 'for' loop.
        /// </summary>
        controls_for,

        /// <summary>
        /// Block for 'for each' loop.
        /// </summary>
        controls_forEach,

        /// <summary>
        /// Block for flow statements: continue, break.
        /// </summary>
        controls_flow_statements,

        #endregion

        #region Math

        /// <summary>
        /// Block for numeric value.
        /// </summary>
        math_number,

        /// <summary>
        /// Block for basic arithmetic operator.
        /// </summary>
        math_arithmetic,

        /// <summary>
        /// Block for advanced math operators with single operand.
        /// </summary>
        math_single,

        /// <summary>
        /// Block for trigonometry operators.
        /// </summary>
        math_trig,

        /// <summary>
        /// Block for constants: PI, E, the Golden Ratio, sqrt(2), 1/sqrt(2), INFINITY.
        /// </summary>
        math_constant,

        /// <summary>
        /// Block for checking if a number is even, odd, prime, whole, positive,
        /// negative or if it is divisible by certain number.
        /// </summary>
        math_number_property,

        /// <summary>
        /// Block for adding to a variable in place.
        /// </summary>
        math_change,

        /// <summary>
        /// Block for rounding functions.
        /// </summary>
        math_round,

        /// <summary>
        /// Block for evaluating a list of numbers to return sum, average, min, max,
        /// etc.  Some functions also work on text (min, max, mode, median).
        /// </summary>
        math_on_list,

        /// <summary>
        /// Block for remainder of a division.
        /// </summary>
        math_modulo,

        /// <summary>
        /// Block for constraining a number between two limits.
        /// </summary>
        math_constrain,

        /// <summary>
        /// Block for random integer between [X] and [Y].
        /// </summary>
        math_random_int,

        /// <summary>
        /// Block for random integer between [X] and [Y].
        /// </summary>
        math_random_float,

        /// <summary>
        /// Block for calculating atan2 of [X] and [Y].
        /// </summary>
        math_atan2,

        #endregion

        #region Procedures

        /// <summary>
        /// Block for defining a procedure with no return value.
        /// </summary>
        procedures_defnoreturn,

        /// <summary>
        /// Block for defining a procedure with a return value.
        /// </summary>
        procedures_defreturn,

        /// <summary>
        /// Mutator block for procedure container.
        /// </summary>
        procedures_mutatorcontainer,

        /// <summary>
        /// Mutator block for procedure argument.
        /// </summary>
        procedures_mutatorarg,

        /// <summary>
        /// Block for calling a procedure with no return value.
        /// </summary>
        procedures_callnoreturn,

        /// <summary>
        /// Block for calling a procedure with a return value.
        /// </summary>
        procedures_callreturn,

        /// <summary>
        /// Block for conditionally returning a value from a procedure.
        /// </summary>
        procedures_ifreturn,

        #endregion

        #region Text

        common_comment,
        text,
        text_with_continuation,
        text_multiline,
        text_join,
        text_append,
        text_length,
        text_isEmpty,
        text_indexOf,
        text_charAt,
        text_changeCase,
        text_trim,
        text_print,
        text_prompt_ext,
        text_prompt,
        text_count,
        text_replace,
        text_reverse,

        #endregion

        #region Systems

        #region process managment

        systems_os_system,
        systems_subprocess_call,

        #endregion

        #region utils and app

        common_app_echo,
        common_app_ls_dir,
        common_app_rhvoice,
        common_system_i2cdetect,

        #endregion

        #region specsymbol

        common_system_pipeline,
        common_system_literal,
        common_system_left_curly_brace,
        common_system_right_curly_brace,
        common_system_redirect_input,
        common_system_redirect_output,
        common_system_whitespace,
        common_system_single_quotation,
        common_system_double_quotation,
        common_system_one_single_quotation,

        #endregion

        #region path

        //common_system_example_flack_app,
        common_system_resources_music,

        #endregion

        #endregion

        #region Variables

        /// <summary>
        /// Block for variable getter
        /// </summary>
        variables_get,

        /// <summary>
        /// Block for variable setter
        /// </summary>
        variables_set,

        variable_with_continuation,

        #endregion

        #region VariablesDynamic

        /// <summary>
        /// Block for variable getter.
        /// </summary>
        variables_get_dynamic,

        /// <summary>
        /// Block for variable setter
        /// </summary>
        variables_set_dynamic,


        #endregion

        #region DateTime blocks

        time_sleep,

        #endregion

        #region Adam common bloks

        #region eye

        common_eye_pack,
        common_eye_pack_simple,
        common_write_i2c_block_data,
        common_import_smbus,
        common_eye_color,
        common_eye_diode_number,
        common_reg_constant,
        common_i2c_device_address,
        common_simple_write_i2c_block_data,

        #endregion

      

        #region music editor

        #region chord

        common_music_major_chord,
        common_music_minor_chord,
        common_music_minor_major_seventh_chords,
        common_music_minor_minor_seventh_chords,
        common_music_major_chord_extended,
        common_music_minor_chord_extended,
        common_music_minor_major_seventh_chords_extended,
        common_music_minor_minor_seventh_chords_extended,

        #endregion

        #region note

        common_music_note,
        common_music_note_extended,
        common_music_classic_note,
        common_music_classic_note_extended,

        #endregion

        #region mixer

        common_music_mixer_init,
        common_music_mixer_load,
        common_music_mixer_play,
        common_music_mixer_get_bussy,

        #endregion

        #region funсtions

        common_music_function_create_chord,
        common_music_function_create_instrument,
        common_music_function_play,

        #endregion

        #region specsymbols

        common_music_spes_symbols_with_numeric,
        common_music_spes_symbols,
        common_music_fraction,

        #endregion

        #endregion

        #region voice

        
        common_say_native,
        common_say_native_with_voice_param,
        common_say_sh,
        common_say_sh_with_voice_param,
        common_say_voices_list,

        #endregion

        # region file extension

        common_say_save_file_dp,


        #endregion

        #endregion

        #region Adam 3.0 blocks

        #region servo controller

        controller_motor_command,
        controller_angle_variable,
        controller_new_instance_class,
        controller_handle_command,
        controller_return_to_start_position_command,
        move_controller,
        move_command_set_speed,
        speed_value,
        speed_stop,
        move_controller_vector,
        move_vector_forward,
        speed_vector_value,
        move_vector_back,
        move_vector_right,
        move_vector_left,
        move_vector_right_and_forward,
        move_vector_left_and_forward,
        move_vector_left_and_back,
        move_vector_right_and_back,
        move_vector_turn_right,
        move_vector_turn_left,
        move_vector_stop,
        move_get_register,
        speed_clear_register,
        move_free_vector_variable,
        move_free_vector,

        #endregion

        #region servo const

        controller_const_head,
        controller_const_neck,
        controller_const_right_hand,
        controller_const_left_hand,
        controller_const_right_lower_arm_up,
        controller_const_left_lower_arm_up,
        controller_const_right_upper_arm,
        controller_const_left_upper_arm,
        controller_const_right_shoulder,
        controller_const_left_shoulder,
        controller_const_chest,
        controller_const_press,

        controller_const_left_upper_leg,
        controller_const_right_upper_leg,

        controller_const_left_lower_leg,
        controller_const_right_lower_leg,

        controller_const_left_foot,
        controller_const_right_foot,

        #endregion

        #region sensors

        adam_two_seven_sensor_temperature,
        adam_two_seven_sensor_acceleration,
        adam_two_seven_sensor_gyro,
        adam_two_seven_sensor_magnetometer,
        adam_two_seven_sensor_pressure,
        adam_two_seven_sensor_altitude,
        adam_two_seven_sensor_declaration_extended,
        adam_two_seven_bmp280_addr_const,
        adam_two_seven_sensor_set_pressure,
        adam_two_seven_sensor_mpu_9250_declaration,
        #endregion

        #endregion

        #region Adam 2.0 blocks

        #region servo block

        import_adam_servo_api_with_param,
        import_adam_servo_api,
        servo_common_function,
        servo_speed_variable,
        servo_angle_variable,
        

        servo_head,
        servo_head_param_func,

        servo_right_hand,
        servo_right_hand_short,
        servo_right_hand_short_param_func,

        servo_left_hand,
        servo_left_hand_short,
        servo_left_hand_short_param_func,

        servo_torso,
        servo_torso_param_func,

        servo_press,
        servo_press_short,
        servo_press_short_param_func,

        servo_legs,
        servo_legs_param_func, 

        servo_fingers,
        servo_fingers_param_func,

        servo_squeeze_fingers,
        servo_left_squeeze_fingers,
        servo_right_squeeze_fingers,
        servo_unclench_fingers,
        servo_left_unclench_fingers,
        servo_right_unclench_fingers,

        #endregion

        #region sensor

        common_i2c_sensor_device,
        common_i2c_sensor_device_extended,
        common_sensor_declaration,
        common_sensor_declaration_extended,

        common_sensor_temperature,
        common_sensor_acceleration,
        common_sensor_magnetometer,
        common_sensor_gyro,
        common_sensor_euler,
        common_sensor_quaternion,
        common_sensor_linear_acceleration,
        common_sensor_gravity,

        #endregion

        #region wheels

        wheels_left_leg,
        wheels_right_leg,
        
        wheels_left_leg_extended,
        wheels_right_leg_extended,

        #endregion

        #region rangefinder

        rangefinder_left,
        rangefinder_right,
        import_adam_rangefinders,
        common_import_rangefinders_i2c,
        rangefinder_get_distance_i2c,
        rangefinder_i2c_address,


        #endregion

        #region servo

        adam_import_servo_lib,
        adam_import_ping_variable,
        baudrate_variable,
        select_baudrate,

        #endregion

        #endregion
    }
}

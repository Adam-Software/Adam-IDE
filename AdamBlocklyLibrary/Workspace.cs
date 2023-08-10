using AdamBlocklyLibrary.Enum;

namespace AdamBlocklyLibrary
{
    public class Workspace
    {
        /// <summary>
        /// Allows blocks to be collapsed or expanded. Defaults to true if the toolbox has categories, false otherwise.
        /// </summary>
        public bool Collapse { get; set; }

        /// <summary>
        /// Tree structure of categories and blocks available to the user. Only CategoriesToolbox are supported.
        /// </summary>
        public Toolbox.Toolbox Toolbox { get; set; }

        /// <summary>
        /// Configures a grid which blocks may snap to.
        /// </summary>
        public BlocklyGrid BlocklyGrid { get; set; }

        /// <summary>
        /// Pre-installed sets of Blockly themes
        /// </summary>
        public BlocklyTheme Theme { get; set; }

        /// <summary>
        /// Displays or hides the trashcan. 
        /// Defaults to true if the toolbox has categories, false otherwise.
        /// </summary>
        public bool ShowTrashcan { get; set; }

        /// <summary>
        /// Determines the renderer used by blockly. Pre-packaged renderers include 'geras' (the default), 'thrasos', and 'zelos' (a scratch-like renderer).
        /// </summary>
        public Render Render { get; set; } = Render.zelos;
    }

}

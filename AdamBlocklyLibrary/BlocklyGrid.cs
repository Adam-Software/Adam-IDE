namespace AdamBlocklyLibrary
{

    public class BlocklyGrid
    {
        /// <summary>
        /// The most important grid property is spacing which defines the distance between the grid's points. 
        /// The default value is 0, which results in no grid.
        /// </summary>
        public int Spacing { get; set; } = 5;

        /// <summary>
        /// The length property is a number that defines the shape of the grid points. 
        /// A length of 0 results in an invisible grid (but still one that may be snapped to), 
        /// a length of 1 (the default value) results in dots, a longer length results in crosses, 
        /// and a length equal or greater than the spacing results in graph paper.
        /// </summary>
        public int Length { get; set; } = 5;

        /// <summary>
        /// The colour property is a string that sets the colour of the points. 
        /// Note the British spelling. Use any CSS-compatible format, including #f00, #ff0000, or rgb(255, 0, 0). rgb 0, 128, 0
        /// The default value is #888
        /// </summary>
        public string Colour { get; set; } = "#888";

        /// <summary>
        /// The snap property is a boolean that sets whether blocks should snap to the nearest grid point when placed on the workspace. 
        /// The default value is false.
        /// </summary>
        public bool Snap { get; set; } = false;

    }
}

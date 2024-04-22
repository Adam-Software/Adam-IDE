
using AdamBlocklyLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdamBlocklyLibrary.Toolbox
{
    public class CategoryToolbox
    {
        [JsonPropertyName("kind")]
        public string Kind { get; } = "category";

        /// <summary>
        /// Name toolbox
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("toolboxitemid")]
        public string ToolboxItemId => Guid.NewGuid().ToString();

        [JsonPropertyName("categorystyle")]
        public CategoryStyle CategoryStyle { get; set; }

        /// <summary>
        /// For set contents use <see cref="CategoryToolboxContents"/> or <see cref="SimpleToolboxContents"/>. 
        /// If <see cref="CategoryToolboxContents"/> is given, then <see cref="SimpleToolboxContents"/> in <see cref="Contents"/> will not be serialized
        /// </summary>
        [JsonPropertyName("contents")]
        public object Contents => CategoryToolboxContents ?? (object)SimpleToolboxContents;

        /// <summary>
        /// If <see cref="CategoryToolboxContents"/> is given, then <see cref="SimpleToolboxContents"/> in <see cref="Contents"/> will not be serialized
        /// </summary>
        [JsonIgnore]
        public IList<CategoryToolbox> CategoryToolboxContents  { get; set; }

        /// <summary>
        /// If <see cref="CategoryToolboxContents"/> is given, then <see cref="SimpleToolboxContents"/> in <see cref="Contents"/> will not be serialized
        /// </summary>
        [JsonIgnore]
        public IList<SimpleToolbox> SimpleToolboxContents { get; set;}

        /// <summary>
        /// Each category can be assigned a colour using the optional colour attribute. 
        /// Note the British spelling. The colour is a number (0-360) defining the hue
        /// By default categry null, is visible without color
        /// </summary>
        [JsonPropertyName("colour")]
        public string Colour { get; set; } = null;

        /// <summary>
        /// Categories are shown collapsed by default when Blockly is loaded, but a category may be expanded with this.
        /// Use expanded if <see cref="CategoryToolboxContents"/> is given. 
        /// </summary>
        [JsonIgnore]
        public bool Expanded { get; set; }

        /// <summary>
        /// For set ExpandedCategory use Expanded
        /// </summary>
        [JsonPropertyName("expanded")]
        public string ExpandedCategory => Expanded ? Expanded.ToString().ToLower() : null;

        /// <summary>
        /// A category can be hidden when the toolbox is first injected, or it can be hidden later on through JavaScript.
        /// </summary>
        [JsonIgnore]
        public bool Hidden { get; set; }

        /// <summary>
        /// For set HiddenCategory use Hidden
        /// </summary>
        [JsonPropertyName("hidden")]
        public string HiddenCategory => Hidden.ToString().ToLower();

    }
}

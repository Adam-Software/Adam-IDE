using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdamBlocklyLibrary.Toolbox
{
    public class Toolbox
    {
        [JsonPropertyName("kind")]
        public string Kind { get; private set; } = "categoryToolbox";

        /// <summary>
        /// For set contents use <see cref="CategoryToolboxContents"/> or <see cref="SimpleToolboxContents"/>. 
        /// If <see cref="CategoryToolboxContents"/> is given, then <see cref="SimpleToolboxContents"/> in <see cref="Contents"/> will not be serialized
        /// </summary>
        [JsonPropertyName("contents")]
        internal object Contents => CategoryToolboxContents ?? (object)SimpleToolboxContents;

        /// <summary>
        /// If <see cref="CategoryToolboxContents"/> is given, then <see cref="SimpleToolboxContents"/> in <see cref="Contents"/> will not be serialized
        /// </summary>
        [JsonIgnore]
        public IList<CategoryToolbox> CategoryToolboxContents { get; set; }

        /// <summary>
        /// If <see cref="CategoryToolboxContents"/> is given, then <see cref="SimpleToolboxContents"/> in <see cref="Contents"/> will not be serialized
        /// </summary>
        [JsonIgnore]
        public IList<SimpleToolbox> SimpleToolboxContents { get; set; }
    }
}

using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.ObjectModel;

namespace AdamStudio.Services.Interfaces
{
    public interface IAvalonEditService : IDisposable
    {
        /// <summary>
        /// Register highlighting for AvalonEdit. You need to call before loading the regions
        /// </summary>
        public void RegisterHighlighting(string highlightingName, byte[] xmlByteArray);
        public ReadOnlyCollection<IHighlightingDefinition> HighlightingDefinitions { get; }
        public IHighlightingDefinition GetDefinition(string name);
    }
}

using ICSharpCode.AvalonEdit.Highlighting;
using System;

namespace AdamController.Services.Interfaces
{
    public interface IAvalonEditService : IDisposable
    {
        public void RegisterHighlighting(string highlightingName, byte[] xmlByteArray);
    }
}

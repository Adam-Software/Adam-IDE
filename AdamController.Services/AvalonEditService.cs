using AdamController.Services.Interfaces;
using ICSharpCode.AvalonEdit.Highlighting;
using System;

namespace AdamController.Services
{
    public class AvalonEditService : IAvalonEditService
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void RegisterHighlighting(string name, string[] extensions, IHighlightingDefinition highlighting)
        {
            throw new NotImplementedException();
        }
    }
}

using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamController.Services.Interfaces
{
    public interface IAvalonEditService : IDisposable
    {
        public void RegisterHighlighting(string name, string[] extensions, IHighlightingDefinition highlighting);
    }
}

using AdamBlocklyLibrary.Properties;
using AdamController.Services.Interfaces;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;

namespace AdamController.Services
{
    public class AvalonEditService : IAvalonEditService
    {
        private readonly IFileManagmentService mFileManagmentService;
        //private readonly HighlightingManager mHighlightingManager;

        public AvalonEditService(IFileManagmentService fileManagmentService)
        {
            mFileManagmentService = fileManagmentService;
            //mHighlightingManager = new HighlightingManager();
        }

        
        public void RegisterHighlighting(string highlightingName, byte[] xmlByteArray)
        {
            var xml = mFileManagmentService.ReadTextAsXml(xmlByteArray);
            var definition = HighlightingLoader.Load(xml, HighlightingManager.Instance);
            
            HighlightingManager.Instance.RegisterHighlighting(highlightingName, Array.Empty<string>(), definition);   

            //mHighlightingManager.RegisterHighlighting(highlightingName, Array.Empty<string>(), definition);   
        }

        public void Dispose()
        {
        }

    }
}

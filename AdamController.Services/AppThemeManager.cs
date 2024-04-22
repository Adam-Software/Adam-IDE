using AdamController.Services.Interfaces;
using System;

namespace AdamController.Services
{
    public class AppThemeManager : IAppThemeManager
    {
        public AppThemeManager()
        {
            Name = "app";
        }

        public string Name { get; private set; }    
        public void Dispose()
        {
           
        }
    }
}

using System;

namespace AdamController.Services.Interfaces
{
    public interface IAppThemeManager : IDisposable
    {
        public string Name { get; }
    }
}

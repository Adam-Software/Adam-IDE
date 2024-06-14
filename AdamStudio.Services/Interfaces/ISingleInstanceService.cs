using System.Windows;

namespace AdamStudio.Services.Interfaces
{
    public interface ISingleInstanceService
    {
        public void Make(Application application, string appName, bool uniquePerUser = true);
    }
}

using AdamStudio.Services.Interfaces;
using System;
using System.Threading;

using System.Windows;

namespace AdamStudio.Services
{
    public class SingleInstanceService : ISingleInstanceService
    {
        private static bool AlreadyProcessedOnThisInstance;

        public void Make(Application application, string appName, bool uniquePerUser = true)
        {
            if (AlreadyProcessedOnThisInstance)
            {
                return;
            }
            AlreadyProcessedOnThisInstance = true;

            string eventName = uniquePerUser
                ? $"{appName}-{Environment.MachineName}-{Environment.UserDomainName}-{Environment.UserName}"
                : $"{appName}-{Environment.MachineName}";

            bool isSecondaryInstance = true;

            EventWaitHandle eventWaitHandle = null;

            try
            {
                eventWaitHandle = EventWaitHandle.OpenExisting(eventName);
            }
            catch
            {
                // This code only runs on the first instance.
                isSecondaryInstance = false;
            }

            if (isSecondaryInstance)
            {
                ActivateFirstInstanceWindow(eventWaitHandle);

                // Let's produce a non-interceptable exit (2009 year approach).
                Environment.Exit(0);
            }

            RegisterFirstInstanceWindowActivation(application, eventName);
        }

        private void ActivateFirstInstanceWindow(EventWaitHandle eventWaitHandle)
        {
            // Let's notify the first instance to activate its main window.
            _ = eventWaitHandle.Set();
        }

        private void RegisterFirstInstanceWindowActivation(Application app, string eventName)
        {
            EventWaitHandle eventWaitHandle = new(
                false,
                EventResetMode.AutoReset,
                eventName);

            _ = ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, WaitOrTimerCallback, app, Timeout.Infinite, false);

            eventWaitHandle.Close();
        }

        private void WaitOrTimerCallback(object state, bool timedOut)
        {
            Application app = (Application)state;

            _ = app.Dispatcher.BeginInvoke(new Action(() =>
            {
                _ = Application.Current.MainWindow.WindowState = WindowState.Normal;
                _ = Application.Current.MainWindow.Activate();
            }));
        }
    }
}

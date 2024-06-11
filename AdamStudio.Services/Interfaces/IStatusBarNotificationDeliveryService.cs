﻿using System;

namespace AdamStudio.Services.Interfaces
{
    #region Delegates

    public delegate void ChangeProgressRingStateEventHandler(object sender, bool newState);
    public delegate void NewCompileLogMessageEventHandler(object sender, string message);
    public delegate void NewAppLogMessageEventHandler(object sender, string message);
    public delegate void UpdateNotificationCounterEventHandler(object sender, int counter);

    #endregion

    public interface IStatusBarNotificationDeliveryService : IDisposable
    {
        #region Event

        public event ChangeProgressRingStateEventHandler RaiseChangeProgressRingStateEvent;
        public event NewCompileLogMessageEventHandler RaiseNewCompileLogMessageEvent;
        public event NewAppLogMessageEventHandler RaiseNewAppLogMessageEvent;
        public event UpdateNotificationCounterEventHandler RaiseUpdateNotificationCounterEvent;

        #endregion

        #region Public fields

        public bool ProgressRingStart { get; set; }
        public string CompileLogMessage { get; set; }
        public string AppLogMessage {  get; set; }
        public int NotificationCounter { get; set; }

        #endregion

        #region Public fields

        public void ResetNotificationCounter();

        #endregion
    }
}

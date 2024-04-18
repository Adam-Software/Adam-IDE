using AdamController.Services.Interfaces;
using AdamController.Services.PythonRemoteRunnerDependency;
using AdamController.Services.UdpClientServiceDependency;
using ControlzEx.Standard;
using System;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace AdamController.Services
{
    public partial class PythonRemoteRunnerService : IPythonRemoteRunnerService
    {
        #region Events

        public event PythonStandartOutputEventHandler RaisePythonStandartOutputEvent;
        public event PythonScriptExecuteStartEventHandler RaisePythonScriptExecuteStartEvent;
        public event PythonScriptExecuteFinishEventHandler RaisePythonScriptExecuteFinishEvent;

        #endregion

        #region Services

        private readonly IUdpClientService mUdpClientService;

        #endregion

        #region Const

        private const string cPattern = $"{cStartMessage}|{cErrorMessage}|{cFinishMessage}";
        private const string cStartMessage = "start";
        private const string cErrorMessage = "error";
        private const string cFinishMessage = "finish";

        private Regex mRegex;
        #endregion

        #region ~

        public PythonRemoteRunnerService(IUdpClientService udpClientService) 
        {
            mUdpClientService = udpClientService;
            
            Subscribe();

            mRegex = MyRegex();
        }


        #endregion

        [GeneratedRegex($"{cStartMessage}|{cErrorMessage}|{cFinishMessage}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)]
        private static partial Regex MyRegex();

        #region Public method

        public void Dispose()
        {
            Unsubscribe();
        }

        #endregion

        #region Private methods

        private void ParseEvents(Match match, string message)
        {
            switch (match.Value)
            {
                case cStartMessage:
                    OnRaisePythonScriptExecuteStartEvent();
                    break;

                case cErrorMessage:
                    break;

                case cFinishMessage:
                    {
                        var cleanMessage = message.Remove(0, 6);
                        RemoteCommandExecuteResult executeResult = cleanMessage.ToCommandResult();
                        OnRaisePythonScriptExecuteFinishEvent(executeResult);
                        break;
                    }
       
       
            }
        }

        private void ParseMessage(string message)
        {
            MatchCollection matches = mRegex.Matches(message);

            if(matches.Count > 0)
            {
                foreach(Match match in matches.Cast<Match>())
                {
                    ParseEvents(match, message);
                    return;
                }
            }

            OnRaisePythonStandartOutputEvent($"{message}\n");
        }

        #endregion

        #region Subscribses

        private void Subscribe()
        {
            mUdpClientService.RaiseUdpClientMessageEnqueueEvent += RaiseUdpClientMessageEnqueueEvent;
        }

        private void Unsubscribe() 
        {
            mUdpClientService.RaiseUdpClientMessageEnqueueEvent -= RaiseUdpClientMessageEnqueueEvent;
        }

        #endregion

        #region Event methods

        private void RaiseUdpClientMessageEnqueueEvent(object sender, ReceivedData data)
        {
            var @string = Encoding.UTF8.GetString(data.Buffer, (int)data.Offset, (int)data.Size);
            ParseMessage(@string);
        }

        #endregion

        #region OnRaise events

        protected virtual void OnRaisePythonStandartOutputEvent(string message)
        {
            PythonStandartOutputEventHandler raiseEvent = RaisePythonStandartOutputEvent;
            raiseEvent?.Invoke(this, message);  
        }

        protected virtual void OnRaisePythonScriptExecuteStartEvent()
        {
            PythonScriptExecuteStartEventHandler raiseEvent = RaisePythonScriptExecuteStartEvent;
            raiseEvent?.Invoke(this);
        }

        protected virtual void OnRaisePythonScriptExecuteFinishEvent(RemoteCommandExecuteResult remoteCommandExecuteResult)
        {
            PythonScriptExecuteFinishEventHandler raiseEvent = RaisePythonScriptExecuteFinishEvent;
            raiseEvent?.Invoke(this, remoteCommandExecuteResult);
        }



        #endregion
    }
}

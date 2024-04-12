﻿using AdamController.Core.Model;
using AdamController.Core.Mvvm;
using AdamController.Services.Interfaces;
using AdamController.WebApi.Client.v1;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ComputerVisionControlViewModel : RegionViewModelBase
    {
        #region Services

        private readonly ICommunicationProviderService mCommunicationProvider;

        #endregion

        #region private var

        private DelegateCommand<string> directionButtonCommandDown;
        private DelegateCommand<string> directionButtonCommandUp;

        #endregion

        #region ~

        public ComputerVisionControlViewModel(IRegionManager regionManager, IDialogService dialogService, ICommunicationProviderService communicationProvider) : base(regionManager, dialogService)
        {
            mCommunicationProvider = communicationProvider;
        }

        #endregion

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Comamand execute when button press
        /// </summary>
        public DelegateCommand<string> DirectionButtonCommandDown => directionButtonCommandDown ??= new DelegateCommand<string>(obj =>
        {
            var vectorSource = JsonConvert.DeserializeObject<VectorModel>(obj);
            
            if (vectorSource.Move.X == 1)
            {
                vectorSource.Move.X = SliderValue;
            }
            else if (vectorSource.Move.X == -1)
            {
                vectorSource.Move.X = -SliderValue;
            }
            
            if (vectorSource.Move.Y == 1)
            {
                vectorSource.Move.Y = SliderValue;
            }
            else if(vectorSource.Move.Y == -1)
            {
                vectorSource.Move.Y = -SliderValue;
            }
           
            if (vectorSource.Move.Z == 1)
            {
                vectorSource.Move.Z = SliderValue;
            }
            else if(vectorSource.Move.Z == -1)
            {
                vectorSource.Move.Z = -SliderValue;
            }
            
            var json = JsonConvert.SerializeObject(vectorSource);

            mCommunicationProvider.WebSocketSendTextMessage(json);
        }, canExecute =>  mCommunicationProvider.IsTcpClientConnected);

        /// <summary>
        /// Comamand execute when button up
        /// </summary>
        public DelegateCommand<string> DirectionButtonCommandUp => directionButtonCommandUp ??= new DelegateCommand<string>(obj => 
        {
            VectorModel vector = new()
            {
                Move = new VectorItem 
                { 
                    X = 0,
                    Y = 0,
                    Z = 0
                } 
            };

            var json = JsonConvert.SerializeObject(vector);
            mCommunicationProvider.WebSocketSendTextMessage(json);

        }, canExecute => mCommunicationProvider.IsTcpClientConnected);

        private DelegateCommand toZeroPositionCommand;
        public DelegateCommand ToZeroPositionCommand => toZeroPositionCommand ??= new DelegateCommand(async () =>
        {
            try
            {
                await BaseApi.StopPythonExecute();
                await BaseApi.MoveToZeroPosition();
            }
            catch
            {
            }

        }, () => mCommunicationProvider.IsTcpClientConnected);

        private float sliderValue;

        public float SliderValue 
        {
            get => sliderValue;
            set
            {
                if (sliderValue == value) return;

                sliderValue = value;

                SetProperty(ref sliderValue, value);
            } 
        }

        public string StopDirrection { get; private set; } = "{\"move\":{\"x\": 0, \"y\": 0, \"z\": 0}}";

        // left/right/up/down +
        public string ForwardDirection { get; private set; } = "{\"move\":{\"x\": 0, \"y\": 1, \"z\": 0}}";
        public string BackDirection { get; private set; } = "{\"move\":{\"x\": 0, \"y\": -1, \"z\": 0}}";
        public string LeftDirection { get; private set; } = "{\"move\":{\"x\": -1, \"y\": 0, \"z\": 0}}";
        public string RightDirection { get; private set; } = "{\"move\":{\"x\": 1, \"y\": 0, \"z\": 0}}";

        //
        public string ForwardLeftDirection { get; private set; } = "{\"move\":{\"x\": -1, \"y\": 1, \"z\": 0}}";
        public string ForwardRightDirection { get; private set; } = "{\"move\":{\"x\": 1, \"y\": 1, \"z\": 0}}";
        public string BackLeftDirection { get; private set; } = "{\"move\":{\"x\": -1, \"y\": -1, \"z\": 0}}";
        public string BackRightDirection { get; private set; } = "{\"move\":{\"x\": 1, \"y\": -1, \"z\": 0}}";

        //rotate +
        public string RotateRightDirrection { get; private set; } = "{\"move\":{\"x\": 0.0, \"y\": 0.0, \"z\": 1.0 }}";
        public string RotateLeftDirrection { get; private set; } = "{\"move\":{\"x\": 0.0, \"y\": 0.0, \"z\": -1.0}}";

        #endregion
    }


}

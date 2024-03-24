using AdamController.Core.Helpers;
using AdamController.Core.Model;
using AdamController.Core.Mvvm;
using AdamController.Services.Interfaces;
using AdamController.WebApi.Client.v1;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ComputerVisionControlViewModel : RegionViewModelBase
    {
        #region private var

        private DelegateCommand<string> directionButtonCommandDown;
        private DelegateCommand<string> directionButtonCommandUp;

        #endregion

        #region ~

        public ComputerVisionControlViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
            
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

            ComunicateHelper.WebSocketSendTextMessage(json);
        }, canExecute => ComunicateHelper.TcpClientIsConnected);

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
            ComunicateHelper.WebSocketSendTextMessage(json);

        }, canExecute => ComunicateHelper.TcpClientIsConnected);

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

        }, () => ComunicateHelper.TcpClientIsConnected);

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

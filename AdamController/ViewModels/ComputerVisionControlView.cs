using AdamController.Commands;
using AdamController.Helpers;
using AdamController.Model;
using AdamController.ViewModels.HamburgerMenu;
using AdamController.WebApi.Client.v1;
using Newtonsoft.Json;

namespace AdamController.ViewModels
{
    public class ComputerVisionControlView : HamburgerMenuItemView
    {

        private RelayCommand<string> directionButtonCommandDown;
        private RelayCommand<string> directionButtonCommandUp;

        public ComputerVisionControlView(HamburgerMenuView hamburgerMenuView) : base(hamburgerMenuView){}

        #region Commands

        /// <summary>
        /// Comamand execute when button press
        /// </summary>
        public RelayCommand<string> DirectionButtonCommandDown => directionButtonCommandDown ??= new RelayCommand<string>(obj =>
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
        public RelayCommand<string> DirectionButtonCommandUp => directionButtonCommandUp ??= new RelayCommand<string>(obj => 
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

        private RelayCommand toZeroPositionCommand;
        public RelayCommand ToZeroPositionCommand => toZeroPositionCommand ??= new RelayCommand(async obj =>
        {
            try
            {
                await BaseApi.StopPythonExecute();
                await BaseApi.MoveToZeroPosition();
            }
            catch
            {
            }

        }, canExecute => ComunicateHelper.TcpClientIsConnected);

        private float sliderValue;
        public float SliderValue 
        {
            get => sliderValue;
            set
            {
                if (sliderValue == value) return;

                sliderValue = value;
                OnPropertyChanged(nameof(SliderValue));
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

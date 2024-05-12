namespace AdamController.Services.TcpClientDependency
{
    public class TcpClientOption
    {
        /// <summary>
        /// The number of reconnections when the connection is lost
        /// </summary>
        public int ReconnectCount { get; set; }

        /// <summary>
        /// Reconnection timeout
        /// </summary>
        public int ReconnectTimeout { get;  set; }
    }
}

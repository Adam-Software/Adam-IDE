namespace AdamController.Services.AdamTcpClientDependency
{
    public class AdamTcpClientOption
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

using System;
using System.Net;
using System.Text;

namespace AdamController.Services.UdpClientServiceDependency
{
    public class ReceivedData : EventArgs
    {
        public ReceivedData(EndPoint endpoint, byte[] buffer, long offset, long size) 
        {
            Endpoint = endpoint;
            Buffer = buffer;
            Offset = offset;
            Size = size;
        }

        public EndPoint Endpoint { get; }
        public byte[] Buffer { get;  }
        public long Offset { get;  }
        public long Size { get;  }

        public override string ToString()
        {
            string @string = Encoding.UTF8.GetString(Buffer, (int) Offset, (int) Size);
            return @string;
        }
    }
}

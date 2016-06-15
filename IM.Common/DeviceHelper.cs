using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;

namespace IM.Common
{
    public class DeviceHelper
    {
        public static bool IsNetWorkAvaiable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        public static string GetLocalHost()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static int GetAvaiablePort()
        {
            TcpListener _listener = null;
            int _avaiablePort = 0;

            try
            {
                _listener = new TcpListener(IPAddress.Loopback, 0);
                _listener.Start();
                _avaiablePort = ((IPEndPoint)_listener.LocalEndpoint).Port;
            }
            finally
            {
                if (_listener != null)
                {
                    _listener.Stop();
                    _listener = null;
                }
            }

            return _avaiablePort;
        }
    }
}

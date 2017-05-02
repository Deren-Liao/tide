using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static System.Console;

namespace TestNetworkConnectivity
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkConnectivityTest("35.184.53.125", 5986);
        }

        public static void NetworkConnectivityTest(string ipAddress, int port)
        {
            using (TcpClient client = new TcpClient())
            {
                do
                {
                    try
                    {
                        client.Connect(ipAddress, port);
                        WriteLine($"Connected");
                        return;
                    }
                    catch (SocketException ex)
                    {
                        WriteLine(ex.ToString());
                        if (ex.SocketErrorCode == SocketError.ConnectionRefused || ex.SocketErrorCode == SocketError.TimedOut)
                        {
                        }
                        else
                        {
                            return;
                        }
                    }
                } while (true);
            }
        }
    }
}

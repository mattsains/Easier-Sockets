using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EasierSockets
{
    public delegate string Dispatcher(string rx);
    public class Sock
    {
        /// <summary>
        /// starts listening on the port specified
        /// </summary>
        /// <param name="port">The port to listen on</param>
        /// <param name="ip">The bind address. Default is all IPv4</param>
        /// TODO: Allow other types of comms, like UDP, IPv6, etc
        public Sock(int port, string ip = "any")
        {
            IPAddress IP;
            if (ip == "any")
                IP = IPAddress.Any;
            else
                try
                {
                    byte[] octets = new byte[4];
                    string[] exploded = ip.Split('.');
                    for (byte i = 0; i < 4; i++)
                        octets[i] = byte.Parse(exploded[i]);
                    IP = new IPAddress(octets);
                }
                catch (Exception)
                {
                    throw new Exception("IP Address malformed");
                }
            //IP Address should be valid by now, attempt a bind
            IPEndPoint remoteEP = new IPEndPoint(IP, port);
            Socket Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void ListenAsync(Dispatcher dispatch, char separator)
        {
            this.ListenAsync(dispatch,separator.ToString());
        }
        public void ListenAsync(Dispatcher dispatch, string separator)
        {

        }
    }
}

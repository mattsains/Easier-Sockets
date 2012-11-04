using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EasierSockets
{
    public class ClientSock
    {
        Socket sock;
        string separator;
        public ClientSock(string host, int port, string separator = "\n")
        {
            IPHostEntry hostip = Dns.GetHostEntry(host);
            IPAddress ip = hostip.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            //TODO: Allow different modes like UDP
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(endpoint);
            this.separator = separator;
        }
        public string send(string message)
        {
            byte[] tx = Encoding.ASCII.GetBytes(message + separator);
            sock.Send(tx);
            byte[] rx = { };
            sock.Receive(rx);
            return Encoding.ASCII.GetString(rx);
        }
        public ~ClientSock()
        {
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
    }
}